namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidSerializer
    {
        public static void Deserialize(HouseBid _housebid, Message _message)
        {
            _message.WriteS64(_housebid.bidEndTime.Ticks);
            _message.WriteS64(_housebid.bidRepayEndTime.Ticks);
            _message.WriteS32(_housebid.minBidPrice);
        }

        public static HouseBid Serialize(Message _message)
        {
            HouseBid bid = new HouseBid();
            bid.bidEndTime = new DateTime(_message.ReadS64());
            bid.bidRepayEndTime = new DateTime(_message.ReadS64());
            bid.minBidPrice = _message.ReadS32();
            return bid;
        }
    }
}

