namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildMemberAddCommand : BasicCommand
    {
        private long m_Id = 0L;
        private GuildMember m_Member = null;
        private bool m_Result = false;
        private string m_strJoinMsg = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 함수에 진입하였습니다");
            if (this.m_Member != null)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildMemberAddCommand.DoProcess() : 길드 [", this.m_Id, "] 에 [", this.m_Member.memberid, "/", this.m_Member.name, "] 를 멤버로 추가합니다" }));
                this.m_Result = QueryManager.Guild.AddMember(this.m_Id, this.m_Member, this.m_strJoinMsg);
                if (this.m_Result)
                {
                    WorkSession.WriteStatus(string.Concat(new object[] { "GuildMemberAddCommand.DoProcess() : 길드 [", this.m_Id, "] 에 [", this.m_Member.memberid, "/", this.m_Member.name, "] 를 멤버로 추가하였습니다" }));
                }
                else
                {
                    WorkSession.WriteStatus(string.Concat(new object[] { "GuildMemberAddCommand.DoProcess() : 길드 [", this.m_Id, "] 에 [", this.m_Member.memberid, "/", this.m_Member.name, "] 를 멤버로 추가하는데 실패하였습니다" }));
                }
            }
            else
            {
                WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 멤버 정보가 null 이기 때문에 추가 할 수 없습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildMemberAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Member = GuildSerializer.ReadGuildMemberFromMsg(_Msg);
            this.m_strJoinMsg = _Msg.ReadString();
        }
    }
}

