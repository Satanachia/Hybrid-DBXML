namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RoyalAlchemistReadCommand : BasicCommand
    {
        private long m_charID = 0L;
        private RoyalAlchemist m_royalAlchemist = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("RoyalAlchemistReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_royalAlchemist = QueryManager.RoyalAlchemist.Read(this.m_charID);
            if (this.m_royalAlchemist != null)
            {
                WorkSession.WriteStatus("RoyalAlchemistReadCommand.DoProcess() : 왕성 연금술사 데이터를 성공적으로 읽었습니다");
            }
            else
            {
                WorkSession.WriteStatus("RoyalAlchemistReadCommand.DoProcess() : 왕성 연금술사 데이터를 읽는데 실패하였습니다.");
            }
            return (this.m_royalAlchemist != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("RoyalAlchemistReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_royalAlchemist != null)
            {
                message.WriteU8(1);
                RoyalAlchemistSerializer.Deserialize(this.m_royalAlchemist, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_charID = _message.ReadS64();
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

