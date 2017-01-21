namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class ChronicleInfoBuilder
    {
        public static Hashtable Build(DataTable _table)
        {
            Hashtable hashtable = new Hashtable();
            if (_table == null)
            {
                throw new Exception("탐사연표 이미지 테이블이 없습니다.");
            }
            if ((_table.Rows != null) || (_table.Rows.Count > 0))
            {
                foreach (DataRow row in _table.Rows)
                {
                    ChronicleInfo info = new ChronicleInfo();
                    info.questID = (int) row["questID"];
                    info.questName = (string) row["questName"];
                    info.keyword = (string) row["keyword"];
                    info.localtext = (string) row["localtext"];
                    info.sort = (string) row["sort"];
                    info.group = (string) row["group"];
                    info.source = (string) row["source"];
                    info.width = (short) row["width"];
                    info.height = (short) row["height"];
                    hashtable[info.questID] = info;
                }
            }
            return hashtable;
        }
    }
}

