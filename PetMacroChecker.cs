using System;
using System.ComponentModel;
using System.Xml.Serialization;

public class PetMacroChecker
{
    [DefaultValue(0x7d0), XmlAttribute]
    public int macroPoint = 0x7d0;
}

