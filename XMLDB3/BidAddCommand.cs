namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BidAddCommand : BasicCommand
    {
        private Bid m_Bid = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("BidAddCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Bid.Add(this.m_Bid);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("BidAddCommand.DoProcess() : 경매 데이터를 성공적으로 생성하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("BidAddCommand.DoProcess() : 경매 데이터를 생성하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BidAddCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _message)
        {
            this.m_Bid = BidSerializer.Serialize(_message);
        }
    }
}

