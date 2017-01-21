namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildDrawMoneyCommand : BasicCommand
    {
        private long m_Id = 0L;
        private int m_Money = 0;
        private int m_remainDrawMoney = 0;
        private int m_remainMoney = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildDrawMoneyCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.WithdrawDrawableMoney(this.m_Id, this.m_Money, out this.m_remainMoney, out this.m_remainDrawMoney);
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildDrawMoneyCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            message.WriteS32(this.m_remainMoney);
            message.WriteS32(this.m_remainDrawMoney);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = _Msg.ReadS64();
            this.m_Money = _Msg.ReadS32();
        }
    }
}

