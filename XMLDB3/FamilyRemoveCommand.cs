namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyRemoveCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private long m_FamilyID = 0L;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FamilyRemoveCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Family.RemoveFamily(this.m_FamilyID, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("FamilyRemoveCommand.DoProcess() : 가문 데이터를 성공적으로 지웠습니다.");
            }
            else
            {
                WorkSession.WriteStatus("FamilyRemoveCommand.DoProcess() : 가문 데이터를 지우는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FamilyRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8(this.m_errorCode);
            }
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_FamilyID = _message.ReadS64();
        }
    }
}

