namespace XMLDB3
{
    using System;
    using System.Data;

    public class HouseBlockObjectBuilder
    {
        public static HouseBlockList Build(DataTable _blockTable)
        {
            if (_blockTable == null)
            {
                return null;
            }
            HouseBlockList list = new HouseBlockList();
            if (_blockTable.Rows.Count > 0)
            {
                list.block = new HouseBlock[_blockTable.Rows.Count];
                for (int i = 0; i < _blockTable.Rows.Count; i++)
                {
                    list.block[i] = new HouseBlock();
                    list.block[i].gameName = (string) _blockTable.Rows[i]["gameName"];
                    list.block[i].flag = (byte) _blockTable.Rows[i]["flag"];
                }
            }
            return list;
        }
    }
}

