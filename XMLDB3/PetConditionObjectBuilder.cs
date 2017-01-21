namespace XMLDB3
{
    using System;
    using System.Data;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class PetConditionObjectBuilder
    {
        private static XmlSerializer serializer = new XmlSerializer(typeof(PetConditionContainer));

        public static PetCondition[] Build(DataRow _pet_row)
        {
            StringReader input = new StringReader("<PetConditionContainer>" + ((string) _pet_row["condition"]) + "</PetConditionContainer>");
            XmlTextReader xmlReader = new XmlTextReader(input);
            PetConditionContainer container = (PetConditionContainer) serializer.Deserialize(xmlReader);
            return container.conditions;
        }
    }
}

