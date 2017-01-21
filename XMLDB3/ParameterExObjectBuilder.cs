namespace XMLDB3
{
    using System;
    using System.Data;

    public class ParameterExObjectBuilder
    {
        public static CharacterParameterEx Build(DataRow _character_row)
        {
            CharacterParameterEx ex = new CharacterParameterEx();
            ex.str_boost = (byte) _character_row["str_boost"];
            ex.dex_boost = (byte) _character_row["dex_boost"];
            ex.int_boost = (byte) _character_row["int_boost"];
            ex.will_boost = (byte) _character_row["will_boost"];
            ex.luck_boost = (byte) _character_row["luck_boost"];
            ex.height_boost = (byte) _character_row["height_boost"];
            ex.fatness_boost = (byte) _character_row["fatness_boost"];
            ex.upper_boost = (byte) _character_row["upper_boost"];
            ex.lower_boost = (byte) _character_row["lower_boost"];
            ex.life_boost = (byte) _character_row["life_boost"];
            ex.mana_boost = (byte) _character_row["mana_boost"];
            ex.stamina_boost = (byte) _character_row["stamina_boost"];
            ex.toxic = (float) _character_row["toxic"];
            ex.toxic_drunken_time = (long) _character_row["toxic_drunken_time"];
            ex.toxic_str = (float) _character_row["toxic_str"];
            ex.toxic_int = (float) _character_row["toxic_int"];
            ex.toxic_dex = (float) _character_row["toxic_dex"];
            ex.toxic_will = (float) _character_row["toxic_will"];
            ex.toxic_luck = (float) _character_row["toxic_luck"];
            ex.lastdungeon = (string) _character_row["lastdungeon"];
            ex.lasttown = (string) _character_row["lasttown"];
            ex.exploLevel = (short) _character_row["exploLevel"];
            ex.exploMaxKeyLevel = (short) _character_row["exploMaxKeyLevel"];
            ex.exploCumLevel = (int) _character_row["exploCumLevel"];
            ex.exploExp = (long) _character_row["exploExp"];
            ex.discoverCount = (int) _character_row["discoverCount"];
            return ex;
        }
    }
}

