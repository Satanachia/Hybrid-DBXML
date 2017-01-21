namespace XMLDB3
{
    using System;

    public class PetMemoryUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            string str = BuildMemoryXmlData(_new.memorys);
            string str2 = BuildMemoryXmlData(_old.memorys);
            if (str != str2)
            {
                return (",[memory]=" + UpdateUtility.BuildString(str));
            }
            return string.Empty;
        }

        private static string BuildMemoryXmlData(PetMemory[] _memorys)
        {
            if ((_memorys == null) || (_memorys.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<memorys>";
            foreach (PetMemory memory in _memorys)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<memory target=\"", memory.target, "\" favor=\"", memory.favor, "\" memory=\"", memory.memory, "\" time_stamp=\"", memory.time_stamp, "\"/>" });
            }
            return (str + "</memorys>");
        }
    }
}

