namespace XMLDB3
{
    using System;

    public class PVPUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if (_new.PVP == null)
            {
                _new.PVP = new CharacterPVP();
            }
            return string.Concat(new object[] { "exec dbo.UpdateCharacterPVP  @idCharacter=", _new.id, ",@winCnt=", _new.PVP.winCnt, ",@loseCnt=", _new.PVP.loseCnt, ",@penaltyPoint=", _new.PVP.penaltyPoint, "\n" });
        }
    }
}

