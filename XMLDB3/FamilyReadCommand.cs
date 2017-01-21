namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyReadCommand : BasicCommand
    {
        private FamilyListFamily m_family = null;
        private long m_familyID = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FamilyReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_family = QueryManager.Family.Read(this.m_familyID);
            if (this.m_family != null)
            {
                WorkSession.WriteStatus("FamilyReadCommand.DoProcess() : 가문 데이터를 성공적으로 읽었습니다");
            }
            else
            {
                WorkSession.WriteStatus("FamilyReadCommand.DoProcess() : 가문 데이터를 읽는데 실패하였습니다.");
            }
            return (this.m_family != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FamilyReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_family != null)
            {
                message.WriteU8(1);
                FamilySerializer.Deserialize(this.m_family, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_familyID = _message.ReadS64();
        }
    }
}

