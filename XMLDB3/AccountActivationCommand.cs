﻿namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountActivationCommand : BasicCommand
    {
        private bool m_Result = false;
        private AccountActivation m_WriteAccount = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : [" + this.m_WriteAccount.id + "] 계정을 생성합니다");
            if (QueryManager.AccountActivation.Create(this.m_WriteAccount))
            {
                WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : [" + this.m_WriteAccount.id + "] 계정을 성공적으로 생성하였습니다");
                this.m_Result = true;
                return true;
            }
            if (this.m_WriteAccount != null)
            {
                WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : [" + this.m_WriteAccount.id + "] 계정 생성에 실패하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : 계정 정보가 null 로 생성에 실패하였습니다");
            }
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountActivationCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result && (this.m_WriteAccount != null))
            {
                message.WriteU8(1);
                message.WriteString(this.m_WriteAccount.id);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_WriteAccount = AccountSerializer.SerializeForActivation(_Msg);
        }
    }
}

