namespace XMLDB3
{
    using System;
    using System.Data;

    public class JoustObjectBuilder
    {
        public static CharacterJoust Build(DataRow _character_row)
        {
            CharacterJoust joust = new CharacterJoust();
            joust.joustPoint = (int) _character_row["joustPoint"];
            joust.joustLastWinYear = (byte) _character_row["joustLastWinYear"];
            joust.joustLastWinWeek = (byte) _character_row["joustLastWinWeek"];
            joust.joustWeekWinCount = (byte) _character_row["joustWeekWinCount"];
            joust.joustDailyWinCount = (short) _character_row["joustDailyWinCount"];
            joust.joustDailyLoseCount = (short) _character_row["joustDailyLoseCount"];
            joust.joustServerWinCount = (short) _character_row["joustServerWinCount"];
            joust.joustServerLoseCount = (short) _character_row["joustServerLoseCount"];
            return joust;
        }
    }
}

