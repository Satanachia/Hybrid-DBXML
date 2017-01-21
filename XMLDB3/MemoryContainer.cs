namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;

    public class MemoryContainer
    {
        [XmlArrayItem("memory", IsNullable=false)]
        public CharacterMemory[] memorys;
    }
}

