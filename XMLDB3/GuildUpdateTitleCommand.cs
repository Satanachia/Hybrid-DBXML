namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildUpdateTitleCommand : BasicCommand
    {
        private bool m_bUsable = false;
        private long m_Id = 0L;
        private bool m_Result = false;
        private string m_strTitle;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildUpdateTitleCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus(string.Format("GuildUpdateTitleCommand.DoProcess() : [{0}] 길드의 타이틀을 {1} 으로 변경합니다.", this.m_Id, this.m_strTitle));
            this.m_Result = QueryManager.Guild.UpdateTitle(this.m_Id, this.m_strTitle, this.m_bUsable);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Format("GuildUpdateTitleCommand.DoProcess() : [{0}] 길드의 타이틀을 {1} 으로 변경했습니다.", this.m_Id, this.m_strTitle));
            }
            else
            {
                WorkSession.WriteStatus("GuildUpdateTitleCommand.DoProcess() : [" + this.m_Id + "] 길드 타이틀 변경에 실패하였습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildUpdateTitleCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_strTitle = _Msg.ReadString();
            this.m_bUsable = _Msg.ReadU8() != 0;
        }
    }
}

