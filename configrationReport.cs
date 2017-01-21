using System;
using System.Xml.Serialization;

public class configrationReport
{
    [XmlAttribute]
    public string receiver;
    [XmlAttribute]
    public string sender;
    [XmlAttribute]
    public string server;
}

