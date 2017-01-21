namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;

    public class PetMemoryContainer
    {
        [XmlArrayItem("memory", IsNullable=false)]
        public PetMemory[] memorys;
    }
}

