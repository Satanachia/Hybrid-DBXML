namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildPointAddCommand : BasicCommand
    {
        private long m_Id = 0L;
        private int m_Point = 0;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildPointAddCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.AddPoint(this.m_Id, this.m_Point);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildPointAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Id = (long) _Msg.ReadU64();
            this.m_Point = _Msg.ReadS32();
        }
    }
}

