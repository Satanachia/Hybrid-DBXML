namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class PetSkillObjectBuilder
    {
        public static PetSkill[] Build(DataTable _skill_table)
        {
            if ((_skill_table == null) || (_skill_table.Rows == null))
            {
                return null;
            }
            ArrayList list = new ArrayList();
            foreach (DataRow row in _skill_table.Rows)
            {
                PetSkill skill = new PetSkill();
                skill.id = (short) row["skill"];
                skill.level = (byte) row["level"];
                skill.flag = (short) row["flag"];
                list.Add(skill);
            }
            return (PetSkill[]) list.ToArray(typeof(PetSkill));
        }
    }
}

