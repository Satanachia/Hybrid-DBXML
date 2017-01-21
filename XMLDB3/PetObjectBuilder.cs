namespace XMLDB3
{
    using System;
    using System.Data;

    public class PetObjectBuilder
    {
        public static PetInfo Build(DataSet ds)
        {
            try
            {
                DataTable table = ds.Tables["pet"];
                DataTable table2 = ds.Tables["pet_skill"];
                DataTable table3 = ds.Tables["CharItemLarge"];
                DataTable table4 = ds.Tables["CharItemSmall"];
                DataTable table5 = ds.Tables["CharItemHuge"];
                DataTable table6 = ds.Tables["CharItemQuest"];
                if ((((table == null) || (table2 == null)) || ((table3 == null) || (table4 == null))) || ((table5 == null) || (table6 == null)))
                {
                    throw new Exception("펫 테이블이 없습니다.");
                }
                if (table.Rows.Count != 1)
                {
                    throw new Exception("캐릭터 테이블 열이 하나 이상입니다.");
                }
                PetInfo info = new PetInfo();
                info.id = (long) table.Rows[0]["id"];
                info.name = (string) table.Rows[0]["name"];
                info.appearance = PetAppearanceObjectBuilder.Build(table.Rows[0]);
                info.parameter = PetParameterObjectBuilder.Build(table.Rows[0]);
                info.parameterEx = PetParameterExObjectBuilder.Build(table.Rows[0]);
                info.data = PetDataObjectBuilder.Build(table.Rows[0]);
                info.memorys = PetMemoryObjectBuilder.Build(table.Rows[0]);
                info.conditions = PetConditionObjectBuilder.Build(table.Rows[0]);
                info.@private = PetPrivateObjectBuilder.Build(table.Rows[0]);
                info.summon = PetSummonObjectBuilder.Build(table.Rows[0]);
                info.macroChecker = PetMacroCheckerObjectBuilder.Build(table.Rows[0]);
                info.skills = PetSkillObjectBuilder.Build(table2);
                info.inventory = PetInventoryObjectBuilder.Build(table3, table4, table5, table6);
                return info;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                return null;
            }
        }
    }
}

