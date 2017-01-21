using System;
using System.Xml.Serialization;

public class CharacterHeartSticker
{
    [XmlAttribute]
    public short heartPoint;
    [XmlAttribute]
    public short heartTotalPoint;
    [XmlAttribute]
    public long heartUpdateTime;
}

