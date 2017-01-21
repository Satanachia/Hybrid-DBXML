using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class EventList
{
    [XmlElement("events")]
    public Event[] events;
}

