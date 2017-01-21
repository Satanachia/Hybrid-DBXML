namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidStartCommand : BasicCommand
    {
        private HouseBid m_HouseBid = null;
        private long m_HouseID = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 집 경매를 시작합니다.");
            this.m_Result = QueryManager.House.CreateBid(this.m_HouseID, this.m_HouseBid);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 집 경매를 시작하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 집 경매를 시작하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseListReadCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_HouseID = _Msg.ReadS64();
            this.m_HouseBid = HouseBidSerializer.Serialize(_Msg);
        }
    }
}

