using System;
using System.Xml.Serialization;

public class CharacterArbeit
{
    [XmlArrayItem("Info", IsNullable=false)]
    public CharacterArbeitInfo[] collection;
    [XmlArrayItem("day", IsNullable=false)]
    public CharacterArbeitDay[] history;
}

