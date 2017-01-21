namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyUpdateCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private FamilyListFamily m_Family = null;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FamilyUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Family.UpdateFamily(this.m_Family, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("FamilyUpdateCommand.DoProcess() : 가문 데이터를 성공적으로 업데이트했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("FamilyUpdateCommand.DoProcess() : 가문 데이터를 업데이트하는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FamilyUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Family = FamilySerializer.Serialize(_message);
        }
    }
}

