using System;
using System.Xml.Serialization;

public class BankItem
{
    [XmlAttribute]
    public long extraTime;
    public Item item;
    [XmlAttribute]
    public string location;
    [XmlAttribute]
    public long time;
}

