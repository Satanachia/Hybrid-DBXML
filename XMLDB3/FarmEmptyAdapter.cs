namespace XMLDB3
{
    using System;

    public class FarmEmptyAdapter : FarmAdapter
    {
        public REPLY_RESULT Expire(long _farmID, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public bool GetOwnerInfo(string _account, ref long _farmID, ref long _ownerCharID, ref string _ownerCharName)
        {
            return false;
        }

        public void Initialize(string _argument)
        {
        }

        public REPLY_RESULT Lease(long _farmID, string _account, long _charID, string _charName, long _expireTime, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public Farm Read(long _farmID)
        {
            return null;
        }

        public REPLY_RESULT Update(Farm _farm, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }
    }
}

