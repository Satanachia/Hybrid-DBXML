using System;
using System.Xml.Serialization;

public class AccountrefPet
{
    [XmlAttribute]
    public long deleted;
    [XmlAttribute]
    public long expiretime;
    [XmlAttribute]
    public byte groupID;
    [XmlAttribute]
    public long id;
    [XmlAttribute]
    public long lasttime;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public int remaintime;
    [XmlAttribute]
    public string server;
    [XmlAttribute]
    public bool tab;
}

