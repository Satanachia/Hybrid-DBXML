namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildMemberCheckJointimeCommand : BasicCommand
    {
        private long m_Id = 0L;
        private bool m_Result = false;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildMemberCheckJointimeCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.CheckMemberJointime(this.m_Id, this.m_Server);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildMemberCheckJointimeCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = (long) _Msg.ReadU64();
            this.m_Server = _Msg.ReadString();
        }
    }
}

