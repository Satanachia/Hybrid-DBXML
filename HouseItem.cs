using System;
using System.Xml.Serialization;

public class HouseItem
{
    [XmlAttribute]
    public byte direction;
    public Item item;
    [XmlAttribute]
    public byte pocket;
    [XmlAttribute]
    public byte posX;
    [XmlAttribute]
    public byte posY;
    [XmlAttribute]
    public int userprice;
}

