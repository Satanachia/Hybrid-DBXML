namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class SkillObjectBuilder
    {
        public static CharacterSkill[] Build(DataTable _skill_table)
        {
            if ((_skill_table == null) || (_skill_table.Rows == null))
            {
                return null;
            }
            ArrayList list = new ArrayList();
            foreach (DataRow row in _skill_table.Rows)
            {
                CharacterSkill skill = new CharacterSkill();
                skill.id = (short) row["skill"];
                skill.version = (short) ((int) row["version"]);
                skill.level = (byte) row["level"];
                skill.maxlevel = (byte) row["maxlevel"];
                skill.experience = (int) row["experience"];
                skill.count = (short) row["count"];
                skill.flag = (short) row["flag"];
                skill.subflag1 = (short) row["subflag1"];
                skill.subflag2 = (short) row["subflag2"];
                skill.subflag3 = (short) row["subflag3"];
                skill.subflag4 = (short) row["subflag4"];
                skill.subflag5 = (short) row["subflag5"];
                skill.subflag6 = (short) row["subflag6"];
                skill.subflag7 = (short) row["subflag7"];
                skill.subflag8 = (short) row["subflag8"];
                skill.subflag9 = (short) row["subflag9"];
                skill.lastPromotionTime = (long) row["lastPromotionTime"];
                skill.promotionConditionCount = (short) row["promotionConditionCount"];
                skill.promotionExperience = (int) row["promotionExperience"];
                list.Add(skill);
            }
            return (CharacterSkill[]) list.ToArray(typeof(CharacterSkill));
        }
    }
}

