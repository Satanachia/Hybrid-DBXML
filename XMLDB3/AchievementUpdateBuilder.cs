namespace XMLDB3
{
    using System;
    using System.Text;

    public class AchievementUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            string str = BuildAchievementDBString(_new.achievements.achievement);
            string str2 = BuildAchievementDBString(_new.achievements.achievement);
            if (!(str != str2) && (_new.achievements.totalscore == _old.achievements.totalscore))
            {
                return string.Empty;
            }
            return string.Format("exec dbo.UpdateAchievement @idCharacter={0}, @totalscore={1}, @achievement={2}\n", _new.id, _new.achievements.totalscore, UpdateUtility.BuildString(str));
        }

        private static string BuildAchievementDBString(CharacterAchievementsAchievement[] _achievements)
        {
            if ((_achievements == null) || (_achievements.Length <= 0))
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder("");
            foreach (CharacterAchievementsAchievement achievement in _achievements)
            {
                builder.AppendFormat("{0}@{1}|", achievement.setid, achievement.bitflag);
            }
            return builder.ToString();
        }
    }
}

