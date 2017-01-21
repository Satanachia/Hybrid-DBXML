using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class RuinList
{
    [XmlElement("ruins")]
    public Ruin[] ruins;
}

