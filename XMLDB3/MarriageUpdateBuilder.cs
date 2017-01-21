namespace XMLDB3
{
    using System;

    public class MarriageUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.marriage == null) || (_old.marriage == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.marriage.mateid != _old.marriage.mateid)
            {
                str = str + ",[mateID]=" + _new.marriage.mateid;
            }
            if (_new.marriage.matename != _old.marriage.matename)
            {
                str = str + ",[mateName]=" + UpdateUtility.BuildString(_new.marriage.matename);
            }
            if (_new.marriage.marriagetime != _old.marriage.marriagetime)
            {
                str = str + ",[marriageTime]=" + _new.marriage.marriagetime;
            }
            if (_new.marriage.marriagecount != _old.marriage.marriagecount)
            {
                str = str + ",[marriageCount]=" + _new.marriage.marriagecount;
            }
            return str;
        }
    }
}

