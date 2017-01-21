using System;
using System.Xml.Serialization;

public class CharacterArbeitDay
{
    [XmlAttribute]
    public int daycount;
    [XmlElement("info")]
    public CharacterArbeitDayInfo[] info;
}

