namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WineDeleteCommand : BasicCommand
    {
        private long m_CharID = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("WineDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Wine.Delete(this.m_CharID);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("WineDeleteCommand.DoProcess() : 와인 데이터를 성공적으로 삭제했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("WineDeleteCommand.DoProcess() : 와인 데이터를 삭제하는데 실패했습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("WineDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_CharID = _message.ReadS64();
        }
    }
}

