namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class DungeonRankUpdateCommand : BasicCommand
    {
        private DungeonRank m_dungeonRank = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 던전 랭킹을 기록합니다");
            this.m_Result = QueryManager.DungeonRank.Update(this.m_dungeonRank);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 던전 랭킹을 기록하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 던전 랭킹 기록에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("DungeonRankUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_dungeonRank = DungeonRankSerializer.Serialize(_Msg);
        }
    }
}

