namespace XMLDB3
{
    using System;
    using System.Data;

    public class ParameterObjectBuilder
    {
        public static CharacterParameter Build(DataRow _character_row)
        {
            CharacterParameter parameter = new CharacterParameter();
            parameter.life = (float) _character_row["life"];
            parameter.life_damage = (float) _character_row["life_damage"];
            parameter.life_max = (float) _character_row["life_max"];
            parameter.mana = (float) _character_row["mana"];
            parameter.mana_max = (float) _character_row["mana_max"];
            parameter.stamina = (float) _character_row["stamina"];
            parameter.stamina_max = (float) _character_row["stamina_max"];
            parameter.food = (float) _character_row["food"];
            parameter.level = (short) _character_row["level"];
            parameter.cumulatedlevel = (int) _character_row["cumulatedlevel"];
            parameter.maxlevel = (short) _character_row["maxlevel"];
            parameter.rebirthcount = (short) _character_row["rebirthcount"];
            parameter.lifetimeskill = (short) _character_row["lifetimeskill"];
            parameter.experience = (long) _character_row["experience"];
            parameter.age = (short) _character_row["age"];
            parameter.strength = (float) _character_row["strength"];
            parameter.dexterity = (float) _character_row["dexterity"];
            parameter.intelligence = (float) _character_row["intelligence"];
            parameter.will = (float) _character_row["will"];
            parameter.luck = (float) _character_row["luck"];
            parameter.life_max_by_food = (float) _character_row["life_max_by_food"];
            parameter.mana_max_by_food = (float) _character_row["mana_max_by_food"];
            parameter.stamina_max_by_food = (float) _character_row["stamina_max_by_food"];
            parameter.strength_by_food = (float) _character_row["strength_by_food"];
            parameter.dexterity_by_food = (float) _character_row["dexterity_by_food"];
            parameter.intelligence_by_food = (float) _character_row["intelligence_by_food"];
            parameter.will_by_food = (float) _character_row["will_by_food"];
            parameter.luck_by_food = (float) _character_row["luck_by_food"];
            parameter.ability_remain = (short) _character_row["ability_remain"];
            parameter.attack_min = (short) _character_row["attack_min"];
            parameter.attack_max = (short) _character_row["attack_max"];
            parameter.wattack_min = (short) _character_row["wattack_min"];
            parameter.wattack_max = (short) _character_row["wattack_max"];
            parameter.critical = (float) _character_row["critical"];
            parameter.protect = (float) _character_row["protect"];
            parameter.defense = (short) _character_row["defense"];
            parameter.rate = (short) _character_row["rate"];
            parameter.rank1 = (short) _character_row["rank1"];
            parameter.rank2 = (short) _character_row["rank2"];
            parameter.score = (long) _character_row["score"];
            return parameter;
        }
    }
}

