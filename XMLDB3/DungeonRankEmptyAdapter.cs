namespace XMLDB3
{
    using System;

    public class DungeonRankEmptyAdapter : DungeonRankAdapter
    {
        public void Initialize(string _Argument)
        {
        }

        public bool Update(DungeonRank _dungeonRank)
        {
            return false;
        }
    }
}

