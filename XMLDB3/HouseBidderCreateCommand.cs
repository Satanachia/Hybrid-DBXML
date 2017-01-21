namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidderCreateCommand : SerializedCommand
    {
        private byte m_ErrorCode = 0;
        private HouseBidder m_HouseBidder = null;
        private long m_HouseID = 0L;
        private int m_RemainMoney = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("HouseBidderCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBidderCreateCommand.DoProcess() : 집 입찰자를 생성합니다.");
            this.m_Result = QueryManager.House.CreateBidder(this.m_HouseID, this.m_HouseBidder, QueryManager.Bank, out this.m_ErrorCode, out this.m_RemainMoney);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("HouseBidderCreateCommand.DoProcess() : 집 입찰자를 생성하였습니다.");
                BankCache cache = (BankCache) ObjectCache.Bank.Find(this.m_HouseBidder.bidAccount);
                if ((cache != null) && cache.IsValid())
                {
                    cache.bank.deposit = this.m_RemainMoney;
                }
                return true;
            }
            WorkSession.WriteStatus("HouseBidderCreateCommnad.DoProcess() : 집 입찰자를 생성하는데 실패하였습니다.");
            return false;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_HouseID = _Msg.ReadS64();
            this.m_HouseBidder = HouseBidderSerializer.Serialize(_Msg);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBidderCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
                message.WriteU8(this.m_ErrorCode);
                if (this.m_ErrorCode == 0)
                {
                    message.WriteS32(this.m_RemainMoney);
                }
            }
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.StringIDRegistant(this.m_HouseBidder.bidAccount);
        }
    }
}

