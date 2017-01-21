namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetDataObjectBuilder
    {
        public static PetData Build(DataRow _pet_row)
        {
            PetData data = new PetData();
            data.ui = (string) _pet_row["ui"];
            data.meta = (string) _pet_row["meta"];
            data.birthday = (DateTime) _pet_row["birthday"];
            if (_pet_row.IsNull("rebirthday"))
            {
                data.rebirthday = DateTime.MinValue;
            }
            else
            {
                data.rebirthday = (DateTime) _pet_row["rebirthday"];
            }
            data.rebirthage = (short) _pet_row["rebirthage"];
            data.playtime = (int) _pet_row["playtime"];
            data.wealth = (int) _pet_row["wealth"];
            data.writeCounter = (byte) _pet_row["writeCounter"];
            return data;
        }
    }
}

