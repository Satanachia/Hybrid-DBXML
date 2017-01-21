using System;
using System.Xml.Serialization;

public class PetData
{
    [XmlAttribute]
    public DateTime birthday;
    [XmlAttribute]
    public string meta;
    [XmlAttribute]
    public int playtime;
    [XmlAttribute]
    public short rebirthage;
    [XmlAttribute]
    public DateTime rebirthday;
    [XmlAttribute]
    public string ui;
    [XmlAttribute]
    public int wealth;
    [XmlAttribute]
    public byte writeCounter;
}

