using System;
using System.Xml.Serialization;

public class GuildStone
{
    [XmlAttribute]
    public float direction;
    [XmlAttribute]
    public long position_id;
    [XmlAttribute]
    public short region;
    [XmlAttribute]
    public string server;
    [XmlAttribute]
    public int type;
    [XmlAttribute]
    public int x;
    [XmlAttribute]
    public int y;
}

