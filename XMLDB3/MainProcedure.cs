namespace XMLDB3
{
    using Mabinogi;
    using Mabinogi.Network;
    using System;

    public class MainProcedure : ServerHandler
    {
        private static MainProcedure m_MainServer = new MainProcedure();

        public static void CloseConnection(int _id)
        {
            m_MainServer.DestroyClient(_id);
        }

        protected override void OnClose(int _id)
        {
            if (CommandRedirection.Enabled)
            {
                CommandRedirection.DestroyClient(_id);
            }
        }

        protected override void OnConnect(int _id)
        {
            if (CommandRedirection.Enabled)
            {
                CommandRedirection.CreateClient(_id);
            }
        }

        protected override void OnExceptionRaised(int _id, Exception _ex)
        {
            ExceptionMonitor.ExceptionRaised(_ex, _id);
        }

        protected override void OnReceive(int _id, Message _msg)
        {
            BasicCommand command = null;
            try
            {
                command = BasicCommand.Parse(_id, _msg);
                if (command != null)
                {
                    try
                    {
                        if (command.IsPrimeCommand)
                        {
                            ProcessManager.AddPrimeCommand(command);
                        }
                        else
                        {
                            ProcessManager.AddCommand(command);
                        }
                    }
                    catch (Exception exception)
                    {
                        command.OnError();
                        throw exception;
                    }
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, command);
            }
            try
            {
                if (CommandRedirection.Enabled)
                {
                    CommandRedirection.SendMessage(_id, _msg);
                }
            }
            catch
            {
            }
        }

        public static void ServerSend(int _id, Message _msg)
        {
            WorkSession.WriteStatus("ServerSend(" + _id + ", msg) : 함수에 진입하였습니다");
            m_MainServer.SendMessage(_id, _msg);
        }

        public static void ServerStart(int port)
        {
            m_MainServer.Start(port);
        }

        public static void ServerStop()
        {
            m_MainServer.Stop();
        }

        public static ServerInstanceInfo[] ConnectionInfo
        {
            get
            {
                return m_MainServer.InstanceInfo;
            }
        }

        public static ServerHandlerInfo ServerInfo
        {
            get
            {
                return m_MainServer.StatisticInfo;
            }
        }
    }
}

