using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class MailBox
{
    [XmlElement("mailItem")]
    public MailItem[] mailItem;
}

