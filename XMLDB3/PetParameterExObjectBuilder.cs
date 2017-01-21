namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetParameterExObjectBuilder
    {
        public static PetParameterEx Build(DataRow _pet_row)
        {
            PetParameterEx ex = new PetParameterEx();
            ex.str_boost = (byte) _pet_row["str_boost"];
            ex.dex_boost = (byte) _pet_row["dex_boost"];
            ex.int_boost = (byte) _pet_row["int_boost"];
            ex.will_boost = (byte) _pet_row["will_boost"];
            ex.luck_boost = (byte) _pet_row["luck_boost"];
            ex.height_boost = (byte) _pet_row["height_boost"];
            ex.fatness_boost = (byte) _pet_row["fatness_boost"];
            ex.upper_boost = (byte) _pet_row["upper_boost"];
            ex.lower_boost = (byte) _pet_row["lower_boost"];
            ex.life_boost = (byte) _pet_row["life_boost"];
            ex.mana_boost = (byte) _pet_row["mana_boost"];
            ex.stamina_boost = (byte) _pet_row["stamina_boost"];
            ex.toxic = (float) _pet_row["toxic"];
            ex.toxic_drunken_time = (long) _pet_row["toxic_drunken_time"];
            ex.toxic_str = (float) _pet_row["toxic_str"];
            ex.toxic_int = (float) _pet_row["toxic_int"];
            ex.toxic_dex = (float) _pet_row["toxic_dex"];
            ex.toxic_will = (float) _pet_row["toxic_will"];
            ex.toxic_luck = (float) _pet_row["toxic_luck"];
            ex.lastdungeon = (string) _pet_row["lastdungeon"];
            ex.lasttown = (string) _pet_row["lasttown"];
            return ex;
        }
    }
}

