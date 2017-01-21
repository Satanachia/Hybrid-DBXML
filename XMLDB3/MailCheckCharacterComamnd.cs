namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailCheckCharacterComamnd : BasicCommand
    {
        private byte m_ErrorCode = 0;
        private string m_Name = string.Empty;
        private string m_OutName = string.Empty;
        private long m_Result = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 캐릭터 이름을 확인합니다.");
            this.m_Result = QueryManager.MailBox.CheckCharacterName(this.m_Name, ref this.m_OutName, ref this.m_ErrorCode);
            if (this.m_Result != 0L)
            {
                WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 캐릭터 이름이 정상입니다.");
            }
            else
            {
                WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 존재하지 않는 캐릭터 입니다.");
            }
            if (this.m_Result == 0L)
            {
                return false;
            }
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("MailCheckCharacterComamnd.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result != 0L)
            {
                message.WriteU8(1);
                message.WriteS64(this.m_Result);
                message.WriteString(this.m_OutName);
                return message;
            }
            message.WriteU8(0);
            message.WriteU8(this.m_ErrorCode);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Name = _Msg.ReadString();
        }
    }
}

