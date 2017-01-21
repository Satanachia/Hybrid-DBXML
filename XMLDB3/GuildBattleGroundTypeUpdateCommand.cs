namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildBattleGroundTypeUpdateCommand : BasicCommand
    {
        private byte m_BattleGroundType = 0;
        private int m_GuildMoney = 0;
        private int m_GuildPoint = 0;
        private long m_Id = 0L;
        private int m_Result = -1;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            this.m_Result = QueryManager.Guild.UpdateBattleGroundType(this.m_Id, this.m_GuildPoint, this.m_GuildMoney, this.m_BattleGroundType);
            if (this.m_Result == 0)
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildBattleGroundTypeUpdateCommand.DoProcess() : [", this.m_Id, "] 길드를 길드전 [", this.m_BattleGroundType, "] 타입으로 설정하였습니다" }));
            }
            else
            {
                WorkSession.WriteStatus(string.Concat(new object[] { "GuildBattleGroundTypeUpdateCommand.DoProcess() : [", this.m_Id, "] 길드를 길드전 [", this.m_BattleGroundType, "] 타입 설정을 실패하였습니다" }));
            }
            return (this.m_Result == 0);
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result == 0)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            message.WriteS32(this.m_Result);
            return message;
        }

        protected override void ReceiveData(Message _message)
        {
            this.m_Id = (long) _message.ReadU64();
            this.m_GuildPoint = _message.ReadS32();
            this.m_GuildMoney = _message.ReadS32();
            this.m_BattleGroundType = _message.ReadU8();
        }
    }
}

