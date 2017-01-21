using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class RoyalAlchemistList
{
    [XmlElement("alchemists")]
    public RoyalAlchemist[] alchemists;
}

