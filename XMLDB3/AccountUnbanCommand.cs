namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountUnbanCommand : BasicCommand
    {
        private string m_Account = string.Empty;
        private string m_Manager = string.Empty;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountUnbanCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountUnbanCommand.DoProcess() : [" + this.m_Account + "] 계정을 언밴합니다");
            if (QueryManager.Account.Unban(this.m_Account, this.m_Manager))
            {
                WorkSession.WriteStatus("AccountUnbanCommand.DoProcess() : [" + this.m_Account + "] 계정을 성공적으로 언밴하였습니다");
                this.m_Result = true;
                return true;
            }
            WorkSession.WriteStatus("AccountUnbanCommand.DoProcess() : [" + this.m_Account + "] 계정 언밴에 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountUnbanCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Manager = _Msg.ReadString();
        }
    }
}

