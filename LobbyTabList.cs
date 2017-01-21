using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class LobbyTabList
{
    [XmlElement("tabInfo")]
    public LobbyTab[] tabInfo;
}

