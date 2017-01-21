namespace XMLDB3
{
    using Mabinogi;
    using Mabinogi.Network;
    using System;
    using System.Collections;
    using System.Threading;

    public class CommandRedirection
    {
        private static bool bActive = false;
        private static Hashtable clients = new Hashtable();
        private static int port = 0;
        private static string server = null;
        private static Hashtable timers = new Hashtable();

        public static void CreateClient(int _clientID)
        {
            if (Enabled)
            {
                ClientHandler handler = new ClientHandler();
                try
                {
                    if (handler.ConnectIP(server, port))
                    {
                        lock (clients.SyncRoot)
                        {
                            if (clients.ContainsKey(_clientID))
                            {
                                ((ClientHandler) clients[_clientID]).Stop();
                            }
                            clients[_clientID] = handler;
                            return;
                        }
                    }
                    ReserveConnect(_clientID);
                }
                catch (Exception)
                {
                }
            }
        }

        public static void DestroyClient(int _clientID)
        {
            try
            {
                ClientHandler handler = null;
                lock (clients.SyncRoot)
                {
                    if (clients.ContainsKey(_clientID))
                    {
                        handler = (ClientHandler) clients[_clientID];
                        clients.Remove(_clientID);
                    }
                }
                if (handler != null)
                {
                    handler.Stop();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void Init(string _server, int _port)
        {
            Stop();
            if ((_server != string.Empty) && (_port != 0))
            {
                server = _server;
                port = _port;
                clients = new Hashtable();
                timers = new Hashtable();
                bActive = true;
            }
            else
            {
                bActive = false;
            }
        }

        private static void Reconnect(object _state)
        {
            int key = (int) _state;
            bool flag = false;
            lock (timers.SyncRoot)
            {
                if (timers.ContainsKey(key))
                {
                    timers.Remove(key);
                    flag = true;
                }
            }
            if (flag)
            {
                CreateClient(key);
            }
        }

        private static void ReserveConnect(int _clientID)
        {
            lock (timers.SyncRoot)
            {
                if (!timers.ContainsKey(_clientID))
                {
                    Timer timer = new Timer(new TimerCallback(CommandRedirection.Reconnect), _clientID, 0x2710, -1);
                    timers[_clientID] = timer;
                }
            }
        }

        public static void SendMessage(int _clientID, Message _msg)
        {
            if (Enabled)
            {
                object obj2;
                ClientHandler handler = null;
                lock ((obj2 = clients.SyncRoot))
                {
                    if (clients.ContainsKey(_clientID))
                    {
                        handler = (ClientHandler) clients[_clientID];
                    }
                }
                if (handler != null)
                {
                    try
                    {
                        handler.SendMessage(_msg);
                        return;
                    }
                    catch (Exception)
                    {
                        lock ((obj2 = clients.SyncRoot))
                        {
                            if (clients.ContainsKey(_clientID))
                            {
                                clients.Remove(_clientID);
                                ReserveConnect(_clientID);
                            }
                        }
                        return;
                    }
                }
                ReserveConnect(_clientID);
            }
        }

        public static void Stop()
        {
            lock (clients.SyncRoot)
            {
                foreach (ClientHandler handler in clients.Values)
                {
                    handler.Stop();
                }
                clients.Clear();
                lock (timers.SyncRoot)
                {
                    foreach (Timer timer in clients.Values)
                    {
                        timer.Dispose();
                    }
                    timers.Clear();
                }
                bActive = false;
            }
        }

        public static bool Enabled
        {
            get
            {
                return bActive;
            }
        }
    }
}

