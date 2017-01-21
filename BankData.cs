using System;
using System.Xml.Serialization;

public class BankData
{
    [XmlAttribute]
    public int deposit;
    [XmlAttribute]
    public string password;
    [XmlAttribute]
    public DateTime updatetime;
}

