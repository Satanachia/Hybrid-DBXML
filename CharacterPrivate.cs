using System;
using System.Xml.Serialization;

public class CharacterPrivate
{
    [XmlArrayItem("book", IsNullable=false)]
    public CharacterPrivateBook[] books;
    [XmlAttribute]
    public long npc_event_bitflag;
    [XmlAttribute]
    public int npc_event_daycount;
    [XmlArrayItem("registered", IsNullable=false)]
    public CharacterPrivateRegistered[] registereds;
    [XmlArrayItem("reserved", IsNullable=false)]
    public CharacterPrivateReserved[] reserveds;
}

