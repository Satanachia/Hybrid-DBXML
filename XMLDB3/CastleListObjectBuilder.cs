namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class CastleListObjectBuilder
    {
        public static CastleList Build(DataSet _ds)
        {
            if (_ds.Tables == null)
            {
                return null;
            }
            DataTable table = _ds.Tables["castlebid"];
            DataTable table2 = _ds.Tables["castlebidder"];
            DataTable table3 = _ds.Tables["castle"];
            DataTable table4 = _ds.Tables["castleBuildResource"];
            DataTable table5 = _ds.Tables["castleBlock"];
            if (((table == null) || (table2 == null)) || (((table3 == null) || (table4 == null)) || (table5 == null)))
            {
                return null;
            }
            CastleList list = new CastleList();
            if ((table.Rows != null) && (table.Rows.Count > 0))
            {
                list.bids = new CastleBid[table.Rows.Count];
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    list.bids[i] = new CastleBid();
                    list.bids[i].castleID = (long) table.Rows[i]["castleID"];
                    list.bids[i].bidEndTime = (DateTime) table.Rows[i]["bidEndTime"];
                    list.bids[i].minBidPrice = (int) table.Rows[i]["minBidPrice"];
                }
            }
            if ((table2.Rows != null) && (table2.Rows.Count > 0))
            {
                list.bidders = new CastleBidder[table2.Rows.Count];
                for (int j = 0; j < table2.Rows.Count; j++)
                {
                    list.bidders[j] = new CastleBidder();
                    list.bidders[j].castleID = (long) table2.Rows[j]["castleID"];
                    list.bidders[j].bidGuildID = (long) table2.Rows[j]["bidGuildID"];
                    list.bidders[j].bidPrice = (int) table2.Rows[j]["bidPrice"];
                    list.bidders[j].bidOrder = (int) table2.Rows[j]["bidOrder"];
                }
            }
            Hashtable hashtable = new Hashtable();
            if ((table4.Rows != null) && (table4.Rows.Count > 0))
            {
                foreach (DataRow row in table4.Rows)
                {
                    CastleBuildResource resource = new CastleBuildResource();
                    long key = (long) row["castleID"];
                    resource.classID = (int) row["classID"];
                    resource.curAmount = (int) row["curAmount"];
                    resource.maxAmount = (int) row["maxAmount"];
                    ArrayList list2 = null;
                    if (hashtable.ContainsKey(key))
                    {
                        list2 = (ArrayList) hashtable[key];
                    }
                    else
                    {
                        list2 = new ArrayList();
                        hashtable[key] = list2;
                    }
                    list2.Add(resource);
                }
            }
            if ((table3.Rows != null) && (table3.Rows.Count > 0))
            {
                list.castles = new Castle[table3.Rows.Count];
                for (int k = 0; k < table3.Rows.Count; k++)
                {
                    list.castles[k] = new Castle();
                    list.castles[k].castleID = (long) table3.Rows[k]["castleID"];
                    list.castles[k].guildID = (long) table3.Rows[k]["guildID"];
                    list.castles[k].constructed = (byte) table3.Rows[k]["constructed"];
                    list.castles[k].castleMoney = (int) table3.Rows[k]["castleMoney"];
                    list.castles[k].weeklyIncome = (int) table3.Rows[k]["weeklyIncome"];
                    list.castles[k].taxrate = (byte) table3.Rows[k]["taxrate"];
                    list.castles[k].updateTime = (DateTime) table3.Rows[k]["updateTime"];
                    list.castles[k].sellDungeonPass = (byte) table3.Rows[k]["sellDungeonPass"];
                    list.castles[k].dungeonPassPrice = (int) table3.Rows[k]["dungeonPassPrice"];
                    list.castles[k].flag = (long) table3.Rows[k]["flag"];
                    list.castles[k].build = new CastleBuild();
                    list.castles[k].build.durability = (int) table3.Rows[k]["durability"];
                    list.castles[k].build.maxDurability = (int) table3.Rows[k]["maxDurability"];
                    list.castles[k].build.buildState = (byte) table3.Rows[k]["buildState"];
                    if (!table3.Rows[k].IsNull("buildNextTime"))
                    {
                        list.castles[k].build.buildNextTime = (DateTime) table3.Rows[k]["buildNextTime"];
                    }
                    else
                    {
                        list.castles[k].build.buildNextTime = DateTime.MinValue;
                    }
                    list.castles[k].build.buildStep = (byte) table3.Rows[k]["buildStep"];
                    if (hashtable.ContainsKey(list.castles[k].castleID))
                    {
                        ArrayList list3 = (ArrayList) hashtable[list.castles[k].castleID];
                        list.castles[k].build.resource = (CastleBuildResource[]) list3.ToArray(typeof(CastleBuildResource));
                    }
                }
            }
            if ((table5.Rows != null) && (table5.Rows.Count > 0))
            {
                Hashtable hashtable2 = new Hashtable();
                for (int m = 0; m < table5.Rows.Count; m++)
                {
                    long num6 = (long) table5.Rows[m]["castleID"];
                    ArrayList list4 = null;
                    if (!hashtable2.ContainsKey(num6))
                    {
                        list4 = new ArrayList();
                    }
                    else
                    {
                        list4 = (ArrayList) hashtable2[num6];
                    }
                    CastleBlock block = new CastleBlock();
                    block.gameName = (string) table5.Rows[m]["gameName"];
                    block.flag = (byte) table5.Rows[m]["flag"];
                    block.entry = (byte) table5.Rows[m]["entry"];
                    list4.Add(block);
                    hashtable2[num6] = list4;
                }
                list.blocks = new CastleBlockList[hashtable2.Count];
                int index = 0;
                foreach (long num8 in hashtable2.Keys)
                {
                    list.blocks[index] = new CastleBlockList();
                    list.blocks[index].castleID = num8;
                    list.blocks[index].block = (CastleBlock[]) ((ArrayList) hashtable2[num8]).ToArray(typeof(CastleBlock));
                    index++;
                }
            }
            return list;
        }
    }
}

