using System;
using System.Xml.Serialization;

public class FamilyListFamilyMember
{
    [XmlAttribute]
    public ushort memberClass;
    [XmlAttribute]
    public long memberID;
    [XmlAttribute]
    public string memberName;
}

