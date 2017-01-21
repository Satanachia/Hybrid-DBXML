namespace XMLDB3
{
    using System;
    using System.Data;

    public class FarmObjectBuilder
    {
        public static CharacterFarm Build(DataRow _character_row)
        {
            CharacterFarm farm = new CharacterFarm();
            farm.farmID = (long) _character_row["farmID"];
            return farm;
        }
    }
}

