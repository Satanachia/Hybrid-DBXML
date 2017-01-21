using System;
using System.Xml.Serialization;

public class CharacterService
{
    [XmlAttribute]
    public int apgiftreceiveday;
    [XmlAttribute]
    public byte nsbombcount;
    [XmlAttribute]
    public int nsbombday;
    [XmlAttribute]
    public int nsgiftreceiveday;
    [XmlAttribute]
    public int nslastrespawnday;
    [XmlAttribute]
    public byte nsrespawncount;
}

