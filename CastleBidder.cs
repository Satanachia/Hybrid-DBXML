using System;
using System.Xml.Serialization;

public class CastleBidder
{
    [XmlAttribute]
    public long bidCharacter;
    [XmlAttribute]
    public string bidCharName;
    [XmlAttribute]
    public long bidGuildID;
    [XmlAttribute]
    public string bidGuildName;
    [XmlAttribute]
    public int bidOrder;
    [XmlAttribute]
    public int bidPrice;
    [XmlAttribute]
    public DateTime bidTime;
    [XmlAttribute]
    public DateTime bidUpdateTime;
    [XmlAttribute]
    public long castleID;
}

