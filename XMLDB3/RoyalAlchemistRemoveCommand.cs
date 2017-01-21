namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RoyalAlchemistRemoveCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private long[] m_removeIDs;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.RoyalAlchemist.Remove(this.m_removeIDs, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.DoProcess() : 왕궁 연금술사 데이터를 성공적으로 지웠습니다.");
            }
            else
            {
                WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.DoProcess() : 왕궁 연금술 데이터를 지우는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
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
            ushort num = _message.ReadU16();
            this.m_removeIDs = new long[num];
            for (ushort i = 0; i < num; i = (ushort) (i + 1))
            {
                this.m_removeIDs[i] = _message.ReadS64();
            }
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

