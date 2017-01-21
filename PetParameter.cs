using System;
using System.Xml.Serialization;

public class PetParameter
{
    [XmlAttribute]
    public short age;
    [XmlAttribute]
    public short attack_max;
    [XmlAttribute]
    public short attack_min;
    [XmlAttribute]
    public float critical;
    [XmlAttribute]
    public int cumulatedlevel;
    [XmlAttribute]
    public short defense;
    [XmlAttribute]
    public float dexterity;
    [XmlAttribute]
    public long experience;
    [XmlAttribute]
    public float food;
    [XmlAttribute]
    public float intelligence;
    [XmlAttribute]
    public short level;
    [XmlAttribute]
    public float life;
    [XmlAttribute]
    public float life_damage;
    [XmlAttribute]
    public float life_max;
    [XmlAttribute]
    public float luck;
    [XmlAttribute]
    public float mana;
    [XmlAttribute]
    public float mana_max;
    [XmlAttribute]
    public short maxlevel;
    [XmlAttribute]
    public float protect;
    [XmlAttribute]
    public short rate;
    [XmlAttribute]
    public short rebirthcount;
    [XmlAttribute]
    public float stamina;
    [XmlAttribute]
    public float stamina_max;
    [XmlAttribute]
    public float strength;
    [XmlAttribute]
    public short wattack_max;
    [XmlAttribute]
    public short wattack_min;
    [XmlAttribute]
    public float will;
}

