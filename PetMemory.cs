using System;
using System.Xml.Serialization;

public class PetMemory
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

