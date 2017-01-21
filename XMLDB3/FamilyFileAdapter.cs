namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;

    public class FamilyFileAdapter : FileAdapter, FamilyAdapter
    {
        public REPLY_RESULT AddFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            if (base.IsExistData(_family.familyID))
            {
                _errorCode = 0;
                return REPLY_RESULT.FAIL_EX;
            }
            base.WriteToDB(_family, _family.familyID);
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT AddMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            FamilyListFamily family = this.Read(_familyID);
            if (family != null)
            {
                FamilyListFamilyMember[] member = family.member;
                if (member == null)
                {
                    family.member = new FamilyListFamilyMember[] { _member };
                }
                else
                {
                    family.member = new FamilyListFamilyMember[member.Length + 1];
                    member.CopyTo(family.member, 0);
                    family.member[member.Length] = _member;
                }
                base.WriteToDB(family, family.familyID);
                return REPLY_RESULT.SUCCESS;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(FamilyListFamily), ConfigManager.GetFileDBPath("family"), ".xml");
        }

        public FamilyListFamily Read(long _familyID)
        {
            if (base.IsExistData(_familyID))
            {
                return (FamilyListFamily) base.ReadFromDB(_familyID);
            }
            return null;
        }

        public FamilyList ReadList()
        {
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if ((files == null) || (files.Length <= 0))
            {
                return new FamilyList();
            }
            ArrayList list = new ArrayList();
            foreach (string str in files)
            {
                FamilyListFamily family = (FamilyListFamily) base.ReadFromDB(Path.GetFileNameWithoutExtension(str));
                if (family != null)
                {
                    list.Add(family);
                }
            }
            FamilyList list2 = new FamilyList();
            list2.family = (FamilyListFamily[]) list.ToArray(typeof(FamilyListFamily));
            return list2;
        }

        public REPLY_RESULT RemoveFamily(long _familyID, ref byte _errorCode)
        {
            if (base.IsExistData(_familyID))
            {
                if (base.DeleteDB(_familyID))
                {
                    return REPLY_RESULT.SUCCESS;
                }
                return REPLY_RESULT.FAIL;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        public REPLY_RESULT RemoveMember(long _familyID, long _memberID, ref byte _errorCode)
        {
            FamilyListFamily family = this.Read(_familyID);
            if (family != null)
            {
                ArrayList list = new ArrayList();
                bool flag = false;
                foreach (FamilyListFamilyMember member in family.member)
                {
                    if (member.memberID == _memberID)
                    {
                        flag = true;
                    }
                    else
                    {
                        list.Add(member);
                    }
                }
                if (flag)
                {
                    family.member = (FamilyListFamilyMember[]) list.ToArray(typeof(FamilyListFamilyMember));
                    base.WriteToDB(family, family.familyID);
                    return REPLY_RESULT.SUCCESS;
                }
            }
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT UpdateFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            FamilyListFamily family = this.Read(_family.familyID);
            if (family != null)
            {
                family.familyName = _family.familyName;
                family.headID = _family.headID;
                family.state = _family.state;
                family.tradition = _family.tradition;
                family.meta = _family.meta;
                base.WriteToDB(family, family.familyID);
                return REPLY_RESULT.SUCCESS;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        public REPLY_RESULT UpdateMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            FamilyListFamily family = this.Read(_familyID);
            if (family != null)
            {
                bool flag = false;
                for (int i = 0; i < family.member.Length; i++)
                {
                    if (family.member[i].memberID == _member.memberID)
                    {
                        family.member[i] = _member;
                        flag = true;
                    }
                }
                if (flag)
                {
                    base.WriteToDB(family, family.familyID);
                    return REPLY_RESULT.SUCCESS;
                }
            }
            return REPLY_RESULT.FAIL;
        }
    }
}

