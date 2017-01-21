namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildStatusUpdateCommand : BasicCommand
    {
        private long m_Id = 0L;
        private int m_PointRequired = 0;
        private bool m_Result = false;
        private bool m_Set = false;
        private byte m_StatusFlag = 0;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildStatusUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.UpdateGuildStatus(this.m_Id, this.m_StatusFlag, this.m_Set, this.m_PointRequired);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildStatusUpdateCommand.DoProcess() : [", this.m_Id, "] 길드의 StatusFlag를 [", this.m_StatusFlag, "] 로 [", this.m_Set, "] 변경했습니다." }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildStatusUpdateCommand.DoProcess() : [", this.m_Id, "] 길드의 StatusFlag를 [", this.m_StatusFlag, "] 로 [", this.m_Set, "] 변경에 실패했습니다." }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildStatusUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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

        protected override void ReceiveData(Message _message)
        {
            this.m_Id = (long) _message.ReadU64();
            this.m_StatusFlag = _message.ReadU8();
            this.m_Set = _message.ReadU8() != 0;
            this.m_PointRequired = _message.ReadS32();
        }
    }
}

