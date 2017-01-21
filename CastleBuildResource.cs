using System;
using System.Xml.Serialization;

public class CastleBuildResource
{
    [XmlAttribute]
    public int classID;
    [XmlAttribute]
    public int curAmount;
    [XmlAttribute]
    public int maxAmount;
}

