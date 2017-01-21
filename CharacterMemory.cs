using System;
using System.Xml.Serialization;

public class CharacterMemory
{
    [XmlAttribute]
    public byte favor;
    [XmlAttribute]
    public byte memory;
    [XmlAttribute]
    public string target;
    [XmlAttribute]
    public byte time_stamp;
}

