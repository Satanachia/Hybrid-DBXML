namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ChronicleInfoInitCommand : BasicCommand
    {
        private ChronicleInfoList m_InfoList = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() : 탐사연표 이미지를 초기화합니다.");
            this.m_Result = QueryManager.Chronicle.UpdateChronicleInfoList(this.m_InfoList);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() :탐사연표 이미지를 초기화하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() : 탐사연표 이미지 초기화에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ChronicleInfoInitCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_InfoList = ChronicleInfoListSerializer.Serialize(_Msg);
        }
    }
}

