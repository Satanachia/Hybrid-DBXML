namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class TitleObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(TitleContainer));

        public static CharacterTitles Build(DataRow _character_row)
        {
            StringReader input = new StringReader("<TitleContainer>" + ((string) _character_row["title"]) + "</TitleContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            TitleContainer container = (TitleContainer) serializer.Deserialize(xmlReader);
            return container.titles;
        }

        public class TitleContainer
        {
            public CharacterTitles titles;
        }
    }
}

