using System;
using System.Xml.Serialization;

public class Castle
{
    public CastleBuild build;
    [XmlAttribute]
    public long castleID;
    [XmlAttribute]
    public int castleMoney;
    [XmlAttribute]
    public byte constructed;
    [XmlAttribute]
    public int dungeonPassPrice;
    [XmlAttribute]
    public long flag;
    [XmlAttribute]
    public long guildID;
    [XmlAttribute]
    public byte sellDungeonPass;
    [XmlAttribute]
    public byte taxrate;
    [XmlAttribute]
    public DateTime updateTime;
    [XmlAttribute]
    public int weeklyIncome;
}

