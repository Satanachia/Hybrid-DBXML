namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBidderSerializer
    {
        public static void Deserialize(HouseBidder _housebidder, Message _message)
        {
            _message.WriteString(_housebidder.bidAccount);
            _message.WriteS32(_housebidder.bidPrice);
            _message.WriteS32(_housebidder.bidOrder);
            _message.WriteString(_housebidder.bidCharName);
        }

        public static HouseBidder Serialize(Message _message)
        {
            HouseBidder bidder = new HouseBidder();
            bidder.bidAccount = _message.ReadString();
            bidder.bidPrice = _message.ReadS32();
            bidder.bidOrder = _message.ReadS32();
            bidder.bidCharName = _message.ReadString();
            bidder.bidCharacter = _message.ReadS64();
            return bidder;
        }
    }
}

