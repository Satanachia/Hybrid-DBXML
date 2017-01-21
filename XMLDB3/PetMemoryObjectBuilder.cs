namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class PetMemoryObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(PetMemoryContainer));

        public static PetMemory[] Build(DataRow _pet_row)
        {
            StringReader input = new StringReader("<PetMemoryContainer>" + ((string) _pet_row["memory"]) + "</PetMemoryContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            PetMemoryContainer container = (PetMemoryContainer) serializer.Deserialize(xmlReader);
            return container.memorys;
        }
    }
}

