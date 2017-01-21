namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FarmUpdateCommand : BasicCommand
    {
        private byte m_ErrorCode = 0;
        private Farm m_Farm = null;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() : 농장을 업데이트합니다.");
            this.m_Result = QueryManager.Farm.Update(this.m_Farm, ref this.m_ErrorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() :  농장을 업데이트했습니다");
                return true;
            }
            WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() : 농장을 업데이트하는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("FarmUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.FAIL_EX)
            {
                message.WriteU8(this.m_ErrorCode);
            }
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Farm = FarmSerializer.Serialize(_Msg);
        }
    }
}

