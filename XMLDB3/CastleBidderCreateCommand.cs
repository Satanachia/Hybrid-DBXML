namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBidderCreateCommand : BasicCommand
    {
        private CastleBidder m_CastleBidder = null;
        private int m_RemainMoney = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 성 입찰자를 생성합니다.");
            this.m_Result = QueryManager.Castle.CreateBidder(this.m_CastleBidder, QueryManager.Guild, ref this.m_RemainMoney);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 성 입찰자를 생성하였습니다.");
                return true;
            }
            WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 성 입찰자를 생성하는데 실패하였습니다.");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleBidderCreateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                message.WriteS32(this.m_RemainMoney);
                return message;
            }
            if (this.m_Result == REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8(0);
                message.WriteS32(this.m_RemainMoney);
            }
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_CastleBidder = CastleBidderSerializer.Serialize(_Msg);
        }
    }
}

