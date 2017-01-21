using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class BidList
{
    [XmlElement("bids")]
    public Bid[] bids;
}

