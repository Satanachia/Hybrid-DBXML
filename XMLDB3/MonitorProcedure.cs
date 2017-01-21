namespace XMLDB3
{
    using Mabinogi;
    using Mabinogi.Network;
    using System;

    public class MonitorProcedure : ServerHandler
    {
        private static MonitorProcedure m_MainServer = new MonitorProcedure();

        public static void CloseConnection(int _id)
        {
            m_MainServer.DestroyClient(_id);
        }

        protected override void OnClose(int _id)
        {
        }

        protected override void OnConnect(int _id)
        {
        }

        protected override void OnReceive(int _id, Message _msg)
        {
            try
            {
                RemoteFunction function = RemoteFunction.Parse(_id, _msg);
                if (function == null)
                {
                    throw new Exception(string.Concat(new object[] { "connection ", _id, " send invalid message ", _msg.ID }));
                }
                try
                {
                    WorkSession.Begin(function.Name, null, _id);
                    function.Process();
                }
                finally
                {
                    WorkSession.End();
                }
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
            }
        }

        public static void ServerSend(int _id, Message _msg)
        {
            m_MainServer.SendMessage(_id, _msg);
        }

        public static void ServerStart(int _port)
        {
            m_MainServer.Start(_port);
        }

        public static void ServerStop()
        {
            m_MainServer.Stop();
        }
    }
}

