namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilySerializer
    {
        public static void Deserialize(FamilyListFamily _data, Message _message)
        {
            if (_data != null)
            {
                _message.WriteS64(_data.familyID);
                _message.WriteString(_data.familyName);
                _message.WriteS64(_data.headID);
                _message.WriteU16(_data.state);
                _message.WriteU32(_data.tradition);
                _message.WriteString(_data.meta);
                if (((_data == null) || (_data.member == null)) || (_data.member.Length == 0))
                {
                    _message.WriteS32(0);
                }
                else
                {
                    _message.WriteS32(_data.member.Length);
                    foreach (FamilyListFamilyMember member in _data.member)
                    {
                        FamilyMemberSerializer.Deserialize(member, _message);
                    }
                }
            }
        }

        public static FamilyListFamily Serialize(Message _message)
        {
            FamilyListFamily family = new FamilyListFamily();
            family.familyID = _message.ReadS64();
            family.familyName = _message.ReadString();
            family.headID = _message.ReadS64();
            family.state = _message.ReadU16();
            family.tradition = _message.ReadU32();
            family.meta = _message.ReadString();
            uint num = _message.ReadU32();
            if (num > 0)
            {
                family.member = new FamilyListFamilyMember[num];
                for (int i = 0; i < num; i++)
                {
                    family.member[i] = FamilyMemberSerializer.Serialize(_message);
                }
                return family;
            }
            family.member = null;
            return family;
        }
    }
}

