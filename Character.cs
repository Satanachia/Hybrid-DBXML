using System;
using System.Xml.Serialization;

[XmlRoot(Namespace="", IsNullable=false)]
public class Character
{
    public CharacterAchievements achievements;
    public CharacterAppearance appearance;
    public CharacterArbeit arbeit;
    [XmlArrayItem("condition", IsNullable=false)]
    public CharacterCondition[] conditions;
    public CharacterData data;
    public CharacterDonation donation;
    public CharacterFarm farm;
    public CharacterHeartSticker heartSticker;
    [XmlAttribute]
    public long id;
    public CharacterJob job;
    public CharacterJoust joust;
    [XmlArrayItem("keyword", IsNullable=false)]
    public CharacterKeyword[] keywords;
    public CharacterMacroChecker macroChecker;
    public CharacterMarriage marriage;
    [XmlArrayItem("memory", IsNullable=false)]
    public CharacterMemory[] memorys;
    [XmlAttribute]
    public string name;
    public CharacterParameter parameter;
    public CharacterParameterEx parameterEx;
    public CharacterPrivate @private;
    public CharacterPVP PVP;
    public CharacterService service;
    [XmlArrayItem("skill", IsNullable=false)]
    public CharacterSkill[] skills;
    public CharacterTitles titles;
    [XmlAttribute]
    public DateTime updatetime;
}

