namespace XMLDB3
{
    using System;
    using System.Text;

    public class PetCreateBuilder
    {
        public static string Build(PetInfo _new)
        {
            if (_new == null)
            {
                throw new ArgumentNullException("PetInfo", "팻 데이터가 없습니다.");
            }
            StringBuilder builder = new StringBuilder(0x7d0);
            builder.Append(BuildGameId(_new.id, _new.name));
            builder.Append(BuildPet(_new));
            builder.Append(InventoryCreateBuilder.Build(_new.id, _new.inventory));
            return builder.ToString();
        }

        private static string BuildGameId(long _petid, string _petname)
        {
            return string.Concat(new object[] { "exec dbo.CreateGameID @id=", _petid, ",@name=", UpdateUtility.BuildString(_petname), ",@flag=2\n" });
        }

        private static string BuildPet(Pet _new)
        {
            return string.Concat(new object[] { 
                "insert into [pet] (id, name, type, skin_color, eye_type, eye_color, mouth_type, status, height, fatness, [upper], [lower], region, x, y, direction, battle_state, extra_01, extra_02, extra_03, life, life_damage, life_max, mana, mana_max, stamina, stamina_max, food, [level], cumulatedlevel, maxlevel, rebirthcount, experience, age, strength, dexterity, intelligence, will, luck, attack_min, attack_max, wattack_min, wattack_max, critical, protect, defense, rate, str_boost, dex_boost, int_boost, will_boost, luck_boost, height_boost, fatness_boost, upper_boost, lower_boost, life_boost, mana_boost, stamina_boost, toxic, toxic_drunken_time, toxic_str, toxic_int, toxic_dex, toxic_will, toxic_luck, lasttown, lastdungeon, ui, meta, birthday, rebirthday, rebirthage, playtime, wealth, writeCounter, condition, memory, reserved, registered, loyalty, favor, update_time, delete_time, macroPoint) values(", _new.id, ",", UpdateUtility.BuildString(_new.name), ",", _new.appearance.type, ",", _new.appearance.skin_color, ",", _new.appearance.eye_type, ",", _new.appearance.eye_color, ",", _new.appearance.mouth_type, ",", _new.appearance.status, 
                ",", _new.appearance.height, ",", _new.appearance.fatness, ",", _new.appearance.upper, ",", _new.appearance.lower, ",", _new.appearance.region, ",", _new.appearance.x, ",", _new.appearance.y, ",", _new.appearance.direction, 
                ",", _new.appearance.battle_state, ",", _new.appearance.extra_01, ",", _new.appearance.extra_02, ",", _new.appearance.extra_03, ",", _new.parameter.life, ",", _new.parameter.life_damage, ",", _new.parameter.life_max, ",", _new.parameter.mana, 
                ",", _new.parameter.mana_max, ",", _new.parameter.stamina, ",", _new.parameter.stamina_max, ",", _new.parameter.food, ",", _new.parameter.level, ",", _new.parameter.cumulatedlevel, ",", _new.parameter.maxlevel, ",", _new.parameter.rebirthcount, 
                ",", _new.parameter.experience, ",", _new.parameter.age, ",", _new.parameter.strength, ",", _new.parameter.dexterity, ",", _new.parameter.intelligence, ",", _new.parameter.will, ",", _new.parameter.luck, ",", _new.parameter.attack_min, 
                ",", _new.parameter.attack_max, ",", _new.parameter.wattack_min, ",", _new.parameter.wattack_max, ",", _new.parameter.critical, ",", _new.parameter.protect, ",", _new.parameter.defense, ",", _new.parameter.rate, ",", _new.parameterEx.str_boost, 
                ",", _new.parameterEx.dex_boost, ",", _new.parameterEx.int_boost, ",", _new.parameterEx.will_boost, ",", _new.parameterEx.luck_boost, ",", _new.parameterEx.height_boost, ",", _new.parameterEx.fatness_boost, ",", _new.parameterEx.upper_boost, ",", _new.parameterEx.lower_boost, 
                ",", _new.parameterEx.life_boost, ",", _new.parameterEx.mana_boost, ",", _new.parameterEx.stamina_boost, ",", _new.parameterEx.toxic, ",", _new.parameterEx.toxic_drunken_time, ",", _new.parameterEx.toxic_str, ",", _new.parameterEx.toxic_int, ",", _new.parameterEx.toxic_dex, 
                ",", _new.parameterEx.toxic_will, ",", _new.parameterEx.toxic_luck, ",", UpdateUtility.BuildString(_new.parameterEx.lasttown), ",", UpdateUtility.BuildString(_new.parameterEx.lastdungeon), ",", UpdateUtility.BuildString(_new.data.ui), ",", UpdateUtility.BuildString(_new.data.meta), ",", UpdateUtility.BuildDateTime(_new.data.birthday), ",NULL ,", _new.data.rebirthage, 
                ",", _new.data.playtime, ",", _new.data.wealth, ",", _new.data.writeCounter, ",'','','','',", _new.summon.loyalty, ",", _new.summon.favor, ",NULL, NULL,", _new.macroChecker.macroPoint, ")\n"
             });
        }
    }
}

