namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBlockUpdateCommand : BasicCommand
    {
        private HouseBlock[] m_AddedBlock = null;
        private HouseBlock[] m_DeletedBlock = null;
        private long m_HouseID = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 집 집 출입 제한 리스트를 업데이트 합니다.");
            this.m_Result = QueryManager.House.UpdateBlock(this.m_HouseID, this.m_AddedBlock, this.m_DeletedBlock);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 집 집 출입 제한 리스트를 업데이트했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 집 집 출입 제한 리스트를 업데이트하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HouseBlockUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_AddedBlock = HouseBlockSerializer.Serialize(_Msg);
            this.m_DeletedBlock = HouseBlockSerializer.Serialize(_Msg);
        }
    }
}

