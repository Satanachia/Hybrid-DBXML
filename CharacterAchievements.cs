using System;
using System.Xml.Serialization;

public class CharacterAchievements
{
    [XmlElement("achievement")]
    public CharacterAchievementsAchievement[] achievement;
    [XmlAttribute]
    public int totalscore;
}

