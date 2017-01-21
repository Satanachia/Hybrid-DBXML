namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidRepayEndCommand : BasicCommand
    {
        private long m_HouseID = 0L;
        private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 환불을 종료합니다.");
            this.m_Result = QueryManager.House.EndBidRepay(this.m_HouseID);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 환불을 종료 하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 환불을 종료하는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBidRepayEndCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_HouseID = _Msg.ReadS64();
        }
    }
}

