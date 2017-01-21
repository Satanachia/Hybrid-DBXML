namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class LoginCommand : SerializedCommand
    {
        private string m_Account = string.Empty;
        private string m_Address = string.Empty;
        private int m_ISPCode = 0;
        private bool m_Result = false;
        private long m_SessionKey = 0L;

        protected override bool _DoProces()
        {
            WorkSession.WriteStatus("LoginCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("LoginCommand.DoProcess() : [" + this.m_Account + "] 계정 로그인 히스토리를 기록합니다.");
            if (QueryManager.Account.LoginSignal(this.m_Account, this.m_SessionKey, this.m_Address, this.m_ISPCode))
            {
                WorkSession.WriteStatus("LoginCommand.DoProcess() : [" + this.m_Account + "] 계정 로그인 히스토리를 기록하였습니다");
                this.m_Result = true;
                return true;
            }
            WorkSession.WriteStatus("LoginCommand.DoProcess() : [" + this.m_Account + "] 계정 로그인 히스토리를 기록하지 못하였습니다");
            return false;
        }

        protected override void _ReceiveData(Message _Msg)
        {
            this.m_Account = _Msg.ReadString();
            this.m_SessionKey = _Msg.ReadS64();
            this.m_Address = _Msg.ReadString();
            this.m_ISPCode = _Msg.ReadS32();
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("LoginCommand.MakeMessage() : 함수에 진입하였습니다");
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
    }
}

