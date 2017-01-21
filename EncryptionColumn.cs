using System;
using System.Xml.Serialization;

public class EncryptionColumn
{
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public ulong passCode;
}

