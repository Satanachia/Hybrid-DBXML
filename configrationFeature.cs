using System;
using System.Xml.Serialization;

public class configrationFeature
{
    [XmlAttribute]
    public bool pvp;

    //Add custom hybridMode flag for reading from config file. NOT the intended purpose of the features config but its w/e
    [XmlAttribute]
    public bool hybridMode;
    // Similar idea here. Ability to toggle the character adapter workaround when running acocuntref in SQL mode but not character. 
    [XmlAttribute]
    public bool characterFileAdapterFix;
}

