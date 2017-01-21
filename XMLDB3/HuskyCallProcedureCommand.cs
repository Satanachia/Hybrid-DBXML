namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HuskyCallProcedureCommand : BasicCommand
    {
        private string m_accountName;
        private bool m_bResult = false;
        private long m_charId;
        private string m_charName;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("HuskyCallProcedureCommand.DoProcess() : 함수에 진입하였습니다");
            HuskyAdapter huskyEvent = QueryManager.HuskyEvent;
            if (huskyEvent != null)
            {
                this.m_bResult = huskyEvent.Callprocedure(this.m_accountName, this.m_charId, this.m_charName);
            }
            else
            {
                this.m_bResult = false;
            }
            if (this.m_bResult)
            {
                WorkSession.WriteStatus("HuskyCallProcedureCommand.DoProcess() : 허스키 프로시져를 성공적으로 실행했습니다");
            }
            else
            {
                WorkSession.WriteStatus("HuskyCallProcedureCommand.DoProcess() : 허스키 프로시져를 실행하는데 실패하였습니다.");
            }
            return this.m_bResult;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("HuskyCallProcedureCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_bResult)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_accountName = _message.ReadString();
            this.m_charId = _message.ReadS64();
            this.m_charName = _message.ReadString();
        }

        public override bool IsPrimeCommand
        {
            get
            {
                return true;
            }
        }
    }
}

