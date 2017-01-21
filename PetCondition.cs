using System;
using System.Xml.Serialization;

public class PetCondition
{
    [XmlAttribute]
    public sbyte flag;
    [XmlAttribute]
    public string meta;
    [XmlAttribute]
    public long time;
    [XmlAttribute]
    public sbyte timemode;
}

