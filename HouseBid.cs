using System;
using System.Xml.Serialization;

public class HouseBid
{
    [XmlAttribute]
    public DateTime bidEndTime;
    [XmlAttribute]
    public DateTime bidRepayEndTime;
    [XmlAttribute]
    public DateTime bidStartTime;
    [XmlAttribute]
    public int minBidPrice;
}

