namespace XMLDB3
{
    using System;

    public class UserDataUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.data == null) || (_old.data == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.data.meta != _old.data.meta)
            {
                str = str + ",[meta]=" + UpdateUtility.BuildString(_new.data.meta);
            }
            if (_new.data.nao_favor != _old.data.nao_favor)
            {
                str = str + ",[nao_favor]=" + _new.data.nao_favor;
            }
            if (_new.data.nao_memory != _old.data.nao_memory)
            {
                str = str + ",[nao_memory]=" + _new.data.nao_memory;
            }
            if (_new.data.nao_style != _old.data.nao_style)
            {
                str = str + ",[nao_style]=" + _new.data.nao_style;
            }
            if (_new.data.playtime != _old.data.playtime)
            {
                str = str + ",[playtime]=" + _new.data.playtime;
            }
            if (_new.data.birthday != _old.data.birthday)
            {
                str = str + ",[birthday]=" + UpdateUtility.BuildDateTime(_new.data.birthday);
            }
            if (_new.data.rebirthday != _old.data.rebirthday)
            {
                str = str + ",[rebirthday]=" + UpdateUtility.BuildDateTime(_new.data.rebirthday);
            }
            if (_new.data.rebirthage != _old.data.rebirthage)
            {
                str = str + ",[rebirthage]=" + _new.data.rebirthage;
            }
            if (_new.data.wealth != _old.data.wealth)
            {
                str = str + ",[wealth]=" + _new.data.wealth;
            }
            if (_new.data.writeCounter != _old.data.writeCounter)
            {
                str = str + ",[writeCounter]=" + _new.data.writeCounter;
            }
            return str;
        }
    }
}

