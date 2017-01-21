namespace XMLDB3
{
    using System;
    using System.Data;

    public class HeartStickerObjectBuilder
    {
        public static CharacterHeartSticker Build(DataRow _character_row)
        {
            CharacterHeartSticker sticker = new CharacterHeartSticker();
            sticker.heartUpdateTime = (long) _character_row["heartUpdateTime"];
            sticker.heartPoint = (short) _character_row["heartPoint"];
            sticker.heartTotalPoint = (short) _character_row["heartTotalPoint"];
            return sticker;
        }
    }
}

