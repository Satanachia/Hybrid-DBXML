using System;
using System.Xml.Serialization;

public class PetPrivate
{
    [XmlArrayItem("registered", IsNullable=false)]
    public PetPrivateRegistered[] registereds;
    [XmlArrayItem("reserved", IsNullable=false)]
    public PetPrivateReserved[] reserveds;
}

