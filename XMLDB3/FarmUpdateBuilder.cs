namespace XMLDB3
{
    using System;

    public class FarmUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.farm == null) || (_old.farm == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.farm.farmID != _old.farm.farmID)
            {
                str = str + ",[farmID]=" + _new.farm.farmID;
            }
            return str;
        }
    }
}

