using System;
using System.Xml.Serialization;

public class CastleBid
{
    [XmlAttribute]
    public DateTime bidEndTime;
    [XmlAttribute]
    public DateTime bidStartTime;
    [XmlAttribute]
    public long castleID;
    [XmlAttribute]
    public int minBidPrice;
}

