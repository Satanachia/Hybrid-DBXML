using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ChronicleInfoList
{
    [XmlElement("infos")]
    public ChronicleInfo[] infos;
    [XmlAttribute]
    public string serverName;
}

