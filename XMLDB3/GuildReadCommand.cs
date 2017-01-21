namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildReadCommand : BasicCommand
    {
        private Guild m_Guild = null;
        private long m_Id = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildReadCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Guild = QueryManager.Guild.Read(this.m_Id);
            if ((this.m_Guild != null) && (this.m_Guild.guildtitle == null))
            {
                this.m_Guild.guildtitle = "";
            }
            if (this.m_Guild == null)
            {
                return false;
            }
            return true;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildReadCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Guild != null)
            {
                message.WriteU8(1);
                GuildSerializer.Deserialize(this.m_Guild, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = (long) _Msg.ReadU64();
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

