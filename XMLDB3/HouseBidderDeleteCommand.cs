namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidderDeleteCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private string m_CharName = string.Empty;
        private long m_HouseID = 0L;
        private int m_MaxRemainMoney = 0;
        private int m_RemainMoney = 0;
        private int m_RepayMoney = 0;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 입찰자를 삭제합니다.");
            this.m_Result = QueryManager.House.DeleteBidder(this.m_HouseID, this.m_Account, this.m_CharName, this.m_RepayMoney, QueryManager.Bank, this.m_MaxRemainMoney, out this.m_RemainMoney);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 입찰자를 삭제하였습니다.");
                BankCache cache = (BankCache) ObjectCache.Bank.Find(this.m_Account);
                if ((cache != null) && cache.IsValid())
                {
                    cache.bank.deposit = this.m_RemainMoney;
                }
                return true;
            }
            WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 입찰자를 삭제하는데 실패하였습니다.");
            return false;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_HouseID = _Msg.ReadS64();
            this.m_Account = _Msg.ReadString();
            this.m_CharName = _Msg.ReadString();
            this.m_RepayMoney = _Msg.ReadS32();
            this.m_MaxRemainMoney = _Msg.ReadS32();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBidEndCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                message.WriteS32(this.m_RemainMoney);
            }
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.StringIDRegistant(this.m_Account);
        }
    }
}

