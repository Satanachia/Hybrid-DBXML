using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class PropIDList
{
    [XmlElement("propID")]
    public long[] propID;
}

