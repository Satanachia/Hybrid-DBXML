using System;
using System.Xml.Serialization;

public class DungeonRank
{
    [XmlAttribute]
    public long characterID;
    [XmlAttribute]
    public string characterName;
    [XmlAttribute]
    public string dungeonName;
    [XmlAttribute]
    public int laptime;
    [XmlAttribute]
    public byte race;
    [XmlAttribute]
    public int score;
    [XmlAttribute]
    public string server;
}

