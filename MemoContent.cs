using System;
using System.Xml.Serialization;

public class MemoContent
{
    public string content;
    [XmlAttribute]
    public string receipantName;
    [XmlAttribute]
    public string receipantServer;
    public MemoCharacter sender;
}

