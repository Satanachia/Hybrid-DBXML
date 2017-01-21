namespace XMLDB3
{
    using System;

    public interface CastleAdapter
    {
        bool CreateBid(CastleBid _bid);
        REPLY_RESULT CreateBidder(CastleBidder _bidder, GuildAdapter _guildAdapter, ref int _remainMoney);
        REPLY_RESULT DeleteBidder(long _castleID, long _guildID, int _repayMoney, GuildAdapter _guildAdapter, ref int _remainMoney);
        bool EndBid(Castle _castle, GuildAdapter _guildAdapter);
        REPLY_RESULT GiveGuildMoney(long _castleID, long _guildID, int _money, GuildAdapter _guildAdapter);
        void Initialize(string _argument);
        CastleList ReadList();
        REPLY_RESULT TakeGuildMoney(long _castleID, long _guildID, int _money, ref int _remainMoney, GuildAdapter _guildAdapter);
        REPLY_RESULT UpdateBidder(long _castleID, long _guildID, int _bidPrice, int _bidDiffPrice, int _bidOrder, GuildAdapter _guildAdapter, ref int _remainMoney);
        bool UpdateBlock(long _castleID, CastleBlock[] _added, CastleBlock[] _deleted);
        bool UpdateBuild(long _castleID, CastleBuild _build);
        bool UpdateBuildResource(long _castleID, CastleBuildResource _resource);
        bool Write(Castle _castle);
    }
}

