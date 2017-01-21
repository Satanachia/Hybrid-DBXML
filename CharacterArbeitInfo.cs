using System;
using System.Xml.Serialization;

public class CharacterArbeitInfo
{
    [XmlAttribute]
    public short category;
    [XmlAttribute]
    public short success;
    [XmlAttribute]
    public short total;
}

