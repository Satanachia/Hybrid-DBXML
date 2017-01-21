using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class MemoList
{
    [XmlAttribute]
    public string account;
    [XmlElement("memo")]
    public MemoContent[] memo;
}

