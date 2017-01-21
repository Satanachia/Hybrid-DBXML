using System;
using System.Xml.Serialization;

public class CharacterCondition
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

