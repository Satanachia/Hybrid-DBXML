using System;
using System.Xml.Serialization;

public class PetSkill
{
    [XmlAttribute]
    public short flag;
    [XmlAttribute]
    public short id;
    [XmlAttribute]
    public byte level;
}

