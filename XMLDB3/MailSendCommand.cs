namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailSendCommand : SerializedCommand
    {
        private byte m_ErrorCode = 0;
        private MailItem m_MailItem = null;
        private long m_Result = 0L;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("MailSendCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("MailSendCommand.DoProcess() : [" + this.m_MailItem.senderCharID + "] 의 데이터를 캐쉬에서 읽습니다");
            this.m_Result = QueryManager.MailBox.SendMail(this.m_MailItem, ref this.m_ErrorCode);
            if (this.m_Result != 0L)
            {
                WorkSession.WriteStatus("MailSendCommand.DoProcess() : 메일을 보냇습니다..");
            }
            else
            {
                WorkSession.WriteStatus("MailSendCommand.DoProcess() : 메일을 보내는데 실패하였습니다..");
            }
            return (this.m_Result != 0L);
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_MailItem = MailItemSeirializer.Serialize(_Msg);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("MailSendCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result != 0L)
            {
                message.WriteU8(1);
                message.WriteS64(this.m_Result);
                return message;
            }
            message.WriteU8(0);
            message.WriteU8(this.m_ErrorCode);
            return message;
        }

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            if (this.m_MailItem.item != null)
            {
                if (this.m_MailItem.senderCharID != 0L)
                {
                    _helper.ObjectIDRegistant(this.m_MailItem.senderCharID);
                }
                _helper.ObjectIDRegistant(this.m_MailItem.item.id);
            }
        }
    }
}

