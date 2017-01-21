using System;
using System.Xml.Serialization;

public class FamilyListFamily
{
    [XmlAttribute]
    public long familyID;
    [XmlAttribute]
    public string familyName;
    [XmlAttribute]
    public long headID;
    [XmlElement("member")]
    public FamilyListFamilyMember[] member;
    [XmlAttribute]
    public string meta;
    [XmlAttribute]
    public ushort state;
    [XmlAttribute]
    public uint tradition;
}

