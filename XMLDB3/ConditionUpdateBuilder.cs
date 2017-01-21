namespace XMLDB3
{
    using System;

    public class ConditionUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            string str = BuildConditionXmlData(_new.conditions);
            string str2 = BuildConditionXmlData(_old.conditions);
            if (str != str2)
            {
                return (",[condition]=" + UpdateUtility.BuildString(str));
            }
            return string.Empty;
        }

        private static string BuildConditionXmlData(CharacterCondition[] _conditions)
        {
            if ((_conditions == null) || (_conditions.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<conditions>";
            foreach (CharacterCondition condition in _conditions)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<condition flag=\"", condition.flag, "\" timemode=\"", condition.timemode, "\" time=\"", condition.time, "\"" });
                if ((condition.meta != null) && (condition.meta.Length > 0))
                {
                    str = str + " meta=\"" + condition.meta + "\"";
                }
                str = str + "/>";
            }
            return (str + "</conditions>");
        }
    }
}

