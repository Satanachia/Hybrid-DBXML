namespace XMLDB3.ItemMarket
{
    using System;
    using System.Collections;
    using System.Threading;
    using XMLDB3;

    public class ItemMarketManager
    {
        private static ArrayList connectionPool = null;
        private static int connectionPoolSize = 0;
        private static int gameNo;
        private static bool initialized = false;
        private static int roundRobin = 0;
        private static bool running = false;
        private static string serverIP;
        private static int serverNo;
        private static short serverPort;
        private static object syncObj = new object();

        public static void CheckHandlers()
        {
            Console.WriteLine("Checking Item Market Handlers...");
            ArrayList list = null;
            lock (syncObj)
            {
                list = (ArrayList) connectionPool.Clone();
            }
            foreach (ItemMarketHandler handler in list)
            {
                if (!handler.IsWorking)
                {
                    Console.WriteLine("Invalid handler [{0}] found. Shutting down...", handler.Name);
                    OnClientClosed(handler, -1);
                }
            }
            Console.WriteLine("Done...");
        }

        public static void Connect(ItemMarketHandler _handler)
        {
            if (_handler != null)
            {
                Console.WriteLine("Item Market Client [{0}] is reconnecting...", _handler.Name);
                try
                {
                    _handler.ConnectIP(serverIP, serverPort);
                }
                catch (Exception exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                    ScheduleReconnect(_handler);
                }
            }
            else
            {
                Console.WriteLine("Item Market Client is null...");
            }
        }

        public static ItemMarketHandler GetHandler()
        {
            lock (syncObj)
            {
                if (connectionPool.Count > 0)
                {
                    return (ItemMarketHandler) connectionPool[roundRobin++ % connectionPool.Count];
                }
                return null;
            }
        }

        public static bool Init(int _gameNo, int _serverNo, string _serverIP, short _serverPort, int _connectionPoolSize, int _codePage)
        {
            Console.WriteLine("Item Market's Enabled.");
            PacketHelper.Init(_codePage);
            if (_connectionPoolSize == 0)
            {
                _connectionPoolSize = 1;
            }
            gameNo = _gameNo;
            serverNo = _serverNo;
            serverIP = _serverIP;
            serverPort = _serverPort;
            connectionPool = new ArrayList(_connectionPoolSize);
            try
            {
                for (int i = 0; i < _connectionPoolSize; i++)
                {
                    ItemMarketHandler handler = new ItemMarketHandler(i.ToString(), _gameNo, _serverNo);
                    handler.OnClosed = new ItemMarketClient.ClientEvent(ItemMarketManager.OnClientClosed);
                    handler.OnInitialized = new ItemMarketClient.ClientEvent(ItemMarketManager.OnClientConnected);
                    handler.OnConnectionFailed = new ItemMarketClient.ClientEvent(ItemMarketManager.OnConnectionFailed);
                    handler.OnInitializeFailed = new ItemMarketClient.ClientEvent(ItemMarketManager.OnInitialzeFailed);
                    Console.WriteLine("Item Market Client [{0}]'s Connecting...", handler.Name);
                    if (!handler.ConnectIP(serverIP, serverPort))
                    {
                        return false;
                    }
                }
                while (_connectionPoolSize > connectionPoolSize)
                {
                    Thread.Sleep(100);
                }
                if (connectionPoolSize >= connectionPool.Count)
                {
                    initialized = true;
                }
                running = true;
                return initialized;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                return false;
            }
        }

        public static void OnClientClosed(object _arg, int _result)
        {
            ExceptionMonitor.ExceptionRaised(new Exception(string.Format("Item Market Client [{0}]'s closed with {1}.", ((ItemMarketHandler) _arg).Name, _result)));
            lock (syncObj)
            {
                connectionPool.Remove(_arg);
            }
            if (!initialized)
            {
                Interlocked.Increment(ref connectionPoolSize);
            }
            ScheduleReconnect((ItemMarketHandler) _arg);
        }

        public static void OnClientConnected(object _arg, int _result)
        {
            ExceptionMonitor.ExceptionRaised(new Exception(string.Format("Item Market Client [{0}]'s Initialized.", ((ItemMarketHandler) _arg).Name)));
            lock (syncObj)
            {
                connectionPool.Add(_arg);
            }
            if (!initialized)
            {
                Interlocked.Increment(ref connectionPoolSize);
            }
        }

        public static void OnConnectionFailed(object _arg, int _result)
        {
            Console.WriteLine("Item Market Client [{0}]'s failed to connect with {1}.", ((ItemMarketHandler) _arg).Name, _result);
            if (!initialized)
            {
                Interlocked.Increment(ref connectionPoolSize);
            }
            ScheduleReconnect((ItemMarketHandler) _arg);
        }

        public static void OnInitialzeFailed(object _arg, int _result)
        {
            Console.WriteLine("Item Market Client [{0}]'s failed to initialize with {1}.", ((ItemMarketHandler) _arg).Name, _result);
        }

        private static void ScheduleReconnect(ItemMarketHandler _handler)
        {
            if (running)
            {
                Console.WriteLine("Item Market Client [{0}] schedule reconnecting...", _handler.Name);
                _handler.ScheduleReconnect();
            }
        }

        public static void Stop()
        {
            initialized = false;
            running = false;
            ArrayList list = null;
            lock (syncObj)
            {
                list = (ArrayList) connectionPool.Clone();
            }
            if (list != null)
            {
                foreach (ItemMarketHandler handler in list)
                {
                    handler.Stop();
                }
            }
            connectionPoolSize = 0x7fffffff;
        }
    }
}

