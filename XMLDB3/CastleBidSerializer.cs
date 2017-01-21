namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBidSerializer
    {
        public static void Deserialize(CastleBid _bid, Message _message)
        {
            _message.WriteS64(_bid.castleID);
            _message.WriteS64(_bid.bidEndTime.Ticks);
            _message.WriteS32(_bid.minBidPrice);
        }

        public static CastleBid Serialize(Message _message)
        {
            CastleBid bid = new CastleBid();
            bid.castleID = _message.ReadS64();
            bid.bidEndTime = new DateTime(_message.ReadS64());
            bid.minBidPrice = _message.ReadS32();
            return bid;
        }
    }
}

