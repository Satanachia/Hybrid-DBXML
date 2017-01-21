namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class DungeonRankSerializer
    {
        public static DungeonRank Serialize(Message _message)
        {
            DungeonRank rank = new DungeonRank();
            rank.server = _message.ReadString();
            rank.characterID = _message.ReadS64();
            rank.characterName = _message.ReadString();
            rank.dungeonName = _message.ReadString();
            rank.race = _message.ReadU8();
            rank.score = _message.ReadS32();
            rank.laptime = _message.ReadS32();
            return rank;
        }
    }
}

