namespace XMLDB3
{
    using System;
    using System.Data;

    public class MarriageObjectBuilder
    {
        public static CharacterMarriage Build(DataRow _character_row)
        {
            CharacterMarriage marriage = new CharacterMarriage();
            marriage.mateid = (long) _character_row["mateID"];
            marriage.matename = (string) _character_row["mateName"];
            marriage.marriagetime = (int) _character_row["marriageTime"];
            marriage.marriagecount = (short) _character_row["marriageCount"];
            return marriage;
        }
    }
}

