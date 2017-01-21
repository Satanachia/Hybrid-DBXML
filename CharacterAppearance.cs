using System;
using System.Xml.Serialization;

public class CharacterAppearance
{
    [XmlAttribute]
    public int battle_state;
    [XmlAttribute]
    public byte direction;
    [XmlAttribute]
    public byte eye_color;
    [XmlAttribute]
    public byte eye_type;
    [XmlAttribute]
    public float fatness;
    [XmlAttribute]
    public float height;
    [XmlAttribute]
    public float lower;
    [XmlAttribute]
    public byte mouth_type;
    [XmlAttribute]
    public int region;
    [XmlAttribute]
    public byte skin_color;
    [XmlAttribute]
    public int status;
    [XmlAttribute]
    public int type;
    [XmlAttribute]
    public float upper;
    [XmlAttribute]
    public byte weapon_set;
    [XmlAttribute]
    public int x;
    [XmlAttribute]
    public int y;
}

