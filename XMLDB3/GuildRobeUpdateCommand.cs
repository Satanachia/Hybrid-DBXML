namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildRobeUpdateCommand : BasicCommand
    {
        private byte m_ErrorCode = 0;
        private int m_GuildMoney = 0;
        private int m_GuildPoint = 0;
        private GuildRobe m_GuildRobe = null;
        private long m_Id = 0L;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildRobeUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.UpdateGuildRobe(this.m_Id, this.m_GuildPoint, this.m_GuildMoney, this.m_GuildRobe, out this.m_ErrorCode);
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildRobeUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            message.WriteU8(this.m_ErrorCode);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_Id = (long) _message.ReadU64();
            this.m_GuildPoint = _message.ReadS32();
            this.m_GuildMoney = _message.ReadS32();
            this.m_GuildRobe = GuildRobeSerializer.Serialize(_message);
        }
    }
}

