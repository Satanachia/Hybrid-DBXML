namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PropIdPoolCommand : BasicCommand
    {
        private long m_IdOffset = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("PropIdPoolCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_IdOffset = QueryManager.PropIdPool.GetIdPool();
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("PropIdPoolCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            message.WriteU64((ulong) this.m_IdOffset);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
        }
    }
}

