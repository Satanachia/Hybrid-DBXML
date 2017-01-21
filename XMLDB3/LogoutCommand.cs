namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class LogoutCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private bool m_Result = false;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("LogoutCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("LogoutCommand.DoProcess() : [" + this.m_Account + "] 계정 로그아웃 히스토리를 기록합니다.");
            if (QueryManager.Account.LogoutSignal(this.m_Account))
            {
                WorkSession.WriteStatus("LogoutCommand.DoProcess() : [" + this.m_Account + "] 계정 로그아웃 히스토리를 기록하였습니다");
                this.m_Result = true;
                return true;
            }
            WorkSession.WriteStatus("LogoutCommand.DoProcess() : [" + this.m_Account + "] 계정 로그아웃 히스토리를 기록하지 못하였습니다");
            return false;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("LogoutCommand.MakeMessage() : 함수에 진입하였습니다");
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

        public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
        {
            _helper.StringIDRegistant(this.m_Account);
        }

        public override bool ReplyEnable
        {
            get
            {
                return false;
            }
        }
    }
}

