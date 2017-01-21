namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildStoneChangeCommand : BasicCommand
    {
        private long m_Id = 0L;
        private int m_RequiredGP = 0;
        private int m_RequiredMoney = 0;
        private int m_Result = -1;
        private int m_StoneType = 0;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildStoneChangeCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.ChangeGuildStone(this.m_Id, this.m_StoneType, this.m_RequiredMoney, this.m_RequiredGP);
            return (this.m_Result == 0);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildStoneChangeCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result == 0)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            message.WriteS32(this.m_Result);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = (long) _Msg.ReadU64();
            this.m_StoneType = _Msg.ReadS32();
            this.m_RequiredMoney = _Msg.ReadS32();
            this.m_RequiredGP = _Msg.ReadS32();
        }
    }
}

