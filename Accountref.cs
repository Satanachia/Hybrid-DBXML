using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Accountref
{
    [XmlAttribute]
    public string account;
    [XmlAttribute]
    public bool beginnerFlag;
    [XmlElement("character")]
    public AccountrefCharacter[] character;
    [XmlAttribute]
    public int flag;
    [XmlAttribute]
    public DateTime @in;
    [XmlAttribute]
    public int lobbyOption;
    [XmlAttribute]
    public byte macroCheckFailure;
    [XmlAttribute]
    public byte macroCheckSuccess;
    [XmlAttribute]
    public short maxslot;
    [XmlAttribute]
    public DateTime @out;
    [XmlElement("pet")]
    public AccountrefPet[] pet;
    [XmlAttribute]
    public int playabletime;
    [XmlAttribute]
    public int supportLastChangeTime;
    [XmlAttribute]
    public byte supportRace;
    [XmlAttribute]
    public byte supportRewardState;
}

