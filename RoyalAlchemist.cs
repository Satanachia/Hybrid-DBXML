using System;
using System.Xml.Schema;
using System.Xml.Serialization;

public class RoyalAlchemist
{
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public long charID;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string charName;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string meta;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public ushort rank;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public byte registrationFlag;
}

