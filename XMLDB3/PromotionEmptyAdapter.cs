namespace XMLDB3
{
    using System;

    public class PromotionEmptyAdapter : PromotionAdapter
    {
        public bool BeginPromotion(string serverName, string channelName, ushort skillid)
        {
            return false;
        }

        public bool EndPromotion(string serverName, ushort skillid)
        {
            return false;
        }

        public void Initialize(string _Argument)
        {
        }

        public bool RecordScore(string serverName, string channelName, ushort skillid, string skillCategory, string skillName, ulong characterID, string characterName, byte race, ushort level, uint point)
        {
            return false;
        }
    }
}

