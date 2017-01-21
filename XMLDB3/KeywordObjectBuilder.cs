namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class KeywordObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(KeywordContainer));

        public static CharacterKeyword[] Build(DataRow _character_row)
        {
            StringReader input = new StringReader("<KeywordContainer>" + ((string) _character_row["keyword"]) + "</KeywordContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            KeywordContainer container = (KeywordContainer) serializer.Deserialize(xmlReader);
            return container.keywords;
        }
    }
}

