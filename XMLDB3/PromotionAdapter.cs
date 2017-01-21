namespace XMLDB3
{
    using System;

    public interface PromotionAdapter
    {
        bool BeginPromotion(string serverName, string channelName, ushort skillid);
        bool EndPromotion(string serverName, ushort skillid);
        void Initialize(string _Argument);
        bool RecordScore(string serverName, string channelName, ushort skillid, string skillCategory, string skillName, ulong characterID, string characterName, byte race, ushort level, uint point);
    }
}

