namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailReadCommand : BasicCommand
    {
        private long m_CharacterID = 0L;
        private MailBox m_ReceiveBox = new MailBox();
        private bool m_Result = false;
        private MailBox m_SendBox = new MailBox();

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("MailReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("MailReadCommand.DoProcess() : 편지함을 받아옵니다.");
            this.m_Result = QueryManager.MailBox.ReadMail(this.m_CharacterID, this.m_ReceiveBox, this.m_SendBox);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("MailReadCommand.DoProcess() :  편지함을 받아오는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("MailReadCommand.DoProcess() :  편지함을 받아오는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("MailReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                MailBoxSerializer.Deserialize(this.m_ReceiveBox, message);
                MailBoxSerializer.Deserialize(this.m_SendBox, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_CharacterID = _Msg.ReadS64();
        }
    }
}

