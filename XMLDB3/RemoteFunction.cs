namespace XMLDB3
{
    using Mabinogi;
    using System;

    public abstract class RemoteFunction
    {
        private Message m_InputMsg = null;
        private string m_Name;
        private int m_NetworkId = 0;
        private int m_QueryKey;
        private Command m_ReplyCommand = Command.MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID;

        protected RemoteFunction()
        {
        }

        protected abstract string BuildName(Message _input);
        protected Message GetNewReply()
        {
            Message message = new Message((uint) this.m_ReplyCommand, 0L);
            message.WriteS32(this.m_QueryKey);
            return message;
        }

        private static Command MatchReplyCommand(Command _requestcmd)
        {
            switch (_requestcmd)
            {
                case Command.MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REPLY;

                case Command.MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REPLY;

                case Command.MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REPLY;

                case Command.MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REQUEST:
                    return Command.MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REPLY;
            }
            return Command.MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID;
        }

        public static RemoteFunction Parse(int _networkid, Message _inputmsg)
        {
            RemoteFunction function = null;
            switch (_inputmsg.ID)
            {
                case 100:
                    function = new RemoteFunction_CacheInfo();
                    break;

                case 0x65:
                    function = new RemoteFunction_ExceptionInfo();
                    break;

                case 0x66:
                    function = new RemoteFunction_NetworkInfo();
                    break;

                case 0x67:
                    function = new RemoteFunction_SessionInfo();
                    break;

                default:
                    function = null;
                    break;
            }
            if (function != null)
            {
                function.m_NetworkId = _networkid;
                function.m_InputMsg = _inputmsg;
                function.m_ReplyCommand = MatchReplyCommand((Command) _inputmsg.ID);
                function.m_QueryKey = _inputmsg.ReadS32();
                function.m_Name = function.BuildName(_inputmsg.Clone());
            }
            return function;
        }

        public abstract Message Process();
        protected void Reply(Message _input)
        {
            MonitorProcedure.ServerSend(this.m_NetworkId, _input);
        }

        protected Message Input
        {
            get
            {
                return this.m_InputMsg;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        private enum Command
        {
            MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REPLY = 0x68,
            MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REQUEST = 100,
            MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REPLY = 0x69,
            MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REQUEST = 0x65,
            MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REPLY = 0x6a,
            MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REQUEST = 0x66,
            MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID = -1,
            MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REPLY = 0x6b,
            MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REQUEST = 0x67
        }
    }
}

