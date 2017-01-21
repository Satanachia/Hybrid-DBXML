using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class HouseBlockList
{
    [XmlElement("block")]
    public HouseBlock[] block;
}

