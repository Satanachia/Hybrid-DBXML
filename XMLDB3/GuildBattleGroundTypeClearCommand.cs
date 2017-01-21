namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Collections;

    public class GuildBattleGroundTypeClearCommand : BasicCommand
    {
        private ArrayList m_GuildList = new ArrayList();
        private bool m_Result = false;
        private string m_Server = string.Empty;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildBattleGroundTypeClearCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.ClearBattleGroundType(this.m_Server, this.m_GuildList);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : [" + this.m_Server + "] 서버 길드의 길드전 참가 여부를 초기화 했습니다.");
            }
            else
            {
                WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : [" + this.m_Server + "] 서버 길드의 길드전 참가 여부 초기화를 실패했습니다.");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildBattleGroundTypeClearCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_Server = _message.ReadString();
            uint num = _message.ReadU32();
            for (uint i = 0; i < num; i++)
            {
                long num3 = (long) _message.ReadU64();
                this.m_GuildList.Add(num3);
            }
        }
    }
}

