namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountBanCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private short m_banType = -1;
        private short m_duration = 0;
        private string m_ManagerName = string.Empty;
        private string m_Purpose = string.Empty;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountBanCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountBanCommand.DoProcess() : [" + this.m_Account + "] 계정을 밴합니다");
            if (QueryManager.Account.Ban(this.m_Account, this.m_banType, this.m_ManagerName, this.m_duration, this.m_Purpose))
            {
                WorkSession.WriteStatus("AccountBanCommand.DoProcess() : [" + this.m_Account + "] 계정을 성공적으로 밴하였습니다");
                this.m_Result = true;
                return true;
            }
            WorkSession.WriteStatus("AccountBanCommand.DoProcess() : [" + this.m_Account + "] 계정 밴에 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountBanCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Account = _Msg.ReadString();
            this.m_banType = _Msg.ReadS8();
            this.m_ManagerName = _Msg.ReadString();
            this.m_duration = _Msg.ReadS8();
            this.m_Purpose = _Msg.ReadString();
        }
    }
}

