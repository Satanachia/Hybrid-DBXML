using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class configration
{
    public configrationCache cache;
    public configrationChronicleRank chronicleRank;
    public configrationEncryption encryption;
    public configrationFeature feature;
    public configrationFile file;
    public configrationItemMarket itemMarket;
    public configrationRedirection redirection;
    public configrationReport report;
    public configrationServer server;
    public configrationSql sql;
    public configrationStatistics statistics;
}

