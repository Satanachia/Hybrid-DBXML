namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class MemoryObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(MemoryContainer));

        public static CharacterMemory[] Build(DataRow _character_row)
        {
            StringReader input = new StringReader("<MemoryContainer>" + ((string) _character_row["memory"]) + "</MemoryContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            MemoryContainer container = (MemoryContainer) serializer.Deserialize(xmlReader);
            return container.memorys;
        }
    }
}

