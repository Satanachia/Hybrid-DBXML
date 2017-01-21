using System;
using System.Xml.Serialization;

public class CastleBuild
{
    [XmlAttribute]
    public DateTime buildNextTime;
    [XmlAttribute]
    public byte buildState;
    [XmlAttribute]
    public byte buildStep;
    [XmlAttribute]
    public int durability;
    [XmlAttribute]
    public int maxDurability;
    [XmlElement("resource")]
    public CastleBuildResource[] resource;
}

