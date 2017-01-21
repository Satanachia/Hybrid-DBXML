namespace XMLDB3
{
    using System;
    using System.Data;

    public class DataObjectBuilder
    {
        public static CharacterData Build(DataRow _character_row)
        {
            CharacterData data = new CharacterData();
            data.meta = (string) _character_row["meta"];
            data.nao_memory = (short) _character_row["nao_memory"];
            data.nao_favor = (short) _character_row["nao_favor"];
            data.nao_style = (byte) _character_row["nao_style"];
            data.birthday = (DateTime) _character_row["birthday"];
            if (_character_row.IsNull("rebirthday"))
            {
                data.rebirthday = DateTime.MinValue;
            }
            else
            {
                data.rebirthday = (DateTime) _character_row["rebirthday"];
            }
            data.rebirthage = (short) _character_row["rebirthage"];
            data.playtime = (int) _character_row["playtime"];
            data.wealth = (int) _character_row["wealth"];
            data.writeCounter = (byte) _character_row["writeCounter"];
            return data;
        }
    }
}

