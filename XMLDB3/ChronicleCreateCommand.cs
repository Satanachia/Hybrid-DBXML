namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ChronicleCreateCommand : BasicCommand
    {
        private string m_CharacterName = string.Empty;
        private Chronicle m_Chronicle = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() : 탐사연표를 생성합니다");
            this.m_Result = QueryManager.Chronicle.Create(this.m_CharacterName, this.m_Chronicle);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() :탐사연표를 생성하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() : 탐사연표 생성에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("ChronicleCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Chronicle = ChronicleSerializer.Serialize(_Msg);
            this.m_CharacterName = _Msg.ReadString();
        }
    }
}

