using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Account
{
    [XmlAttribute]
    public byte authority;
    [XmlAttribute]
    public DateTime blocking_date;
    [XmlAttribute]
    public short blocking_duration;
    [XmlAttribute]
    public bool changePassword;
    [XmlAttribute]
    public string email;
    [XmlAttribute]
    public string eserialnumber;
    [XmlAttribute]
    public short flag;
    [XmlAttribute]
    public string id;
    [XmlAttribute]
    public string machineids;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string password;
    [XmlAttribute]
    public string serialnumber;
    public AccountSMSAuth SMSAuth;
}

