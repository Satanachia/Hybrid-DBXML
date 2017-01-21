namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildGetJoinedMemberCountCommand : BasicCommand
    {
        private int m_count;
        private DateTime m_endTime;
        private long m_Id = 0L;
        private bool m_Result = false;
        private DateTime m_startTime;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 길드에 해당 기간동안 가입한 인원수를 가져옵니다.");
            this.m_Result = QueryManager.Guild.GetJoinedMemberCount(this.m_Id, this.m_startTime, this.m_endTime, out this.m_count);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 길드에 해당 기간동안 가입한 인원수를 가져오는데 성공했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 길드에 해당 기간동안 가입한 인원수를 가져오는데 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                message.WriteU16((ushort) this.m_count);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_Id = _Msg.ReadS64();
            this.m_startTime = new DateTime((long) _Msg.ReadU64());
            this.m_endTime = new DateTime((long) _Msg.ReadU64());
        }
    }
}

