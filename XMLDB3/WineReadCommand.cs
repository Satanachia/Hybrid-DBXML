namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WineReadCommand : BasicCommand
    {
        private long m_CharID = 0L;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;
        private Wine m_Wine = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("WineReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Wine.Read(this.m_CharID, out this.m_Wine);
            if (this.m_Result != REPLY_RESULT.FAIL)
            {
                WorkSession.WriteStatus("WineReadCommand.DoProcess() : 와인 데이터를 읽는데 성공했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("WineReadCommand.DoProcess() : 와인 데이터를 읽는데 실패했습니다.");
            }
            return (this.m_Result != REPLY_RESULT.FAIL);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("WineReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WineSerializer.Deserialize(this.m_Wine, message);
            }
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_CharID = _message.ReadS64();
        }
    }
}

