using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class DungeonRankList
{
    [XmlElement("dungeonRanks")]
    public DungeonRank[] dungeonRanks;
}

