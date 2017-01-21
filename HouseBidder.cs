using System;
using System.Xml.Serialization;

public class HouseBidder
{
    [XmlAttribute]
    public string bidAccount;
    [XmlAttribute]
    public long bidCharacter;
    [XmlAttribute]
    public string bidCharName;
    [XmlAttribute]
    public int bidOrder;
    [XmlAttribute]
    public int bidPrice;
    [XmlAttribute]
    public DateTime bidTime;
    [XmlAttribute]
    public DateTime bidUpdateTime;
}

