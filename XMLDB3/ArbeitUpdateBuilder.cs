namespace XMLDB3
{
    using System;

    public class ArbeitUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_old.arbeit == null) || (_new.arbeit == null))
            {
                return string.Empty;
            }
            string str = BuildHistoryXmlData(_new.arbeit.history);
            string str2 = BuildHistoryXmlData(_old.arbeit.history);
            string str3 = BuildCollectionXmlData(_new.arbeit.collection);
            string str4 = BuildCollectionXmlData(_old.arbeit.collection);
            string str5 = string.Empty;
            if (str != str2)
            {
                str5 = str5 + ",[history]=" + UpdateUtility.BuildString(str);
            }
            if (str3 != str4)
            {
                str5 = str5 + ",[collection]=" + UpdateUtility.BuildString(str3);
            }
            return str5;
        }

        private static string BuildCollectionXmlData(CharacterArbeitInfo[] _collection)
        {
            if ((_collection == null) || (_collection.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<collection>";
            foreach (CharacterArbeitInfo info in _collection)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<Info category=\"", info.category, "\" total=\"", info.total, "\" success=\"", info.success, "\"/>" });
            }
            return (str + "</collection>");
        }

        private static string BuildHistoryXmlData(CharacterArbeitDay[] _history)
        {
            if ((_history == null) || (_history.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<history>";
            foreach (CharacterArbeitDay day in _history)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<day daycount=\"", day.daycount, "\">" });
                if ((day.info != null) && (day.info.Length > 0))
                {
                    foreach (CharacterArbeitDayInfo info in day.info)
                    {
                        obj2 = str;
                        str = string.Concat(new object[] { obj2, "<info category=\"", info.category, "\"/>" });
                    }
                }
                str = str + "</day>";
            }
            return (str + "</history>");
        }
    }
}

