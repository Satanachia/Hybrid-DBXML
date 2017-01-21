using System;
using System.Xml.Serialization;

public class LobbyTab
{
    [XmlAttribute]
    public long charID;
    [XmlAttribute]
    public string server;
    [XmlAttribute]
    public bool tab;
}

