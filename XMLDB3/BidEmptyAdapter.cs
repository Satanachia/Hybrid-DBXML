namespace XMLDB3
{
    using System;

    public class BidEmptyAdapter : BidAdapter
    {
        public bool Add(Bid _bid)
        {
            return false;
        }

        public void Initialize(string _argument)
        {
        }

        public BidList Read()
        {
            return null;
        }

        public REPLY_RESULT Remove(long _bidID, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT Update(Bid _bid, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }
    }
}

