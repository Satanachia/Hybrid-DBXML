using System;
using System.Xml.Serialization;

public class Ruin
{
    [XmlAttribute]
    public long exploCharID;
    [XmlAttribute]
    public string exploCharName;
    [XmlAttribute]
    public DateTime exploTime;
    [XmlAttribute]
    public int lastTime;
    [XmlAttribute]
    public int position;
    [XmlAttribute]
    public int ruinID;
    [XmlAttribute]
    public int state;
}

