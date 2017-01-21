using System;
using System.Xml.Serialization;

public class GuildRobe
{
    [XmlAttribute]
    public int color1;
    [XmlAttribute]
    public byte color2Index;
    [XmlAttribute]
    public byte color3Index;
    [XmlAttribute]
    public byte color4Index;
    [XmlAttribute]
    public byte color5Index;
    [XmlAttribute]
    public byte emblemBeltDeco;
    [XmlAttribute]
    public byte emblemChestDeco;
    [XmlAttribute]
    public byte emblemChestIcon;
}

