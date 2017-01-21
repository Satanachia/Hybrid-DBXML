using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ChronicleList
{
    [XmlElement("chronicles")]
    public Chronicle[] chronicles;
}

