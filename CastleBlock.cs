using System;
using System.Xml.Serialization;

public class CastleBlock
{
    [XmlAttribute]
    public byte entry;
    [XmlAttribute]
    public byte flag;
    [XmlAttribute]
    public string gameName;
}

