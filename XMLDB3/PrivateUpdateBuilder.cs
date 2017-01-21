namespace XMLDB3
{
    using System;

    public class PrivateUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.@private == null) || (_old.@private == null))
            {
                return string.Empty;
            }
            string str = BuildReservedXmlData(_new.@private.reserveds);
            string str2 = BuildReservedXmlData(_old.@private.reserveds);
            string str3 = BuildBookXmlData(_new.@private.books);
            string str4 = BuildBookXmlData(_old.@private.books);
            string str5 = string.Empty;
            if (str != str2)
            {
                str5 = str5 + ",[reserved]=" + UpdateUtility.BuildString(str);
            }
            if (str3 != str4)
            {
                str5 = str5 + ",[book]=" + UpdateUtility.BuildString(str3);
            }
            if (_new.@private.npc_event_daycount != _old.@private.npc_event_daycount)
            {
                str5 = str5 + ",[npc_event_daycount]=" + _new.@private.npc_event_daycount;
            }
            if (_new.@private.npc_event_bitflag != _old.@private.npc_event_bitflag)
            {
                str5 = str5 + ",[npc_event_bitflag]=" + _new.@private.npc_event_bitflag;
            }
            return str5;
        }

        private static string BuildBookXmlData(CharacterPrivateBook[] _books)
        {
            if ((_books == null) || (_books.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<books>";
            foreach (CharacterPrivateBook book in _books)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<book id=\"", book.id, "\" />" });
            }
            return (str + "</books>");
        }

        private static string BuildRegisteredXmlData(CharacterPrivateRegistered[] _registereds)
        {
            if ((_registereds == null) || (_registereds.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<registereds>";
            foreach (CharacterPrivateRegistered registered in _registereds)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<registered id=\"", registered.id, "\" start=\"", registered.start, "\" end=\"", registered.end, "\" extra=\"", registered.extra, "\" />" });
            }
            return (str + "</registereds>");
        }

        private static string BuildReservedXmlData(CharacterPrivateReserved[] _reserveds)
        {
            if ((_reserveds == null) || (_reserveds.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<reserveds>";
            foreach (CharacterPrivateReserved reserved in _reserveds)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<reserved id=\"", reserved.id, "\" rate=\"", reserved.rate, "\"/>" });
            }
            return (str + "</reserveds>");
        }
    }
}

