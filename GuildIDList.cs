using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class GuildIDList
{
    [XmlElement("guildID")]
    public long[] guildID;
}

