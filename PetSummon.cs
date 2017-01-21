using System;
using System.Xml.Serialization;

public class PetSummon
{
    [XmlAttribute]
    public long expiretime;
    [XmlAttribute]
    public byte favor;
    [XmlAttribute]
    public long lasttime;
    [XmlAttribute]
    public byte loyalty;
    [XmlAttribute]
    public int remaintime;
}

