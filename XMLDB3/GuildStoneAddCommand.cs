namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildStoneAddCommand : BasicCommand
    {
        private GuildStone m_GuildStone = null;
        private long m_Id = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildStoneAddCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.SetGuildStone(this.m_Id, this.m_GuildStone);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildStoneAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_GuildStone = GuildStoneSerializer.Serialize(_Msg);
        }
    }
}

