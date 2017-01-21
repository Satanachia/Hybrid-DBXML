using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class HouseInventory
{
    [XmlAttribute]
    public string account;
    [XmlElement("item")]
    public HouseItem[] item;
}

