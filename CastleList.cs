using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class CastleList
{
    [XmlElement("bidders")]
    public CastleBidder[] bidders;
    [XmlElement("bids")]
    public CastleBid[] bids;
    [XmlElement("blocks")]
    public CastleBlockList[] blocks;
    [XmlElement("castles")]
    public Castle[] castles;
}

