namespace XMLDB3
{
    using System;

    public interface FarmAdapter
    {
        REPLY_RESULT Expire(long _farmID, ref byte _errorCode);
        bool GetOwnerInfo(string _account, ref long _farmID, ref long _ownerCharID, ref string _ownerCharName);
        void Initialize(string _Argument);
        REPLY_RESULT Lease(long _farmID, string _account, long _charID, string _charName, long _expireTime, ref byte _errorCode);
        Farm Read(long _farmID);
        REPLY_RESULT Update(Farm _farm, ref byte _errorCode);
    }
}

