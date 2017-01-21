namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBuildItemUpdateCommand : BasicCommand
    {
        private long m_CastleID = 0L;
        private CastleBuildResource m_Resource = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 성 건설 아이템 정보를 업데이트 합니다.");
            this.m_Result = QueryManager.Castle.UpdateBuildResource(this.m_CastleID, this.m_Resource);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 성 건설 아이템 정보를 업데이트하였습니다..");
            }
            else
            {
                WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 성 건설 아이템 정보를 업데이트하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleBuildItemUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Resource = CastleBuildResourceSerializer.Serialize(_Msg);
        }
    }
}

