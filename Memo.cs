using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Memo
{
    public string content;
    [XmlElement("receipants")]
    public MemoCharacter[] receipants;
    public MemoCharacter sender;
}

