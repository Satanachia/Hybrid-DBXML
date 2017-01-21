namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBlockUpdateCommand : BasicCommand
    {
        private CastleBlock[] m_AddedBlock = null;
        private long m_CastleID = 0L;
        private CastleBlock[] m_DeletedBlock = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 성 집 출입 제한 리스트를 업데이트 합니다.");
            this.m_Result = QueryManager.Castle.UpdateBlock(this.m_CastleID, this.m_AddedBlock, this.m_DeletedBlock);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 성 집 출입 제한 리스트를 업데이트했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 성 집 출입 제한 리스트를 업데이트하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleBlockUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_CastleID = _Msg.ReadS64();
            this.m_AddedBlock = CastleBlockSerializer.Serialize(_Msg);
            this.m_DeletedBlock = CastleBlockSerializer.Serialize(_Msg);
        }
    }
}

