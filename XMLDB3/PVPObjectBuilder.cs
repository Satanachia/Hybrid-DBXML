namespace XMLDB3
{
    using System;
    using System.Data;

    public class PVPObjectBuilder
    {
        public static CharacterPVP Build(DataTable _pvpTable)
        {
            CharacterPVP rpvp = new CharacterPVP();
            if ((_pvpTable != null) && (_pvpTable.Rows.Count >= 1))
            {
                rpvp.winCnt = (long) _pvpTable.Rows[0]["winCnt"];
                rpvp.loseCnt = (long) _pvpTable.Rows[0]["loseCnt"];
                rpvp.penaltyPoint = (int) _pvpTable.Rows[0]["penaltyPoint"];
            }
            return rpvp;
        }
    }
}

