namespace XMLDB3
{
    using System;

    public class JoustUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.joust == null) || (_old.joust == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.joust.joustPoint != _old.joust.joustPoint)
            {
                str = str + ",[joustPoint]=" + _new.joust.joustPoint;
            }
            if (_new.joust.joustLastWinYear != _old.joust.joustLastWinYear)
            {
                str = str + ",[joustLastWinYear]=" + _new.joust.joustLastWinYear;
            }
            if (_new.joust.joustLastWinWeek != _old.joust.joustLastWinWeek)
            {
                str = str + ",[joustLastWinWeek]=" + _new.joust.joustLastWinWeek;
            }
            if (_new.joust.joustWeekWinCount != _old.joust.joustWeekWinCount)
            {
                str = str + ",[joustWeekWinCount]=" + _new.joust.joustWeekWinCount;
            }
            if (_new.joust.joustDailyWinCount != _old.joust.joustDailyWinCount)
            {
                str = str + ",[joustDailyWinCount]=" + _new.joust.joustDailyWinCount;
            }
            if (_new.joust.joustDailyLoseCount != _old.joust.joustDailyLoseCount)
            {
                str = str + ",[joustDailyLoseCount]=" + _new.joust.joustDailyLoseCount;
            }
            if (_new.joust.joustServerWinCount != _old.joust.joustServerWinCount)
            {
                str = str + ",[joustServerWinCount]=" + _new.joust.joustServerWinCount;
            }
            if (_new.joust.joustServerLoseCount != _old.joust.joustServerLoseCount)
            {
                str = str + ",[joustServerLoseCount]=" + _new.joust.joustServerLoseCount;
            }
            return str;
        }
    }
}

