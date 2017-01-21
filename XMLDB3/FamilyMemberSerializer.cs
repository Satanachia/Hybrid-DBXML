namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyMemberSerializer
    {
        public static void Deserialize(FamilyListFamilyMember _data, Message _message)
        {
            if (_data != null)
            {
                _message.WriteS64(_data.memberID);
                _message.WriteString(_data.memberName);
                _message.WriteU16(_data.memberClass);
            }
        }

        public static FamilyListFamilyMember Serialize(Message _message)
        {
            FamilyListFamilyMember member = new FamilyListFamilyMember();
            member.memberID = _message.ReadS64();
            member.memberName = _message.ReadString();
            member.memberClass = _message.ReadU16();
            return member;
        }
    }
}

