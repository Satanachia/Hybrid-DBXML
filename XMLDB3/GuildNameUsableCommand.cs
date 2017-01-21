namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildNameUsableCommand : BasicCommand
    {
        private string m_Name;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildNameUsableCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.IsUsableName(this.m_Name);
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildNameUsableCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Name = _Msg.ReadString();
        }
    }
}

