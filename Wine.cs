using System;
using System.Xml.Serialization;

[XmlType(Namespace="http://tempuri.org/wine.xsd"), XmlRoot(Namespace="http://tempuri.org/wine.xsd", IsNullable=false)]
public class Wine
{
    [XmlAttribute]
    public int acidity;
    [XmlAttribute]
    public short agingCount;
    [XmlAttribute]
    public DateTime agingStartTime;
    [XmlAttribute]
    public long charID;
    [XmlAttribute]
    public int freshness;
    [XmlAttribute]
    public DateTime lastRackingTime;
    [XmlAttribute]
    public int purity;
    [XmlAttribute]
    public byte wineType;
}

