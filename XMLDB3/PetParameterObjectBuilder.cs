namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetParameterObjectBuilder
    {
        public static PetParameter Build(DataRow _pet_row)
        {
            PetParameter parameter = new PetParameter();
            parameter.life = (float) _pet_row["life"];
            parameter.life_damage = (float) _pet_row["life_damage"];
            parameter.life_max = (float) _pet_row["life_max"];
            parameter.mana = (float) _pet_row["mana"];
            parameter.mana_max = (float) _pet_row["mana_max"];
            parameter.stamina = (float) _pet_row["stamina"];
            parameter.stamina_max = (float) _pet_row["stamina_max"];
            parameter.food = (float) _pet_row["food"];
            parameter.level = (short) _pet_row["level"];
            parameter.cumulatedlevel = (int) _pet_row["cumulatedlevel"];
            parameter.maxlevel = (short) _pet_row["maxlevel"];
            parameter.rebirthcount = (short) _pet_row["rebirthcount"];
            parameter.experience = (long) _pet_row["experience"];
            parameter.age = (short) _pet_row["age"];
            parameter.strength = (float) _pet_row["strength"];
            parameter.dexterity = (float) _pet_row["dexterity"];
            parameter.intelligence = (float) _pet_row["intelligence"];
            parameter.will = (float) _pet_row["will"];
            parameter.luck = (float) _pet_row["luck"];
            parameter.attack_min = (short) _pet_row["attack_min"];
            parameter.attack_max = (short) _pet_row["attack_max"];
            parameter.wattack_min = (short) _pet_row["wattack_min"];
            parameter.wattack_max = (short) _pet_row["wattack_max"];
            parameter.critical = (float) _pet_row["critical"];
            parameter.protect = (float) _pet_row["protect"];
            parameter.defense = (short) _pet_row["defense"];
            parameter.rate = (short) _pet_row["rate"];
            return parameter;
        }
    }
}

