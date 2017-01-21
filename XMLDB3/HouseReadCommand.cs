namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseReadCommand : BasicCommand
    {
        private House m_House = null;
        private long m_HouseID = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 집을 읽습니다.");
            this.m_Result = QueryManager.House.Read(this.m_HouseID, out this.m_House);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 집을 얻어왔습니다");
            }
            else
            {
                WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 집을 얻는데 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseListReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                HouseSerializer.Deserialize(this.m_House, message);
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

