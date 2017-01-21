namespace XMLDB3
{
    using System;

    public interface BidAdapter
    {
        bool Add(Bid _bid);
        void Initialize(string _argument);
        BidList Read();
        REPLY_RESULT Remove(long _bidID, ref byte _errorCode);
        REPLY_RESULT Update(Bid _bid, ref byte _errorCode);
    }
}

