namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildUpdateCommand : BasicCommand
    {
        private int m_Flag = 0;
        private int m_Gold = 0;
        private int m_GP = 0;
        private long m_Id = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.UpdateGuildProperties(this.m_Id, this.m_GP, this.m_Gold, this.m_Flag);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8(this.m_Result ? ((byte) 1) : ((byte) 0));
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = (long) _Msg.ReadU64();
            this.m_GP = _Msg.ReadS32();
            this.m_Gold = _Msg.ReadS32();
            this.m_Flag = _Msg.ReadS8();
        }
    }
}

