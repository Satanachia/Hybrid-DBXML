namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseSerializer
    {
        public static void Deserialize(House _house, Message _message)
        {
            if (_house != null)
            {
                _message.WriteU8(1);
                HouseAppearSerializer.Deserialize(_house, _message);
                if (_house.bid != null)
                {
                    _message.WriteU8(1);
                    HouseBidSerializer.Deserialize(_house.bid, _message);
                }
                else
                {
                    _message.WriteU8(0);
                }
                if (_house.bidders != null)
                {
                    _message.WriteS32(_house.bidders.Length);
                    foreach (HouseBidder bidder in _house.bidders)
                    {
                        HouseBidderSerializer.Deserialize(bidder, _message);
                    }
                }
                else
                {
                    _message.WriteS32(0);
                }
            }
            else
            {
                _message.WriteU8(0);
                _message.WriteU8(0);
                _message.WriteS32(0);
            }
        }
    }
}

