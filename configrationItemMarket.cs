using System;
using System.Xml.Serialization;

public class configrationItemMarket
{
    [XmlAttribute]
    public int codePage;
    [XmlAttribute]
    public int connectionPool;
    [XmlAttribute]
    public int gameNo;
    [XmlAttribute]
    public string ip;
    [XmlAttribute]
    public short port;
    [XmlAttribute]
    public int serverNo;
}

