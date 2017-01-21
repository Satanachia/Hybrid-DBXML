namespace XMLDB3
{
    using System;

    public interface RoyalAlchemistAdapter
    {
        REPLY_RESULT Add(RoyalAlchemist _royalAlchemist, ref byte _errorCode);
        void Initialize(string _argument);
        RoyalAlchemist Read(long _charID);
        RoyalAlchemistList ReadList();
        REPLY_RESULT Remove(long[] _removeIDs, ref byte _errorCode);
        REPLY_RESULT Update(RoyalAlchemist _royalAlchemist, ref byte _errorCode);
    }
}

