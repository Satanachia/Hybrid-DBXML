using System;
using System.Xml.Serialization;

public class Ego
{
    [XmlAttribute]
    public byte egoDesire;
    [XmlAttribute]
    public int egoDexExp;
    [XmlAttribute]
    public byte egoDexLevel;
    [XmlAttribute]
    public int egoIntExp;
    [XmlAttribute]
    public byte egoIntLevel;
    [XmlAttribute]
    public int egoLuckExp;
    [XmlAttribute]
    public byte egoLuckLevel;
    [XmlAttribute]
    public string egoName;
    [XmlAttribute]
    public long egoSkillCoolTime;
    [XmlAttribute]
    public byte egoSkillCount;
    [XmlAttribute]
    public int egoSkillGauge;
    [XmlAttribute]
    public int egoSocialExp;
    [XmlAttribute]
    public byte egoSocialLevel;
    [XmlAttribute]
    public int egoStrExp;
    [XmlAttribute]
    public byte egoStrLevel;
    [XmlAttribute]
    public byte egoType;
    [XmlAttribute]
    public int egoWillExp;
    [XmlAttribute]
    public byte egoWillLevel;
}

