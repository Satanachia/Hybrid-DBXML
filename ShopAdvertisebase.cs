using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ShopAdvertisebase
{
    [XmlAttribute]
    public string account;
    [XmlAttribute]
    public string area;
    [XmlAttribute]
    public string characterName;
    [XmlAttribute]
    public string comment;
    [XmlAttribute]
    public int leafletCount;
    [XmlAttribute]
    public int region;
    [XmlAttribute]
    public string server;
    [XmlAttribute]
    public string shopName;
    [XmlAttribute]
    public long startTime;
    [XmlAttribute]
    public int x;
    [XmlAttribute]
    public int y;
}

