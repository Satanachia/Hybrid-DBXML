namespace XMLDB3
{
    using System;

    public class PetParameterExUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if ((_new.parameterEx == null) || (_old.parameterEx == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.parameterEx.str_boost != _old.parameterEx.str_boost)
            {
                str = str + ",[str_boost]=" + _new.parameterEx.str_boost;
            }
            if (_new.parameterEx.dex_boost != _old.parameterEx.dex_boost)
            {
                str = str + ",[dex_boost]=" + _new.parameterEx.dex_boost;
            }
            if (_new.parameterEx.int_boost != _old.parameterEx.int_boost)
            {
                str = str + ",[int_boost]=" + _new.parameterEx.int_boost;
            }
            if (_new.parameterEx.will_boost != _old.parameterEx.will_boost)
            {
                str = str + ",[will_boost]=" + _new.parameterEx.will_boost;
            }
            if (_new.parameterEx.luck_boost != _old.parameterEx.luck_boost)
            {
                str = str + ",[luck_boost]=" + _new.parameterEx.luck_boost;
            }
            if (_new.parameterEx.height_boost != _old.parameterEx.height_boost)
            {
                str = str + ",[height_boost]=" + _new.parameterEx.height_boost;
            }
            if (_new.parameterEx.fatness_boost != _old.parameterEx.fatness_boost)
            {
                str = str + ",[fatness_boost]=" + _new.parameterEx.fatness_boost;
            }
            if (_new.parameterEx.upper_boost != _old.parameterEx.upper_boost)
            {
                str = str + ",[upper_boost]=" + _new.parameterEx.upper_boost;
            }
            if (_new.parameterEx.lower_boost != _old.parameterEx.lower_boost)
            {
                str = str + ",[lower_boost]=" + _new.parameterEx.lower_boost;
            }
            if (_new.parameterEx.life_boost != _old.parameterEx.life_boost)
            {
                str = str + ",[life_boost]=" + _new.parameterEx.life_boost;
            }
            if (_new.parameterEx.mana_boost != _old.parameterEx.mana_boost)
            {
                str = str + ",[mana_boost]=" + _new.parameterEx.mana_boost;
            }
            if (_new.parameterEx.stamina_boost != _old.parameterEx.stamina_boost)
            {
                str = str + ",[stamina_boost]=" + _new.parameterEx.stamina_boost;
            }
            if (_new.parameterEx.toxic != _old.parameterEx.toxic)
            {
                str = str + ",[toxic]=" + _new.parameterEx.toxic;
            }
            if (_new.parameterEx.toxic_drunken_time != _old.parameterEx.toxic_drunken_time)
            {
                str = str + ",[toxic_drunken_time]=" + _new.parameterEx.toxic_drunken_time;
            }
            if (_new.parameterEx.toxic_str != _old.parameterEx.toxic_str)
            {
                str = str + ",[toxic_str]=" + _new.parameterEx.toxic_str;
            }
            if (_new.parameterEx.toxic_int != _old.parameterEx.toxic_int)
            {
                str = str + ",[toxic_int]=" + _new.parameterEx.toxic_int;
            }
            if (_new.parameterEx.toxic_dex != _old.parameterEx.toxic_dex)
            {
                str = str + ",[toxic_dex]=" + _new.parameterEx.toxic_dex;
            }
            if (_new.parameterEx.toxic_will != _old.parameterEx.toxic_will)
            {
                str = str + ",[toxic_will]=" + _new.parameterEx.toxic_will;
            }
            if (_new.parameterEx.toxic_luck != _old.parameterEx.toxic_luck)
            {
                str = str + ",[toxic_luck]=" + _new.parameterEx.toxic_luck;
            }
            if (_new.parameterEx.lasttown != _old.parameterEx.lasttown)
            {
                str = str + ",[lasttown]=" + UpdateUtility.BuildString(_new.parameterEx.lasttown);
            }
            if (_new.parameterEx.lastdungeon != _old.parameterEx.lastdungeon)
            {
                str = str + ",[lastdungeon]=" + UpdateUtility.BuildString(_new.parameterEx.lastdungeon);
            }
            return str;
        }
    }
}

