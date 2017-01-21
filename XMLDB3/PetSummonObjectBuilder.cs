namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetSummonObjectBuilder
    {
        public static PetSummon Build(DataRow _pet_row)
        {
            PetSummon summon = new PetSummon();
            summon.loyalty = (byte) _pet_row["loyalty"];
            summon.favor = (byte) _pet_row["favor"];
            return summon;
        }
    }
}

