using System;
using System.Xml.Schema;
using System.Xml.Serialization;

public class WorldMeta
{
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string key;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public byte type;
    [XmlAttribute(Form=XmlSchemaForm.Qualified)]
    public string value;
}

