using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class LogInOutReport
{
    [XmlAttribute]
    public string account;
    [XmlAttribute]
    public string countrycode;
    [XmlAttribute]
    public string inout;
    [XmlAttribute]
    public string ip;
}

