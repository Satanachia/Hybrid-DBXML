namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BidListSerializer
    {
        public static void Deserialize(BidList _list, Message _message)
        {
            if (((_list == null) || (_list.bids == null)) || (_list.bids.Length == 0))
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_list.bids.Length);
                foreach (Bid bid in _list.bids)
                {
                    BidSerializer.Deserialize(bid, _message);
                }
            }
        }
    }
}

