namespace XMLDB3
{
    using System;

    public class FamilyEmptyAdapter : FamilyAdapter
    {
        public REPLY_RESULT AddFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT AddMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public void Initialize(string _argument)
        {
        }

        public FamilyListFamily Read(long _familyID)
        {
            return null;
        }

        public FamilyList ReadList()
        {
            return null;
        }

        public REPLY_RESULT RemoveFamily(long _familyID, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT RemoveMember(long _familyID, long _memberID, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT UpdateFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT UpdateMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }
    }
}

