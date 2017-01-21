namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;

    public class KeywordContainer
    {
        [XmlArrayItem("keyword", IsNullable=false)]
        public CharacterKeyword[] keywords;
    }
}

