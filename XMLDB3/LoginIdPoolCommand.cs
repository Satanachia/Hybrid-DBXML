namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class LoginIdPoolCommand : BasicCommand
    {
        private long m_IdOffset = 0L;
        private int m_Size = 0;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("LoginIdPoolCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_IdOffset = QueryManager.LoginIdPool.GetIdPool(this.m_Size);
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("LoginIdPoolCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU64((ulong) this.m_IdOffset);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Size = _Msg.ReadS32();
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

