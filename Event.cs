using System;
using System.Xml.Schema;
using System.Xml.Serialization;

public class Event
{
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string account;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string charName;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public byte eventType;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string serverName;
}

