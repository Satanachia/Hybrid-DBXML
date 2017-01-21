namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WineUpdateCommand : BasicCommand
    {
        private bool m_Result = false;
        private Wine m_Wine = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("WineUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Wine.Update(this.m_Wine);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("WineUpdateCommand.DoProcess() : 와인 데이터를 업데이트하는데 성공했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("WineUpdateCommand.DoProcess() : 와인 데이터를 업데이트하는데 실패했습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("WineUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_Wine = WineSerializer.Serialize(_message);
        }
    }
}

