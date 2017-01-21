namespace XMLDB3
{
    using System;

    public interface FamilyAdapter
    {
        REPLY_RESULT AddFamily(FamilyListFamily _family, ref byte _errorCode);
        REPLY_RESULT AddMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode);
        void Initialize(string _argument);
        FamilyListFamily Read(long _familyID);
        FamilyList ReadList();
        REPLY_RESULT RemoveFamily(long _familyID, ref byte _errorCode);
        REPLY_RESULT RemoveMember(long _familyID, long _memberID, ref byte _errorCode);
        REPLY_RESULT UpdateFamily(FamilyListFamily _family, ref byte _errorCode);
        REPLY_RESULT UpdateMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode);
    }
}

