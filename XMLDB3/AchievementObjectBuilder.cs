namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class AchievementObjectBuilder
    {
        public static CharacterAchievements Build(DataRow _achievementRow)
        {
            CharacterAchievements achievements = new CharacterAchievements();
            achievements.totalscore = (int) _achievementRow["totalscore"];
            string str = (string) _achievementRow["achievement"];
            if (str.Length > 0)
            {
                string[] strArray = str.Split(new char[] { '|' });
                ArrayList list = new ArrayList();
                foreach (string str2 in strArray)
                {
                    if ((str2 != null) && (str2.Length > 0))
                    {
                        string[] strArray2 = str2.Split(new char[] { '@' });
                        if ((strArray2 == null) || (strArray2.Length < 2))
                        {
                            ExceptionMonitor.ExceptionRaised(new Exception("Fail to parse achievement"), str);
                        }
                        else
                        {
                            try
                            {
                                short num = Convert.ToInt16(strArray2[0]);
                                int num2 = Convert.ToInt32(strArray2[1]);
                                CharacterAchievementsAchievement achievement = new CharacterAchievementsAchievement();
                                achievement.setid = num;
                                achievement.bitflag = num2;
                                list.Add(achievement);
                            }
                            catch (Exception exception)
                            {
                                ExceptionMonitor.ExceptionRaised(exception, str);
                            }
                        }
                    }
                }
                achievements.achievement = (CharacterAchievementsAchievement[]) list.ToArray(typeof(CharacterAchievementsAchievement));
            }
            return achievements;
        }
    }
}

