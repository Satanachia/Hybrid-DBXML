using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class GroupIDList
{
    [XmlElement("group")]
    public GroupID[] group;
}

