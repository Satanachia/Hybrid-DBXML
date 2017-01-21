namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class EventUpdateCommand : BasicCommand
    {
        private byte m_errorCode = 0;
        private Event m_Event = null;
        private REPLY_RESULT m_Result = REPLY_RESULT.FAIL;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("EventUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Event.Update(this.m_Event, ref this.m_errorCode);
            if (this.m_Result == REPLY_RESULT.SUCCESS)
            {
                WorkSession.WriteStatus("EventUpdateCommand.DoProcess() : 이벤트 데이터를 성공적으로 업데이트했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("EventUpdateCommand.DoProcess() : 이벤트 데이터를 업데이트하는데 실패하였습니다.");
            }
            return (this.m_Result == REPLY_RESULT.SUCCESS);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("EventUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU8((byte) this.m_Result);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_Event = EventSerializer.Serialize(_message);
        }
    }
}

