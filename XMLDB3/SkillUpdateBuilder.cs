namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Text;

    public class SkillUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            StringBuilder builder = new StringBuilder();
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            if (_new.skills != null)
            {
                foreach (CharacterSkill skill in _new.skills)
                {
                    hashtable.Add(skill.id, skill);
                }
            }
            if (_old.skills != null)
            {
                foreach (CharacterSkill skill2 in _old.skills)
                {
                    hashtable2.Add(skill2.id, skill2);
                }
            }
            foreach (CharacterSkill skill3 in hashtable2.Values)
            {
                if (!hashtable.Contains(skill3.id))
                {
                    builder.Append(string.Concat(new object[] { "exec dbo.DeleteCharacterSkill @idCharacter=", _new.id, ",@skill=", skill3.id, "\n" }));
                }
            }
            foreach (CharacterSkill skill4 in hashtable.Values)
            {
                CharacterSkill skill5 = null;
                if (hashtable2.Contains(skill4.id))
                {
                    skill5 = (CharacterSkill) hashtable2[skill4.id];
                }
                builder.Append(BuildSkill(skill4, skill5, _new.id));
            }
            return builder.ToString();
        }

        private static string BuildSkill(CharacterSkill _new, CharacterSkill _old, long _idchar)
        {
            if (((_new != null) && (_old != null)) && (_new.id == _old.id))
            {
                bool flag = false;
                string str = "update character_skill set ";
                if (_new.version != _old.version)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[version]=" + _new.version;
                }
                if (_new.level != _old.level)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[level]=" + _new.level;
                }
                if (_new.maxlevel != _old.maxlevel)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[maxlevel]=" + _new.maxlevel;
                }
                if (_new.experience != _old.experience)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[experience]=" + _new.experience;
                }
                if (_new.count != _old.count)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[count]=" + _new.count;
                }
                if (_new.flag != _old.flag)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[flag]=" + _new.flag;
                }
                if (_new.subflag1 != _old.subflag1)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag1]=" + _new.subflag1;
                }
                if (_new.subflag2 != _old.subflag2)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag2]=" + _new.subflag2;
                }
                if (_new.subflag3 != _old.subflag3)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag3]=" + _new.subflag3;
                }
                if (_new.subflag4 != _old.subflag4)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag4]=" + _new.subflag4;
                }
                if (_new.subflag5 != _old.subflag5)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag5]=" + _new.subflag5;
                }
                if (_new.subflag6 != _old.subflag6)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag6]=" + _new.subflag6;
                }
                if (_new.subflag7 != _old.subflag7)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag7]=" + _new.subflag7;
                }
                if (_new.subflag8 != _old.subflag8)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag8]=" + _new.subflag8;
                }
                if (_new.subflag9 != _old.subflag9)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[subflag9]=" + _new.subflag9;
                }
                if (_new.lastPromotionTime != _old.lastPromotionTime)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[lastPromotionTime]=" + _new.lastPromotionTime;
                }
                if (_new.promotionConditionCount != _old.promotionConditionCount)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[promotionConditionCount]=" + _new.promotionConditionCount;
                }
                if (_new.promotionExperience != _old.promotionExperience)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[character_skill].[promotionExperience]=" + _new.promotionExperience;
                }
                object obj2 = str;
                str = string.Concat(new object[] { obj2, " where [character_skill].[id]=", _idchar, " and [character_skill].[skill]=", _new.id });
                if (flag)
                {
                    return (str + "\n");
                }
                return string.Empty;
            }
            if ((_new != null) && (_old == null))
            {
                return string.Concat(new object[] { 
                    "exec dbo.AddCharacterSkill @idCharacter=", _idchar, ",@skill=", _new.id, ",@version=", _new.version, ",@level=", _new.level, ",@maxlevel=", _new.maxlevel, ",@experience=", _new.experience, ",@count=", _new.count, ",@flag=", _new.flag, 
                    ",@subflag1=", _new.subflag1, ",@subflag2=", _new.subflag2, ",@subflag3=", _new.subflag3, ",@subflag4=", _new.subflag4, ",@subflag5=", _new.subflag5, ",@subflag6=", _new.subflag6, ",@subflag7=", _new.subflag7, ",@subflag8=", _new.subflag8, 
                    ",@subflag9=", _new.subflag9, ",@lastPromotionTime=", _new.lastPromotionTime, ",@promotionConditionCount=", _new.promotionConditionCount, ",@promotionExperience=", _new.promotionExperience, "\n"
                 });
            }
            return string.Empty;
        }
    }
}

