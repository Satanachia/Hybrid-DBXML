namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class ChronicleCacheBuilder
    {
        public static Hashtable Build(DataTable _table, ChronicleInfo[] _infos)
        {
            Hashtable hashtable = new Hashtable();
            if (_table == null)
            {
                throw new Exception("탐사연표 랭킹 테이블이 없습니다.");
            }
            if ((_table.Rows != null) || (_table.Rows.Count > 0))
            {
                foreach (DataRow row in _table.Rows)
                {
                    int num = (int) row["questID"];
                    hashtable[num] = (int) row["totalCount"];
                }
            }
            if ((_infos != null) && (_infos.Length > 0))
            {
                foreach (ChronicleInfo info in _infos)
                {
                    if (info.IsRankingChronicle && !hashtable.ContainsKey(info.questID))
                    {
                        hashtable[info.questID] = 0;
                    }
                }
            }
            return hashtable;
        }
    }
}

