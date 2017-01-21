using System;
using System.Xml.Serialization;

public class PromotionRecord
{
    [XmlAttribute]
    public string channel;
    [XmlAttribute]
    public ulong characterID;
    [XmlAttribute]
    public string characterName;
    [XmlAttribute]
    public ushort level;
    [XmlAttribute]
    public uint point;
    [XmlAttribute]
    public byte race;
    [XmlAttribute]
    public string serverName;
    [XmlAttribute]
    public string skillCategory;
    [XmlAttribute]
    public ushort skillid;
    [XmlAttribute]
    public string skillName;
}

