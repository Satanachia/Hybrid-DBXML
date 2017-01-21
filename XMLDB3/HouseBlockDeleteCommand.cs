namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBlockDeleteCommand : BasicCommand
    {
        private long m_HouseID = 0L;
        private bool m_Result;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 집 집 출입 제한 리스트를 삭제합니다.");
            this.m_Result = QueryManager.House.DeleteBlock(this.m_HouseID);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 집 집 출입 제한 리스트를 삭제하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 집 집 출입 제한 리스트를 삭제하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBlockDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _Msg)
        {
            this.m_HouseID = _Msg.ReadS64();
        }
    }
}

