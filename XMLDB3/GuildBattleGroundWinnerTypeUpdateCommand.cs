namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildBattleGroundWinnerTypeUpdateCommand : BasicCommand
    {
        private byte m_BattleGroundWinnerType = 0;
        private long m_Id = 0L;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildBattleGroundWinnerTypeUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.UpdateBattleGroundWinnerType(this.m_Id, this.m_BattleGroundWinnerType);
            if (this.m_Result)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildBattleGroundWinnerTypeUpdateCommand.DoProcess() : [", this.m_Id, "] 길드의 길드전 순위 [", this.m_BattleGroundWinnerType, "] 로 변경했습니다." }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildBattleGroundWinnerTypeUpdateCommand.DoProcess() : [", this.m_Id, "] 길드의 길드전 순위 [", this.m_BattleGroundWinnerType, "] 로 변경 실패했습니다." }));
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildBattleGroundWinnerTypeUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_BattleGroundWinnerType = _message.ReadU8();
        }
    }
}

