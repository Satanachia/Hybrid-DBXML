using System;
using System.Xml.Serialization;

public class AccountSMSAuth
{
    [XmlAttribute]
    public string carrier;
    [XmlAttribute]
    public string cPhone;
    [XmlAttribute]
    public string lastIP;
    [XmlAttribute]
    public byte loginType;
}

