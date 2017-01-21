using System;
using System.Xml.Serialization;

public class CharacterDonation
{
    [XmlAttribute]
    public long donationUpdate;
    [XmlAttribute]
    public int donationValue;
}

