using System;
using System.Xml.Serialization;

public class CastleBlockList
{
    [XmlElement("block")]
    public CastleBlock[] block;
    [XmlAttribute]
    public long castleID;
}

