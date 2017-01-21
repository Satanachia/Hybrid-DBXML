using System;
using System.Xml.Serialization;

public class AccountrefCharacter
{
    [XmlAttribute]
    public long deleted;
    [XmlAttribute]
    public byte groupID;
    [XmlAttribute]
    public long id;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public byte race;
    [XmlAttribute]
    public string server;
    [XmlAttribute]
    public bool supportCharacter;
    [XmlAttribute]
    public bool tab;
}

