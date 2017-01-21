namespace XMLDB3
{
    using Mabinogi;
    using Mabinogi.Network;
    using System;
    using System.Collections;
    using System.Net.Sockets;
    using System.Threading;

    public class Profiler : ServerHandler
    {
        private SortedList m_ClientList = new SortedList();
        private static Profiler m_MainProfiler = new Profiler();

        private Profiler()
        {
        }

        public static void AddProfileString(string _id)
        {
            Message message = new Message(0, 0L);
            message.WriteString(_id);
            message.WriteString((Thread.CurrentThread.Name == null) ? string.Empty : Thread.CurrentThread.Name);
            m_MainProfiler.BroadCast(message);
        }

        private void BroadCast(Message _msg)
        {
            ProfilerClient[] array = null;
            lock (this.m_ClientList.SyncRoot)
            {
                if (this.m_ClientList.Count > 0)
                {
                    array = new ProfilerClient[this.m_ClientList.Count];
                    this.m_ClientList.Values.CopyTo(array, 0);
                }
            }
            if (array != null)
            {
                foreach (ProfilerClient client in array)
                {
                    try
                    {
                        client.SendProfile(this, _msg);
                    }
                    catch (SocketException exception)
                    {
                        if (exception.ErrorCode == 0x2747)
                        {
                            base.DestroyClient(client.ID);
                        }
                        ExceptionMonitor.ExceptionRaised(exception);
                    }
                    catch (Exception exception2)
                    {
                        ExceptionMonitor.ExceptionRaised(exception2);
                    }
                }
            }
        }

        protected override void OnClose(int _id)
        {
            lock (this.m_ClientList.SyncRoot)
            {
                this.m_ClientList.Remove(_id);
            }
        }

        protected override void OnConnect(int _id)
        {
            ProfilerClient client = new ProfilerClient(_id);
            lock (this.m_ClientList.SyncRoot)
            {
                this.m_ClientList.Add(_id, client);
            }
        }

        protected override void OnReceive(int _id, Message _msg)
        {
            ProfilerClient client = null;
            if (this.m_ClientList.ContainsKey(_id))
            {
                client = (ProfilerClient) this.m_ClientList[_id];
            }
            else
            {
                return;
            }
            if (_msg.ID == 100)
            {
                int num = client.ResetSendCount();
                Message message = new Message(200, 0L);
                message.WriteS32(num);
                base.SendMessage(_id, message);
            }
        }

        public static void ServerSend(int _id, Message _msg)
        {
            m_MainProfiler.SendMessage(_id, _msg);
        }

        public static void ServerStart(int _port)
        {
            m_MainProfiler.Start(_port);
        }

        public static void ServerStop()
        {
            m_MainProfiler.Stop();
        }

        private enum Command
        {
            PROFILER_COMMAND_PING_REPLY = 200,
            PROFILER_COMMAND_PING_REQUEST = 100
        }
    }
}

