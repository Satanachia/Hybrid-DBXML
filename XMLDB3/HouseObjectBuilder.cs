namespace XMLDB3
{
    using System;
    using System.Data;

    public class HouseObjectBuilder
    {
        public static House Build(DataSet _ds)
        {
            DataTable table = _ds.Tables["housebid"];
            DataTable table2 = _ds.Tables["housebidder"];
            DataTable table3 = _ds.Tables["house"];
            if (((table == null) || (table2 == null)) || (table3 == null))
            {
                return null;
            }
            if ((table3.Rows == null) || (table3.Rows.Count < 1))
            {
                return null;
            }
            House house = new House();
            house.houseID = (long) table3.Rows[0]["houseID"];
            if (!table3.Rows[0].IsNull("account"))
            {
                house.account = (string) table3.Rows[0]["account"];
            }
            else
            {
                house.account = string.Empty;
            }
            house.constructed = (byte) table3.Rows[0]["constructed"];
            house.updateTime = (DateTime) table3.Rows[0]["updateTime"];
            house.charName = (string) table3.Rows[0]["charName"];
            house.houseName = (string) table3.Rows[0]["houseName"];
            house.houseClass = (int) table3.Rows[0]["houseClass"];
            house.roofSkin = (byte) table3.Rows[0]["roofSkin"];
            house.roofColor1 = (byte) table3.Rows[0]["roofColor1"];
            house.roofColor2 = (byte) table3.Rows[0]["roofColor2"];
            house.roofColor3 = (byte) table3.Rows[0]["roofColor3"];
            house.wallSkin = (byte) table3.Rows[0]["wallSkin"];
            house.wallColor1 = (byte) table3.Rows[0]["wallColor1"];
            house.wallColor2 = (byte) table3.Rows[0]["wallColor2"];
            house.wallColor3 = (byte) table3.Rows[0]["wallColor3"];
            house.innerSkin = (byte) table3.Rows[0]["innerSkin"];
            house.innerColor1 = (byte) table3.Rows[0]["innerColor1"];
            house.innerColor2 = (byte) table3.Rows[0]["innerColor2"];
            house.innerColor3 = (byte) table3.Rows[0]["innerColor3"];
            house.width = (int) table3.Rows[0]["width"];
            house.height = (int) table3.Rows[0]["height"];
            if (!table3.Rows[0].IsNull("bidSuccessDate"))
            {
                house.bidSuccessDate = (DateTime) table3.Rows[0]["bidSuccessDate"];
            }
            else
            {
                house.bidSuccessDate = DateTime.MinValue;
            }
            if (!table3.Rows[0].IsNull("taxPrevDate"))
            {
                house.taxPrevDate = (DateTime) table3.Rows[0]["taxPrevDate"];
            }
            else
            {
                house.taxPrevDate = DateTime.MinValue;
            }
            if (!table3.Rows[0].IsNull("taxNextDate"))
            {
                house.taxNextDate = (DateTime) table3.Rows[0]["taxNextDate"];
            }
            else
            {
                house.taxNextDate = DateTime.MinValue;
            }
            house.taxPrice = (int) table3.Rows[0]["taxPrice"];
            house.taxAutopay = (byte) table3.Rows[0]["taxAutopay"];
            house.houseMoney = (int) table3.Rows[0]["houseMoney"];
            house.deposit = (int) table3.Rows[0]["deposit"];
            house.flag = (long) table3.Rows[0]["flag"];
            if ((table.Rows != null) && (table.Rows.Count > 0))
            {
                house.bid = new HouseBid();
                house.bid.bidEndTime = (DateTime) table.Rows[0]["bidEndTime"];
                house.bid.bidRepayEndTime = (DateTime) table.Rows[0]["bidRepayEndTime"];
                house.bid.minBidPrice = (int) table.Rows[0]["minBidPrice"];
            }
            if ((table2.Rows != null) && (table2.Rows.Count > 0))
            {
                house.bidders = new HouseBidder[table2.Rows.Count];
                for (int i = 0; i < table2.Rows.Count; i++)
                {
                    house.bidders[i] = new HouseBidder();
                    house.bidders[i].bidAccount = (string) table2.Rows[i]["bidAccount"];
                    house.bidders[i].bidPrice = (int) table2.Rows[i]["bidPrice"];
                    house.bidders[i].bidOrder = (int) table2.Rows[i]["bidOrder"];
                    house.bidders[i].bidCharName = (string) table2.Rows[i]["bidCharName"];
                }
            }
            return house;
        }
    }
}

