using System;
using System.Xml.Serialization;

public class CharacterTitles
{
    [XmlAttribute]
    public long appliedtime;
    [XmlAttribute]
    public short option;
    [XmlAttribute]
    public short selected;
    [XmlElement("title")]
    public CharacterTitlesTitle[] title;
}

