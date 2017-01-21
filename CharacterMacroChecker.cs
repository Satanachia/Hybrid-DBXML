using System;
using System.ComponentModel;
using System.Xml.Serialization;

public class CharacterMacroChecker
{
    [XmlAttribute, DefaultValue(0x7d0)]
    public int macroPoint = 0x7d0;
}

