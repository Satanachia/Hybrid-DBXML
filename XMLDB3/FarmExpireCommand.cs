namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FarmExpireCommand : BasicCommand
    {
        private byte m_ErrorCode = 0;
        private long m_FarmId = 0L;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FarmExpireCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("FarmExpireCommand.DoProcess() : 농장을 만료시킵니다.");
            this.m_Result = QueryManager.Farm.Expire(this.m_FarmId, ref this.m_ErrorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("FarmExpireCommand.DoProcess() :  농장을 만료시켰습니다");
                return true;
            }
            WorkSession.WriteStatus("FarmExpireCommand.DoProcess() : 농장을 만료시키는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FarmExpireCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result != REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8((byte) this.m_Result);
                return message;
            }
            message.WriteU8((byte) this.m_Result);
            message.WriteU8(this.m_ErrorCode);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_FarmId = _Msg.ReadS64();
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

