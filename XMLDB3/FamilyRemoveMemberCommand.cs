namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyRemoveMemberCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private long m_familyID;
        private long m_familyMemberID = 0L;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FamilyRemoveMemberCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Family.RemoveMember(this.m_familyID, this.m_familyMemberID, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("FamilyRemoveMemberCommand.DoProcess() : 가문 멤버 데이터를 성공적으로 지웠습니다.");
            }
            else
            {
                WorkSession.WriteStatus("FamilyRemoveMemberCommand.DoProcess() : 가문 멤버 데이터를 지우는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FamilyRemoveMemberCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_familyID = _message.ReadS64();
            this.m_familyMemberID = _message.ReadS64();
        }
    }
}

