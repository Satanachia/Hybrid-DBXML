using System;
using System.Xml.Serialization;

public class CharacterMarriage
{
    [XmlAttribute]
    public short marriagecount;
    [XmlAttribute]
    public int marriagetime;
    [XmlAttribute]
    public long mateid;
    [XmlAttribute]
    public string matename;
}

