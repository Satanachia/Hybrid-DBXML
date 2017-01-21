namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Text;

    public class PetSkillUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            StringBuilder builder = new StringBuilder();
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            if (_new.skills != null)
            {
                foreach (PetSkill skill in _new.skills)
                {
                    hashtable.Add(skill.id, skill);
                }
            }
            if (_old.skills != null)
            {
                foreach (PetSkill skill2 in _old.skills)
                {
                    hashtable2.Add(skill2.id, skill2);
                }
            }
            foreach (PetSkill skill3 in hashtable2.Values)
            {
                if (!hashtable.Contains(skill3.id))
                {
                    builder.Append(string.Concat(new object[] { "exec dbo.DeletePetSkill @idPet=", _new.id, ",@skill=", skill3.id, "\n" }));
                }
            }
            foreach (PetSkill skill4 in hashtable.Values)
            {
                PetSkill skill5 = null;
                if (hashtable2.Contains(skill4.id))
                {
                    skill5 = (PetSkill) hashtable2[skill4.id];
                }
                builder.Append(BuildSkill(skill4, skill5, _new.id));
            }
            return builder.ToString();
        }

        private static string BuildSkill(PetSkill _new, PetSkill _old, long _idPet)
        {
            string str = "pet_skill";
            if (((_new != null) && (_old != null)) && (_new.id == _old.id))
            {
                object obj2;
                bool flag = false;
                string str2 = "update " + str + " set ";
                if (_new.level != _old.level)
                {
                    if (flag)
                    {
                        str2 = str2 + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    obj2 = str2;
                    str2 = string.Concat(new object[] { obj2, "[", str, "].[level]=", _new.level });
                }
                if (_new.flag != _old.flag)
                {
                    if (flag)
                    {
                        str2 = str2 + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    obj2 = str2;
                    str2 = string.Concat(new object[] { obj2, "[", str, "].[flag]=", _new.flag });
                }
                obj2 = str2;
                str2 = string.Concat(new object[] { obj2, " where [", str, "].[id]=", _idPet, " and [", str, "].[skill]=", _new.id });
                if (flag)
                {
                    return (str2 + "\n");
                }
                return string.Empty;
            }
            if ((_new != null) && (_old == null))
            {
                return string.Concat(new object[] { "exec dbo.AddPetSkill @idPet=", _idPet, ",@skill=", _new.id, ",@level=", _new.level, ",@flag=", _new.flag, "\n" });
            }
            return string.Empty;
        }
    }
}

