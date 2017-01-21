namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class ConditionObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(ConditionContainer));

        public static CharacterCondition[] Build(DataRow _character_row)
        {
            StringReader input = new StringReader("<ConditionContainer>" + ((string) _character_row["condition"]) + "</ConditionContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            ConditionContainer container = (ConditionContainer) serializer.Deserialize(xmlReader);
            return container.conditions;
        }
    }
}

