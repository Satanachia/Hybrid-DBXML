namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RoyalAlchemistReadListCommand : BasicCommand
    {
        private RoyalAlchemistList m_royalAlchemistList = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("RoyalAlchemistReadListCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_royalAlchemistList = QueryManager.RoyalAlchemist.ReadList();
            if (this.m_royalAlchemistList != null)
            {
                WorkSession.WriteStatus("RoyalAlchemistReadListCommand.DoProcess() : 왕성 연금술사 데이터를 성공적으로 읽었습니다");
            }
            else
            {
                WorkSession.WriteStatus("RoyalAlchemistReadListCommand.DoProcess() : 왕성 연금술사 데이터를 읽는데 실패하였습니다.");
            }
            return (this.m_royalAlchemistList != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("RoyalAlchemistReadListCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_royalAlchemistList != null)
            {
                message.WriteU8(1);
                RoyalAlchemistListSerializer.Deserialize(this.m_royalAlchemistList, message);
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

