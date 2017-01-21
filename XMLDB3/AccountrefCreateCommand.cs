namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountrefCreateCommand : BasicCommand
    {
        private bool m_Result = false;
        private Accountref m_WriteAccountref = null;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : [" + this.m_WriteAccountref.account + "] 게임계정을 생성합니다");
            this.m_Result = QueryManager.Accountref.Create(this.m_WriteAccountref);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() [" + this.m_WriteAccountref.account + "] 게임계정을 생성하였습니다.");
                return true;
            }
            if (this.m_WriteAccountref != null)
            {
                WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : [" + this.m_WriteAccountref.account + "] 게임계정 생성에 실패하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : 입력된 게임계정 정보가 null 로 생성에 실패하였습니다");
            }
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountrefCreateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteString(this.m_WriteAccountref.account);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_WriteAccountref = AccountrefSerializer.Serialize(_Msg);
        }
    }
}

