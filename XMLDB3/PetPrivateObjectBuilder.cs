namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class PetPrivateObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(PetPrivate));

        public static PetPrivate Build(DataRow _pet_row)
        {
            StringReader input = new StringReader("<PetPrivate>" + ((string) _pet_row["reserved"]) + ((string) _pet_row["registered"]) + "</PetPrivate>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            return (PetPrivate) serializer.Deserialize(xmlReader);
        }
    }
}

