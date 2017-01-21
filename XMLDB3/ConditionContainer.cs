namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;

    public class ConditionContainer
    {
        [XmlArrayItem("condition", IsNullable=false)]
        public CharacterCondition[] conditions;
    }
}

