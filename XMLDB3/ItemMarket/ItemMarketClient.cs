namespace XMLDB3.ItemMarket
{
    using Mabinogi.Network;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class ItemMarketClient
    {
        protected Socket m_ClientSocket = null;
        private Mutex m_CloseMutex = new Mutex();
        private bool m_Closing = false;
        protected byte m_ReadCount = 0;
        protected volatile bool m_Running = false;
        protected ConnectionState m_State = ConnectionState.NotInitialized;
        private IPAddress m_TargetIP = null;
        private int m_TargetPort = 0;
        protected byte m_WriteCount = 0;
        public ClientEvent OnClosed = null;
        public ClientEvent OnConnectionFailed = null;
        public ClientEvent OnInitialized = null;
        public ClientEvent OnInitializeFailed = null;

        protected bool Connect(IPAddress _IP, int _Port)
        {
            bool flag;
            try
            {
                lock (this)
                {
                    if (this.m_Running)
                    {
                        throw new Exception(string.Concat(new object[] { "already connected to ", this.m_TargetIP.ToString(), " : ", this.m_TargetPort }));
                    }
                    EndPoint remoteEP = new IPEndPoint(_IP, _Port);
                    this.m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.m_ClientSocket.Connect(remoteEP);
                    this.m_Running = true;
                    this.m_State = ConnectionState.JustConnected;
                    this.m_TargetIP = _IP;
                    this.m_TargetPort = _Port;
                    this.m_WriteCount = 0;
                    this.m_ReadCount = 0;
                    new AsynchReceiveObject(this).Receive();
                    this.OnConnect();
                    flag = true;
                }
            }
            catch (SocketException exception)
            {
                if (this.m_Running)
                {
                    throw exception;
                }
                this.m_ClientSocket.Close();
                this.m_ClientSocket = null;
                if (this.OnClosed != null)
                {
                    this.OnClosed(this, 0);
                }
                flag = false;
            }
            return flag;
        }

        public bool ConnectIP(string _Address, int _Port)
        {
            IPAddress address = IPAddress.Parse(_Address);
            return this.Connect(address, _Port);
        }

        protected virtual void OnConnect()
        {
        }

        protected virtual int OnReceive(byte[] _buffer, int _length)
        {
            return 0;
        }

        private static void ReceiveCallBack(IAsyncResult ar)
        {
            if (ar != null)
            {
                AsynchReceiveObject asyncState = (AsynchReceiveObject) ar.AsyncState;
                if (asyncState != null)
                {
                    asyncState.Receive(ar);
                }
            }
        }

        public void Send(byte[] _Buffer, int _Length)
        {
            if (this.m_Running)
            {
                ItemMarketClient client;
                Monitor.Enter(client = this);
                try
                {
                    AsynchSendObject state = new AsynchSendObject(this, _Buffer, _Buffer.Length);
                    this.m_ClientSocket.BeginSend(_Buffer, 0, _Buffer.Length, SocketFlags.None, new AsyncCallback(ItemMarketClient.SendCallBack), state);
                }
                catch (SocketException exception)
                {
                    if (this.m_ClientSocket.Connected)
                    {
                        throw exception;
                    }
                    this.Stop();
                }
                catch (ObjectDisposedException)
                {
                }
                finally
                {
                    Monitor.Exit(client);
                }
            }
        }

        private static void SendCallBack(IAsyncResult ar)
        {
            if (ar != null)
            {
                AsynchSendObject asyncState = (AsynchSendObject) ar.AsyncState;
                if (asyncState != null)
                {
                    asyncState.Send(ar);
                }
            }
        }

        public void Stop()
        {
            Mutex mutex;
            lock ((mutex = this.m_CloseMutex))
            {
                if (this.m_Closing)
                {
                    return;
                }
                this.m_Closing = true;
            }
            try
            {
                ConnectionState state = this.m_State;
                lock (this)
                {
                    this.m_Running = false;
                    this.m_State = ConnectionState.NotInitialized;
                    if (this.m_ClientSocket != null)
                    {
                        if (this.m_ClientSocket.Connected)
                        {
                            this.m_ClientSocket.Shutdown(SocketShutdown.Both);
                            this.m_ClientSocket.Close();
                        }
                        this.m_ClientSocket = null;
                    }
                }
                switch (state)
                {
                    case ConnectionState.JustConnected:
                        if (this.OnConnectionFailed != null)
                        {
                            this.OnConnectionFailed(this, 0);
                        }
                        return;

                    case ConnectionState.Initialized:
                        if (this.OnClosed != null)
                        {
                            this.OnClosed(this, 0);
                        }
                        return;
                }
            }
            finally
            {
                lock ((mutex = this.m_CloseMutex))
                {
                    this.m_Closing = false;
                }
            }
        }

        public IPAddress Address
        {
            get
            {
                return this.m_TargetIP;
            }
        }

        public int Port
        {
            get
            {
                return this.m_TargetPort;
            }
        }

        private class AsynchReceiveObject
        {
            private byte[] m_Buffer = new byte[0x400];
            private NetworkBuffer m_DataBuffer = new NetworkBuffer();
            private ItemMarketClient m_Instance;
            private Socket m_WorkSocket;

            public AsynchReceiveObject(ItemMarketClient _Instance)
            {
                this.m_Instance = _Instance;
                this.m_WorkSocket = this.m_Instance.m_ClientSocket;
            }

            public void Receive()
            {
                try
                {
                    this.m_WorkSocket.BeginReceive(this.m_Buffer, 0, this.m_Buffer.Length, SocketFlags.None, new AsyncCallback(ItemMarketClient.ReceiveCallBack), this);
                }
                catch (SocketException exception)
                {
                    if (this.m_WorkSocket.Connected)
                    {
                        throw exception;
                    }
                    this.m_Instance.Stop();
                }
                catch (ObjectDisposedException)
                {
                }
            }

            public void Receive(IAsyncResult ar)
            {
                ItemMarketClient client;
                Monitor.Enter(client = this.m_Instance);
                try
                {
                    int num = this.m_WorkSocket.EndReceive(ar);
                    if (num > 0)
                    {
                        this.m_DataBuffer.AddBuffer(this.m_Buffer, num);
                        for (int i = this.m_Instance.OnReceive(this.m_DataBuffer.GetBuffer(), this.m_DataBuffer.GetBufSize()); i != 0; i = this.m_Instance.OnReceive(this.m_DataBuffer.GetBuffer(), this.m_DataBuffer.GetBufSize()))
                        {
                            this.m_DataBuffer.PopBuffer(i);
                        }
                    }
                    else
                    {
                        this.m_Instance.Stop();
                        return;
                    }
                    this.m_WorkSocket.BeginReceive(this.m_Buffer, 0, this.m_Buffer.Length, SocketFlags.None, new AsyncCallback(ItemMarketClient.ReceiveCallBack), this);
                }
                catch (SocketException exception)
                {
                    if (this.m_WorkSocket.Connected)
                    {
                        throw exception;
                    }
                    this.m_Instance.Stop();
                }
                catch (ObjectDisposedException)
                {
                }
                finally
                {
                    Monitor.Exit(client);
                }
            }
        }

        private class AsynchSendObject
        {
            private ItemMarketClient m_Instance;
            private byte[] m_SendBuffer = null;
            private int m_SendBufSize = 0;
            private Socket m_WorkSocket;

            public AsynchSendObject(ItemMarketClient _Instance, byte[] _buffer, int _buffersize)
            {
                this.m_Instance = _Instance;
                this.m_WorkSocket = this.m_Instance.m_ClientSocket;
                this.m_SendBuffer = _buffer;
                this.m_SendBufSize = _buffersize;
            }

            private bool IsValid()
            {
                return (this.m_Instance != null);
            }

            public void Send(IAsyncResult ar)
            {
                if (this.IsValid())
                {
                    lock (this.m_Instance)
                    {
                        if (this.m_WorkSocket != null)
                        {
                            try
                            {
                                this.m_WorkSocket.EndSend(ar);
                            }
                            catch (ObjectDisposedException)
                            {
                            }
                            catch (SocketException exception)
                            {
                                if (this.m_WorkSocket.Connected)
                                {
                                    throw exception;
                                }
                            }
                        }
                    }
                }
            }
        }

        public delegate void ClientEvent(object _arg, int _result);

        protected enum ConnectionState
        {
            NotInitialized,
            JustConnected,
            Initialized
        }

        private class ForceTerminateException : Exception
        {
        }
    }
}

