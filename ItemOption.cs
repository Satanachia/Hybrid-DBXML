using System;
using System.Xml.Serialization;

public class ItemOption
{
    [XmlAttribute]
    public byte enable;
    [XmlAttribute]
    public int enabledata;
    [XmlAttribute]
    public int execdata;
    [XmlAttribute]
    public short execute;
    [XmlAttribute]
    public int flag;
    [XmlAttribute]
    public byte open;
    [XmlAttribute]
    public int opendata;
    [XmlAttribute]
    public byte type;
}

