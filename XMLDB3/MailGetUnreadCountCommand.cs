namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailGetUnreadCountCommand : BasicCommand
    {
        private long m_ReceiverID = 0L;
        private bool m_Result = false;
        private int m_UnreadCount = 0;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 안읽은 메일 갯수를 가져옵니다.");
            this.m_Result = QueryManager.MailBox.GetUnreadCount(this.m_ReceiverID, out this.m_UnreadCount);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 안읽은 메일 갯수를 가져오는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 안읽은 메일 갯수를 가져오는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("MailGetUnreadCountCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU32((uint) this.m_UnreadCount);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_ReceiverID = _Msg.ReadS64();
        }
    }
}

