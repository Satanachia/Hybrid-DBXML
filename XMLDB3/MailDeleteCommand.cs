namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailDeleteCommand : SerializedCommand
    {
        private byte m_ErrorCode = 0;
        private long m_itemID = 0L;
        private byte m_itemType = 0;
        private long m_PostID = 0L;
        private long m_ReceiverID = 0L;
        private bool m_Result = false;
        private long m_SenderID = 0L;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 받은 편지함을 삭제합니다.");
            this.m_Result = QueryManager.MailBox.DeleteMail(this.m_PostID, this.m_itemID, this.m_itemType, this.m_ReceiverID, this.m_SenderID, ref this.m_ErrorCode);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 받은 편지함을 삭제하는데 성공하였습니다.");
            }
            else
            {
                WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 받은 편지함을 삭제하는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_PostID = _Msg.ReadS64();
            this.m_itemID = _Msg.ReadS64();
            this.m_itemType = _Msg.ReadU8();
            this.m_ReceiverID = _Msg.ReadS64();
            this.m_SenderID = _Msg.ReadS64();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("MailDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            message.WriteU8(this.m_ErrorCode);
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            if (this.m_itemID != 0L)
            {
                _helper.ObjectIDRegistant(this.m_itemID);
            }
            if (this.m_ReceiverID != 0L)
            {
                _helper.ObjectIDRegistant(this.m_ReceiverID);
            }
            if (this.m_SenderID != 0L)
            {
                _helper.ObjectIDRegistant(this.m_SenderID);
            }
        }
    }
}

