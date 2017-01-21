using System;
using System.Xml.Serialization;

public class CharacterJoust
{
    [XmlAttribute]
    public short joustDailyLoseCount;
    [XmlAttribute]
    public short joustDailyWinCount;
    [XmlAttribute]
    public byte joustLastWinWeek;
    [XmlAttribute]
    public byte joustLastWinYear;
    [XmlAttribute]
    public int joustPoint;
    [XmlAttribute]
    public short joustServerLoseCount;
    [XmlAttribute]
    public short joustServerWinCount;
    [XmlAttribute]
    public byte joustWeekWinCount;
}

