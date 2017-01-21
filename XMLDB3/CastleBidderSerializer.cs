namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBidderSerializer
    {
        public static void Deserialize(CastleBidder _bidder, Message _message)
        {
            _message.WriteS64(_bidder.castleID);
            _message.WriteS64(_bidder.bidGuildID);
            _message.WriteS32(_bidder.bidPrice);
            _message.WriteS32(_bidder.bidOrder);
        }

        public static CastleBidder Serialize(Message _message)
        {
            CastleBidder bidder = new CastleBidder();
            bidder.castleID = _message.ReadS64();
            bidder.bidGuildID = _message.ReadS64();
            bidder.bidPrice = _message.ReadS32();
            bidder.bidOrder = _message.ReadS32();
            bidder.bidGuildName = _message.ReadString();
            bidder.bidCharName = _message.ReadString();
            bidder.bidCharacter = _message.ReadS64();
            return bidder;
        }
    }
}

