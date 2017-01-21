using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ShopAdvertiseItembase
{
    [XmlAttribute]
    public int @class;
    [XmlAttribute]
    public int color_01;
    [XmlAttribute]
    public int color_02;
    [XmlAttribute]
    public int color_03;
    [XmlAttribute]
    public long id;
    [XmlAttribute]
    public string itemName;
    [XmlAttribute]
    public int price;
    [XmlAttribute]
    public byte storedtype;
}

