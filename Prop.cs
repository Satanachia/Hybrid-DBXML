using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Prop
{
    [XmlAttribute]
    public int classid;
    [XmlAttribute]
    public int color1;
    [XmlAttribute]
    public int color2;
    [XmlAttribute]
    public int color3;
    [XmlAttribute]
    public int color4;
    [XmlAttribute]
    public int color5;
    [XmlAttribute]
    public int color6;
    [XmlAttribute]
    public int color7;
    [XmlAttribute]
    public int color8;
    [XmlAttribute]
    public int color9;
    [XmlAttribute]
    public float direction;
    [XmlAttribute]
    public long entertime;
    [XmlElement("event")]
    public PropEvent[] @event;
    [XmlAttribute]
    public string extra;
    [XmlAttribute]
    public long id;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public int region;
    [XmlAttribute]
    public float scale;
    [XmlAttribute]
    public string state;
    [XmlAttribute]
    public int x;
    [XmlAttribute]
    public int y;
    [XmlAttribute]
    public int z;
}

