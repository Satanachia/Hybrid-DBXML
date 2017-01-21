namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountrefReadCommand : BasicCommand
    {
        private Accountref m_ReadAccountref = null;
        private bool m_Result = false;
        private string m_strAccountref;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : [" + this.m_strAccountref + "] 게임계정을 읽습니다");
            this.m_ReadAccountref = QueryManager.Accountref.Read(this.m_strAccountref);
            if (this.m_ReadAccountref != null)
            {
                WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : [" + this.m_strAccountref + "] 게임계정을 읽었습니다");
                this.m_Result = true;
                return true;
            }
            WorkSession.WriteStatus("AccountrefReadCommand.DoProcess() : [" + this.m_strAccountref + "] 게임계정을 읽지 못하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("AccountrefReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result && (this.m_ReadAccountref != null))
            {
                message.WriteU8(1);
                AccountrefSerializer.Deserialize(this.m_ReadAccountref, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_strAccountref = _Msg.ReadString();
        }
    }
}

