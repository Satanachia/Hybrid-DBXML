using System;
using System.Xml.Serialization;

public class QuestObjective
{
    [XmlAttribute]
    public byte active;
    [XmlAttribute]
    public byte complete;
    [XmlAttribute]
    public string data;
}

