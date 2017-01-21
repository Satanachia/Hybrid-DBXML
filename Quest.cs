using System;
using System.Xml.Serialization;

public class Quest
{
    [XmlAttribute]
    public byte complete;
    [XmlAttribute]
    public string data;
    [XmlAttribute]
    public long id;
    [XmlArrayItem("objective", IsNullable=false)]
    public QuestObjective[] objectives;
    [XmlAttribute]
    public long start_time;
    [XmlAttribute]
    public int templateid;
}

