namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;

    public class PetConditionContainer
    {
        [XmlArrayItem("condition", IsNullable=false)]
        public PetCondition[] conditions;
    }
}

