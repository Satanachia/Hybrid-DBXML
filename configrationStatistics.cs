using System;
using System.Xml.Serialization;

public class configrationStatistics
{
    public connection database;
    [XmlAttribute]
    public int period;
}

