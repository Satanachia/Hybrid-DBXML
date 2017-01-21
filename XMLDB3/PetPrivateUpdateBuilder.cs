namespace XMLDB3
{
    using System;

    public class PetPrivateUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if ((_new.@private == null) || (_old.@private == null))
            {
                return string.Empty;
            }
            string str = BuildReservedXmlData(_new.@private.reserveds);
            string str2 = BuildReservedXmlData(_old.@private.reserveds);
            string str3 = BuildRegisteredXmlData(_new.@private.registereds);
            string str4 = BuildRegisteredXmlData(_old.@private.registereds);
            string str5 = string.Empty;
            if (str != str2)
            {
                str5 = str5 + ",[reserved]=" + UpdateUtility.BuildString(str);
            }
            if (str3 != str4)
            {
                str5 = str5 + ",[registered]=" + UpdateUtility.BuildString(str3);
            }
            return str5;
        }

        private static string BuildRegisteredXmlData(PetPrivateRegistered[] _registereds)
        {
            if ((_registereds == null) || (_registereds.Length <= 0))
            {
                return "<registereds/>";
            }
            string str = "<registereds>";
            foreach (PetPrivateRegistered registered in _registereds)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<registered id=\"", registered.id, "\" start=\"", registered.start, "\" end=\"", registered.end, "\" extra=\"", registered.extra, "\" />" });
            }
            return (str + "</registereds>");
        }

        private static string BuildReservedXmlData(PetPrivateReserved[] _reserveds)
        {
            if ((_reserveds == null) || (_reserveds.Length <= 0))
            {
                return string.Empty;
            }
            string str = "<reserveds>";
            foreach (PetPrivateReserved reserved in _reserveds)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "<reserved id=\"", reserved.id, "\" rate=\"", reserved.rate, "\"/>" });
            }
            return (str + "</reserveds>");
        }
    }
}

