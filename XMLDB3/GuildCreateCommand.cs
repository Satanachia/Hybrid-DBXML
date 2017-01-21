namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildCreateCommand : BasicCommand
    {
        private Guild m_Guild = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildCreateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus(string.Concat(new object[] { "GuildCreateCommand.DoProcess() : [", this.m_Guild.id, "/", this.m_Guild.name, "@", this.m_Guild.server, "] 길드를 생성합니다" }));
            this.m_Result = QueryManager.Guild.Create(this.m_Guild);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildCreateCommand.DoProcess() : [", this.m_Guild.id, "/", this.m_Guild.name, "@", this.m_Guild.server, "] 길드를 생성하였습니다" }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildCreateCommand.DoProcess() : [", this.m_Guild.id, "/", this.m_Guild.name, "@", this.m_Guild.server, "] 길드 생성에 실패하였습니다" }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Guild = GuildSerializer.Serialize(_Msg);
        }
    }
}

