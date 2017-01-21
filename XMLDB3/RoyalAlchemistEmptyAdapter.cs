namespace XMLDB3
{
    using System;

    public class RoyalAlchemistEmptyAdapter : RoyalAlchemistAdapter
    {
        public REPLY_RESULT Add(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public void Initialize(string _argument)
        {
        }

        public RoyalAlchemist Read(long _charID)
        {
            return null;
        }

        public RoyalAlchemistList ReadList()
        {
            return new RoyalAlchemistList();
        }

        public REPLY_RESULT Remove(long[] _removeIDs, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT Update(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }
    }
}

