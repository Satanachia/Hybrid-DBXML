using System;
using System.Xml.Serialization;

public class connection
{
    [XmlAttribute]
    public string database;
    [XmlAttribute]
    public string password;
    [XmlAttribute]
    public string server;
    [XmlAttribute]
    public string user;
}

