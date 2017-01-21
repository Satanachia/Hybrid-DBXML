using System;
using System.Xml.Serialization;

public class CharacterPVP
{
    [XmlAttribute]
    public long loseCnt;
    [XmlAttribute]
    public int penaltyPoint;
    [XmlAttribute]
    public long winCnt;
}

