namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class ArbeitObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(CharacterArbeit));

        public static CharacterArbeit Build(DataRow _character_row)
        {
            StringReader input = new StringReader("<CharacterArbeit>" + ((string) _character_row["history"]) + ((string) _character_row["collection"]) + "</CharacterArbeit>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            return (CharacterArbeit) serializer.Deserialize(xmlReader);
        }
    }
}

