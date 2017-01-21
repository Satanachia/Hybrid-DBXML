namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildListGetCommand : BasicCommand
    {
        private DateTime m_CurrentDBTime = DateTime.MinValue;
        private GuildIDList m_DeleteList = null;
        private GuildIDList m_GuildList = null;
        private string m_Server = string.Empty;
        private long m_TimeTick = 0L;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : DB 시간의 현재시간을 얻어옵니다");
            this.m_CurrentDBTime = QueryManager.Guild.GetDBCurrentTime();
            if (this.m_TimeTick == 0L)
            {
                WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 길드 전체 리스트를 얻어옵니다");
                this.m_GuildList = QueryManager.Guild.LoadGuildList(this.m_Server, DateTime.MinValue);
                this.m_DeleteList = new GuildIDList();
                this.m_DeleteList.guildID = null;
            }
            else
            {
                WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 특정 시간까지의 변화된 길드 리스트를 얻어옵니다");
                this.m_GuildList = QueryManager.Guild.LoadGuildList(this.m_Server, new DateTime(this.m_TimeTick));
                WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 특정 시간까지의 삭제된 길드 리스트를 얻어옵니다");
                this.m_DeleteList = QueryManager.Guild.LoadDeletedGuildList(this.m_Server, new DateTime(this.m_TimeTick));
            }
            if ((this.m_GuildList != null) && (this.m_DeleteList != null))
            {
                WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 길드 리스트를 얻어왔습니다");
                return true;
            }
            WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 길드 리스트를 얻는데 실패하였습니다");
            return false;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildListGetCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if ((this.m_GuildList != null) && (this.m_DeleteList != null))
            {
                message.WriteU8(1);
                message.WriteU64((ulong) this.m_CurrentDBTime.Ticks);
                GuildIDListSerializer.Deserialize(this.m_GuildList, message);
                GuildIDListSerializer.Deserialize(this.m_DeleteList, message);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Server = _Msg.ReadString();
            this.m_TimeTick = (long) _Msg.ReadU64();
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

