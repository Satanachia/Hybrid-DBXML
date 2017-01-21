using System;
using System.Xml.Serialization;

[XmlRoot("aaa", Namespace="", IsNullable=false)]
public class Itembase
{
    [XmlAttribute]
    public short attack_max;
    [XmlAttribute]
    public short attack_min;
    [XmlAttribute]
    public byte attack_speed;
    [XmlAttribute]
    public byte balance;
    [XmlAttribute]
    public short bundle;
    [XmlAttribute]
    public int @class;
    [XmlAttribute]
    public int color_01;
    [XmlAttribute]
    public int color_02;
    [XmlAttribute]
    public int color_03;
    [XmlAttribute]
    public byte critical;
    [XmlAttribute]
    public string data;
    [XmlAttribute]
    public int defence;
    [XmlAttribute]
    public byte down_hit_count;
    [XmlAttribute]
    public int durability;
    [XmlAttribute]
    public int durability_max;
    [XmlAttribute]
    public short effective_range;
    public Ego ego;
    [XmlAttribute]
    public byte exp_point;
    [XmlAttribute]
    public short experience;
    [XmlAttribute]
    public int expiration;
    [XmlAttribute]
    public int figure;
    [XmlAttribute]
    public byte flag;
    [XmlAttribute]
    public byte grade;
    [XmlAttribute]
    public long id;
    [XmlAttribute]
    public byte linked_pocket;
    [XmlElement("options")]
    public ItemOption[] options;
    [XmlAttribute]
    public int origin_durability_max;
    [XmlAttribute]
    public byte pocket;
    [XmlAttribute]
    public int pos_x;
    [XmlAttribute]
    public int pos_y;
    [XmlAttribute]
    public short prefix;
    [XmlAttribute]
    public int price;
    [XmlAttribute]
    public short protect;
    public Quest quest;
    [XmlAttribute]
    public int sellingprice;
    [XmlAttribute]
    public byte storedtype;
    [XmlAttribute]
    public short suffix;
    [XmlAttribute]
    public byte upgrade_max;
    [XmlAttribute]
    public byte upgraded;
    [XmlAttribute]
    public int varint;
    [XmlAttribute]
    public short wattack_max;
    [XmlAttribute]
    public short wattack_min;
}

