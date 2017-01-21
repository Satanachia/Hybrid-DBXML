using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class ChannelingKey
{
    [XmlAttribute]
    public string keystring;
    [XmlAttribute]
    public byte provider;
}

