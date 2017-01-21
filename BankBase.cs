using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class BankBase
{
    [XmlAttribute]
    public string account;
    public BankData data;
}

