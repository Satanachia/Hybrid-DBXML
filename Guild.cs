using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Guild
{
    [XmlAttribute]
    public byte battlegroundtype;
    [XmlAttribute]
    public byte battlegroundwinnertype;
    [XmlAttribute]
    public DateTime drawabledate;
    [XmlAttribute]
    public int drawablemoney;
    [XmlAttribute]
    public byte enable;
    [XmlAttribute]
    public DateTime expiration;
    [XmlAttribute]
    public string greeting;
    [XmlAttribute]
    public int guildmoney;
    [XmlAttribute]
    public int guildpoint;
    [XmlAttribute]
    public byte guildstatusflag;
    [XmlAttribute]
    public string guildtitle;
    [XmlAttribute]
    public int guildtype;
    [XmlAttribute]
    public long id;
    [XmlAttribute]
    public int jointype;
    [XmlAttribute]
    public string leaving;
    [XmlAttribute]
    public int maxmember;
    [XmlElement("member")]
    public GuildMember[] member;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string profile;
    [XmlAttribute]
    public string refuse;
    public GuildRobe robe;
    [XmlAttribute]
    public string server;
    public GuildStone stone;
}

