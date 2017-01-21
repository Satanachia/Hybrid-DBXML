using System;
using System.Xml.Serialization;

public class PropEvent
{
    [XmlAttribute]
    public bool @default;
    [XmlAttribute]
    public string extra;
    [XmlAttribute]
    public int signal;
    [XmlAttribute]
    public int type;
}

