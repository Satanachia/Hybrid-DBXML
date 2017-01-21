using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class FamilyList
{
    [XmlElement("family")]
    public FamilyListFamily[] family;
}

