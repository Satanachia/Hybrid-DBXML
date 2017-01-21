namespace XMLDB3
{
    using System;
    using System.Text;

    public class CharacterCreateBuilder
    {
        public static string Build(CharacterInfo _new)
        {
            if (_new == null)
            {
                throw new ArgumentNullException("CharacterInfo", "캐릭터 데이터가 없습니다.");
            }
            StringBuilder builder = new StringBuilder(0x7d0);
            builder.Append(BuildGameId(_new.id, _new.name));
            builder.Append(BuildCharacter(_new));
            builder.Append(InventoryCreateBuilder.Build(_new.id, _new.inventory));
            return builder.ToString();
        }

        private static string BuildCharacter(CharacterInfo _new)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("insert into [");
            builder.Append("character");
            builder.Append("] ");
            builder.Append("(id, name, type, skin_color, eye_type, eye_color, mouth_type, status, \r\n\t\t\t\theight, fatness, [upper], [lower], region, x, y, direction, battle_state,\r\n\t\t\t\tweapon_set, life, life_damage, life_max,\r\n\t\t\t\tmana, mana_max, stamina, stamina_max, food, [level], cumulatedlevel, experience,\r\n\t\t\t\tage, strength, dexterity, intelligence, will, luck,\r\n\t\t\t\tlife_max_by_food, mana_max_by_food, stamina_max_by_food, strength_by_food, dexterity_by_food, intelligence_by_food, will_by_food, luck_by_food, ability_remain, \r\n\t\t\t\tattack_min, attack_max, wattack_min, wattack_max, critical, protect, \r\n\t\t\t\tdefense, rate, str_boost, dex_boost, int_boost, will_boost, luck_boost, height_boost, \r\n\t\t\t\tfatness_boost, upper_boost, lower_boost, life_boost, mana_boost, stamina_boost, \r\n\t\t\t\ttoxic, toxic_drunken_time, toxic_str, toxic_int, toxic_dex, toxic_will, toxic_luck, \r\n\t\t\t\tlasttown, lastdungeon, exploLevel, exploMaxKeyLevel, exploCumLevel, exploExp, discoverCount, meta, nao_memory, nao_favor, birthday, \r\n\t\t\t\tplaytime, wealth, writeCounter, condition, collection, history, keyword, memory, title, \r\n\t\t\t\treserved, book, update_time, delete_time, maxlevel, rebirthcount, lifetimeskill, \r\n\t\t\t\tnsrespawncount, nslastrespawnday, nsgiftreceiveday, apgiftreceiveday, rank1, rank2, \r\n\t\t\t\trebirthday, rebirthage, nao_style, score, npc_event_daycount, npc_event_bitflag, \r\n\t\t\t\tmateID, mateName, marriageTime, marriageCount, nsbombcount, nsbombday, farmID, \r\n\t\t\t\theartUpdateTime, heartPoint, heartTotalPoint,\r\n\t\t\t\tjoustPoint, joustLastWinYear, joustLastWinWeek, joustWeekWinCount,\r\n\t\t\t\tjoustDailyWinCount, joustDailyLoseCount, joustServerWinCount, joustServerLoseCount, macroPoint,\r\n\t\t\t\tdonationValue, donationUpdate, jobId) ");
            builder.Append("values(");
            builder.AppendFormat("{0}, ", _new.id);
            builder.AppendFormat("{0}, ", UpdateUtility.BuildString(_new.name));
            builder.AppendFormat("{0}, ", _new.appearance.type);
            builder.AppendFormat("{0}, ", _new.appearance.skin_color);
            builder.AppendFormat("{0}, ", _new.appearance.eye_type);
            builder.AppendFormat("{0}, ", _new.appearance.eye_color);
            builder.AppendFormat("{0}, ", _new.appearance.mouth_type);
            builder.AppendFormat("{0}, ", _new.appearance.status);
            builder.AppendFormat("{0}, ", _new.appearance.height);
            builder.AppendFormat("{0}, ", _new.appearance.fatness);
            builder.AppendFormat("{0}, ", _new.appearance.upper);
            builder.AppendFormat("{0}, ", _new.appearance.lower);
            builder.AppendFormat("{0}, ", _new.appearance.region);
            builder.AppendFormat("{0}, ", _new.appearance.x);
            builder.AppendFormat("{0}, ", _new.appearance.y);
            builder.AppendFormat("{0}, ", _new.appearance.direction);
            builder.AppendFormat("{0}, ", _new.appearance.battle_state);
            builder.AppendFormat("{0}, ", _new.appearance.weapon_set);
            builder.AppendFormat("{0}, ", _new.parameter.life);
            builder.AppendFormat("{0}, ", _new.parameter.life_damage);
            builder.AppendFormat("{0}, ", _new.parameter.life_max);
            builder.AppendFormat("{0}, ", _new.parameter.mana);
            builder.AppendFormat("{0}, ", _new.parameter.mana_max);
            builder.AppendFormat("{0}, ", _new.parameter.stamina);
            builder.AppendFormat("{0}, ", _new.parameter.stamina_max);
            builder.AppendFormat("{0}, ", _new.parameter.food);
            builder.AppendFormat("{0}, ", _new.parameter.level);
            builder.AppendFormat("{0}, ", _new.parameter.cumulatedlevel);
            builder.AppendFormat("{0}, ", _new.parameter.experience);
            builder.AppendFormat("{0}, ", _new.parameter.age);
            builder.AppendFormat("{0}, ", _new.parameter.strength);
            builder.AppendFormat("{0}, ", _new.parameter.dexterity);
            builder.AppendFormat("{0}, ", _new.parameter.intelligence);
            builder.AppendFormat("{0}, ", _new.parameter.will);
            builder.AppendFormat("{0}, ", _new.parameter.luck);
            builder.AppendFormat("{0}, ", _new.parameter.life_max_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.mana_max_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.stamina_max_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.strength_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.dexterity_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.intelligence_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.will_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.luck_by_food);
            builder.AppendFormat("{0}, ", _new.parameter.ability_remain);
            builder.AppendFormat("{0}, ", _new.parameter.attack_min);
            builder.AppendFormat("{0}, ", _new.parameter.attack_max);
            builder.AppendFormat("{0}, ", _new.parameter.wattack_min);
            builder.AppendFormat("{0}, ", _new.parameter.wattack_max);
            builder.AppendFormat("{0}, ", _new.parameter.critical);
            builder.AppendFormat("{0}, ", _new.parameter.protect);
            builder.AppendFormat("{0}, ", _new.parameter.defense);
            builder.AppendFormat("{0}, ", _new.parameter.rate);
            builder.AppendFormat("{0}, ", _new.parameterEx.str_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.dex_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.int_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.will_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.luck_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.height_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.fatness_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.upper_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.lower_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.life_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.mana_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.stamina_boost);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic_drunken_time);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic_str);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic_int);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic_dex);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic_will);
            builder.AppendFormat("{0}, ", _new.parameterEx.toxic_luck);
            builder.AppendFormat("{0}, ", UpdateUtility.BuildString(_new.parameterEx.lasttown));
            builder.AppendFormat("{0}, ", UpdateUtility.BuildString(_new.parameterEx.lastdungeon));
            builder.AppendFormat("{0}, ", _new.parameterEx.exploLevel);
            builder.AppendFormat("{0}, ", _new.parameterEx.exploMaxKeyLevel);
            builder.AppendFormat("{0}, ", _new.parameterEx.exploCumLevel);
            builder.AppendFormat("{0}, ", _new.parameterEx.exploExp);
            builder.AppendFormat("{0}, ", _new.parameterEx.discoverCount);
            builder.AppendFormat("{0}, ", UpdateUtility.BuildString(_new.data.meta));
            builder.AppendFormat("{0}, ", _new.data.nao_memory);
            builder.AppendFormat("{0}, ", _new.data.nao_favor);
            builder.AppendFormat("{0}, ", UpdateUtility.BuildDateTime(_new.data.birthday));
            builder.AppendFormat("{0}, ", _new.data.playtime);
            builder.AppendFormat("{0}, ", _new.data.wealth);
            builder.AppendFormat("{0}, ", _new.data.writeCounter);
            builder.Append("'', ");
            builder.Append("'', ");
            builder.Append("'', ");
            builder.Append("'', ");
            builder.Append("'', ");
            builder.Append("'<titles selected=\"0\" appliedtime=\"0\" option=\"0\"></titles>', ");
            builder.Append("'', ");
            builder.Append("'', ");
            builder.Append("NULL, ");
            builder.Append("NULL, ");
            builder.AppendFormat("{0}, ", _new.parameter.maxlevel);
            builder.AppendFormat("{0}, ", _new.parameter.rebirthcount);
            builder.AppendFormat("{0}, ", _new.parameter.lifetimeskill);
            builder.AppendFormat("{0}, ", _new.service.nsrespawncount);
            builder.AppendFormat("{0}, ", _new.service.nslastrespawnday);
            builder.AppendFormat("{0}, ", _new.service.nsgiftreceiveday);
            builder.AppendFormat("{0}, ", _new.service.apgiftreceiveday);
            builder.AppendFormat("{0}, ", _new.parameter.rank1);
            builder.AppendFormat("{0}, ", _new.parameter.rank2);
            builder.Append("NULL, ");
            builder.AppendFormat("{0}, ", _new.data.rebirthage);
            builder.AppendFormat("{0}, ", _new.data.nao_style);
            builder.AppendFormat("{0}, ", _new.parameter.score);
            builder.AppendFormat("{0}, ", _new.@private.npc_event_daycount);
            builder.AppendFormat("{0}, ", _new.@private.npc_event_bitflag);
            builder.AppendFormat("{0}, ", _new.marriage.mateid);
            builder.AppendFormat("{0}, ", UpdateUtility.BuildString(_new.marriage.matename));
            builder.AppendFormat("{0}, ", _new.marriage.marriagetime);
            builder.AppendFormat("{0}, ", _new.marriage.marriagecount);
            builder.AppendFormat("{0}, ", _new.service.nsbombcount);
            builder.AppendFormat("{0}, ", _new.service.nsbombday);
            builder.AppendFormat("{0}, ", _new.farm.farmID);
            builder.AppendFormat("{0},  ", _new.heartSticker.heartUpdateTime);
            builder.AppendFormat("{0},  ", _new.heartSticker.heartPoint);
            builder.AppendFormat("{0},  ", _new.heartSticker.heartTotalPoint);
            builder.AppendFormat("{0},  ", _new.joust.joustPoint);
            builder.AppendFormat("{0},  ", _new.joust.joustLastWinYear);
            builder.AppendFormat("{0},  ", _new.joust.joustLastWinWeek);
            builder.AppendFormat("{0},  ", _new.joust.joustWeekWinCount);
            builder.AppendFormat("{0},  ", _new.joust.joustDailyWinCount);
            builder.AppendFormat("{0},  ", _new.joust.joustDailyLoseCount);
            builder.AppendFormat("{0},  ", _new.joust.joustServerWinCount);
            builder.AppendFormat("{0},  ", _new.joust.joustServerLoseCount);
            builder.AppendFormat("{0},  ", _new.macroChecker.macroPoint);
            builder.AppendFormat("{0},  ", _new.donation.donationValue);
            builder.AppendFormat("{0},  ", _new.donation.donationUpdate);
            builder.AppendFormat("{0})\n", _new.job.jobId);
            builder.Append("insert into CharacterAchievement (id, totalscore, achievement) ");
            builder.Append("values(");
            builder.AppendFormat("{0},  ", _new.id);
            builder.Append("0,  ");
            builder.Append("'')\n");
            return builder.ToString();
        }

        private static string BuildGameId(long _characterId, string _charactername)
        {
            return string.Concat(new object[] { "exec dbo.CreateGameID @id=", _characterId, ",@name=", UpdateUtility.BuildString(_charactername), ",@flag=1\n" });
        }
    }
}

