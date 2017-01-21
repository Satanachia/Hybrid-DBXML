namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildDeleteCommand : BasicCommand
    {
        private long m_Id = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : [" + this.m_Id + "] 길드를 제거합니다");
            this.m_Result = QueryManager.Guild.Delete(this.m_Id);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : [" + this.m_Id + "] 길드를 제거하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : [" + this.m_Id + "] 길드 제거에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Id = _Msg.ReadS64();
        }
    }
}

