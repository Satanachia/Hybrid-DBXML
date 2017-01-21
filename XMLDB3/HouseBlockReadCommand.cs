namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBlockReadCommand : BasicCommand
    {
        private HouseBlockList m_HouseBlockList = null;
        private long m_HouseID = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 집 집 출입 제한 리스트를 읽습니다.");
            this.m_HouseBlockList = QueryManager.House.ReadBlock(this.m_HouseID);
            if (this.m_HouseBlockList != null)
            {
                WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 집 집 출입 제한 리스트를 읽었습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 집 집 출입 제한 리스트를 읽는데 실패하였습니다.");
            }
            return (this.m_HouseBlockList != null);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBlockReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_HouseBlockList != null)
            {
                message.WriteU8(1);
                HouseBlockSerializer.Deserialize(this.m_HouseBlockList.block, message);
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

