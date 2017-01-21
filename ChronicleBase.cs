using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ChronicleBase
{
    [XmlAttribute]
    public long charID;
    [XmlAttribute]
    public DateTime createTime;
    [XmlAttribute]
    public string meta;
    [XmlAttribute]
    public int questID;
    [XmlAttribute]
    public string serverName;
}

