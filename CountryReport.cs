using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class CountryReport
{
    [XmlAttribute]
    public string reportstring;
}

