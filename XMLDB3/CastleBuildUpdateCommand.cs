namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBuildUpdateCommand : BasicCommand
    {
        private CastleBuild m_Build = null;
        private long m_CastleID = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 성 건설 정보를 업데이트 합니다.");
            this.m_Result = QueryManager.Castle.UpdateBuild(this.m_CastleID, this.m_Build);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 성 건설 정보를 업데이트하였습니다..");
            }
            else
            {
                WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 성 건설 정보를 업데이트하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CastleBuildUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Build = CastleBuildSerializer.Serialize(_Msg);
        }
    }
}

