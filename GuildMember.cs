using System;
using System.Xml.Serialization;

public class GuildMember
{
    [XmlAttribute]
    public string account;
    [XmlAttribute]
    public int @class;
    [XmlAttribute]
    public long memberid;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public int point;
}

