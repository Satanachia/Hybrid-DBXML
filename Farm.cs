using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Farm
{
    [XmlAttribute]
    public byte crop;
    [XmlAttribute]
    public byte currentWork;
    [XmlAttribute]
    public long expireTime;
    [XmlAttribute]
    public long farmID;
    [XmlAttribute]
    public short growth;
    [XmlAttribute]
    public short insect;
    [XmlAttribute]
    public short insectWork;
    [XmlAttribute]
    public long lastWorkTime;
    [XmlAttribute]
    public short nutrient;
    [XmlAttribute]
    public short nutrientWork;
    [XmlAttribute]
    public string ownerAccount;
    [XmlAttribute]
    public long ownerCharID;
    [XmlAttribute]
    public string ownerCharName;
    [XmlAttribute]
    public long plantTime;
    [XmlAttribute]
    public byte todayWorkCount;
    [XmlAttribute]
    public short water;
    [XmlAttribute]
    public short waterWork;
    [XmlAttribute]
    public long workCompleteTime;
}

