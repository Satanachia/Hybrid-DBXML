using System;
using System.Xml.Serialization;

public class configrationEncryption
{
    [XmlAttribute]
    public uint cacheKeyTimeout;
    [XmlElement("columns")]
    public EncryptionColumn[] columns;
    [XmlAttribute]
    public string dll;
    [XmlAttribute]
    public string userName;
}

