namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidEndCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private long m_HouseID = 0L;
        private bool m_Result = false;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 집 경매를 종료합니다.");
            this.m_Result = QueryManager.House.EndBid(this.m_HouseID, this.m_Account, this.m_Server);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 집 경매를 종료 하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 집 경매를 종료하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBidEndCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Account = _Msg.ReadString();
            this.m_Server = _Msg.ReadString();
        }
    }
}

