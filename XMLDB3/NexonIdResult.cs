namespace XMLDB3
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot(Namespace="", IsNullable=false)]
    public class NexonIdResult
    {
        [XmlElement("MabiID")]
        public string mabinogiId;
        [XmlElement("Result")]
        public int result;
    }
}

