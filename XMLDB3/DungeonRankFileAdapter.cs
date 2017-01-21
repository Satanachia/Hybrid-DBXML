namespace XMLDB3
{
    using System;

    public class DungeonRankFileAdapter : FileAdapter, DungeonRankAdapter
    {
        private const string dungeonRankFileName = "dungeonrank";

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(DungeonRankList), ConfigManager.GetFileDBPath("dungeonrank"), ".xml");
        }

        public bool Update(DungeonRank _dungeonRank)
        {
            DungeonRank[] rankArray;
            if (!base.IsExistData("dungeonrank"))
            {
                DungeonRankList list = new DungeonRankList();
                list.dungeonRanks = new DungeonRank[] { _dungeonRank };
                base.WriteToDB(list, "dungeonrank");
                return true;
            }
            DungeonRankList list2 = (DungeonRankList) base.ReadFromDB("dungeonrank");
            if (list2 == null)
            {
                return false;
            }
            if ((list2.dungeonRanks == null) || (list2.dungeonRanks.Length == 0))
            {
                rankArray = new DungeonRank[1];
            }
            else
            {
                rankArray = new DungeonRank[list2.dungeonRanks.Length + 1];
                list2.dungeonRanks.CopyTo(rankArray, 0);
            }
            rankArray[list2.dungeonRanks.Length] = _dungeonRank;
            list2.dungeonRanks = rankArray;
            base.WriteToDB(list2, "dungeonrank");
            return true;
        }
    }
}

