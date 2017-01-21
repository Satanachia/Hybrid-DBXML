using System;
using System.Xml.Serialization;

public class configrationRedirection
{
    [XmlAttribute]
    public bool enable;
    [XmlAttribute]
    public int port;
    [XmlAttribute]
    public string server;
}

