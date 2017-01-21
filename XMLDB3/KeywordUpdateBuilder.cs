namespace XMLDB3
{
    using System;

    public class KeywordUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            string str = BuildKeywordXmlData(_new.keywords);
            string str2 = BuildKeywordXmlData(_old.keywords);
            if (str != str2)
            {
                return (",[keyword]=" + UpdateUtility.BuildString(str));
            }
            return string.Empty;
        }

        private static string BuildKeywordXmlData(CharacterKeyword[] _keywords)
        {
            if ((_keywords == null) || (_keywords.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<keywords>";
            foreach (CharacterKeyword keyword in _keywords)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<keyword keyword=\"", keyword.keyword, "\"/>" });
            }
            return (str + "</keywords>");
        }
    }
}

