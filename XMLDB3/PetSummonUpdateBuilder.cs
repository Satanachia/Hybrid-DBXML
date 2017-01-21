namespace XMLDB3
{
    using System;

    public class PetSummonUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if ((_new.data == null) || (_old.data == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.summon.loyalty != _old.summon.loyalty)
            {
                str = str + ",[loyalty]=" + _new.summon.loyalty;
            }
            if (_new.summon.favor != _old.summon.favor)
            {
                str = str + ",[favor]=" + _new.summon.favor;
            }
            return str;
        }
    }
}

