using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class AccountActivation
{
    [XmlAttribute]
    public byte authority;
    [XmlAttribute]
    public DateTime blocking_date;
    [XmlAttribute]
    public short blocking_duration;
    [XmlAttribute]
    public string email;
    [XmlAttribute]
    public short flag;
    [XmlAttribute]
    public string id;
    [XmlAttribute]
    public string name;
    [XmlAttribute]
    public string password;
    [XmlAttribute]
    public byte provider_code;
    [XmlAttribute]
    public string serialnumber;
}

