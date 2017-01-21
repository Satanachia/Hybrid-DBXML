namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountReadCommand : BasicCommand
    {
        private Account m_ReadAccount = null;
        private bool m_Result = false;
        private string m_strAccount;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountReadCommand.DoProcess() : [" + this.m_strAccount + "] 계정 정보 읽기를 쿼리합니다");
            this.m_ReadAccount = QueryManager.Account.Read(this.m_strAccount);
            if (this.m_ReadAccount != null)
            {
                WorkSession.WriteStatus("AccountReadCommand.DoProcess() : [" + this.m_strAccount + "] 계정 정보를 읽었습니다");
                this.m_Result = true;
                return true;
            }
            WorkSession.WriteStatus("AccountReadCommand.DoProcess() : [" + this.m_strAccount + "] 계정에 대한 쿼리를 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result && (this.m_ReadAccount != null))
            {
                message.WriteU8(1);
                AccountSerializer.Deserialize(this.m_ReadAccount, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_strAccount = _Msg.ReadString();
        }
    }
}

