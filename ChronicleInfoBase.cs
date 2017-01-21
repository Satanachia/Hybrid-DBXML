using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ChronicleInfoBase
{
    [XmlAttribute]
    public string group;
    [XmlAttribute]
    public short height;
    [XmlAttribute]
    public string keyword;
    [XmlAttribute]
    public string localtext;
    [XmlAttribute]
    public int questID;
    [XmlAttribute]
    public string questName;
    [XmlAttribute]
    public string sort;
    [XmlAttribute]
    public string source;
    [XmlAttribute]
    public short width;
}

