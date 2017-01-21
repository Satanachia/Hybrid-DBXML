using System;
using System.Xml.Serialization;

public class CharacterData
{
    [XmlAttribute]
    public DateTime birthday;
    [XmlAttribute]
    public string meta;
    [XmlAttribute]
    public short nao_favor;
    [XmlAttribute]
    public short nao_memory;
    [XmlAttribute]
    public byte nao_style;
    [XmlAttribute]
    public int playtime;
    [XmlAttribute]
    public short rebirthage;
    [XmlAttribute]
    public DateTime rebirthday;
    [XmlAttribute]
    public int wealth;
    [XmlAttribute]
    public byte writeCounter;
}

