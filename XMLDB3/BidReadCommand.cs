namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BidReadCommand : BasicCommand
    {
        private BidList m_BidList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("BidReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_BidList = QueryManager.Bid.Read();
            if (this.m_BidList != null)
            {
                WorkSession.WriteStatus("BidReadCommand.DoProcess() : 경매 데이터를 성공적으로 읽었습니다");
            }
            else
            {
                WorkSession.WriteStatus("BidReadCommand.DoProcess() : 경매 데이터를 읽는데 실패하였습니다.");
            }
            return (this.m_BidList != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("BidReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_BidList != null)
            {
                message.WriteU8(1);
                BidListSerializer.Deserialize(this.m_BidList, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
        }

        public override bool IsPrimeCommand
        {
            get
            {
                return true;
            }
        }
    }
}

