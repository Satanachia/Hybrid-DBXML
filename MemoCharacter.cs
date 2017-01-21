using System;
using System.Xml.Serialization;

public class MemoCharacter
{
    [XmlAttribute]
    public string account;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string server;
}

