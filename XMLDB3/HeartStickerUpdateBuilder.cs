namespace XMLDB3
{
    using System;

    public class HeartStickerUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if ((_new.heartSticker == null) || (_old.heartSticker == null))
            {
                return string.Empty;
            }
            string str = string.Empty;
            if (_new.heartSticker.heartUpdateTime != _old.heartSticker.heartUpdateTime)
            {
                str = str + ",[heartUpdateTime]=" + _new.heartSticker.heartUpdateTime;
            }
            if (_new.heartSticker.heartPoint != _old.heartSticker.heartPoint)
            {
                str = str + ",[heartPoint]=" + _new.heartSticker.heartPoint;
            }
            if (_new.heartSticker.heartTotalPoint != _old.heartSticker.heartTotalPoint)
            {
                str = str + ",[heartTotalPoint]=" + _new.heartSticker.heartTotalPoint;
            }
            return str;
        }
    }
}

