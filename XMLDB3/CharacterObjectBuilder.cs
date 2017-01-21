namespace XMLDB3
{
    using System;
    using System.Data;

    public class CharacterObjectBuilder
    {
        public static CharacterInfo Build(DataSet ds)
        {
            DataTable table = ds.Tables["character"];
            DataTable table2 = ds.Tables["character_skill2"];
            DataTable table3 = ds.Tables["CharacterQuest"];
            DataTable table4 = ds.Tables["CharItemLarge"];
            DataTable table5 = ds.Tables["CharItemSmall"];
            DataTable table6 = ds.Tables["CharItemHuge"];
            DataTable table7 = ds.Tables["CharItemQuest"];
            DataTable table8 = ds.Tables["CharacterAchievement"];
            DataTable table9 = null;
            if (ds.Tables.Contains("CharItemEgo"))
            {
                table9 = ds.Tables["CharItemEgo"];
            }
            if ((((table == null) || (table2 == null)) || ((table3 == null) || (table4 == null))) || (((table5 == null) || (table6 == null)) || ((table7 == null) || (table8 == null))))
            {
                throw new Exception("캐릭터 테이블이 없습니다.");
            }
            DataTable table10 = null;
            if (ConfigManager.IsPVPable)
            {
                if (!ds.Tables.Contains("character_pvp") || (ds.Tables["character_pvp"] == null))
                {
                    throw new Exception("캐릭터 PVP 테이블이 없습니다.");
                }
                table10 = ds.Tables["character_pvp"];
            }
            if (table.Rows.Count != 1)
            {
                throw new Exception("캐릭터 테이블에 캐릭터가 없거나, 두개 이상입니다.");
            }
            CharacterInfo info = new CharacterInfo();
            info.id = (long) table.Rows[0]["id"];
            info.name = (string) table.Rows[0]["name"];
            info.appearance = AppearanceObjectBuilder.Build(table.Rows[0]);
            info.parameter = ParameterObjectBuilder.Build(table.Rows[0]);
            info.parameterEx = ParameterExObjectBuilder.Build(table.Rows[0]);
            info.data = DataObjectBuilder.Build(table.Rows[0]);
            info.keywords = KeywordObjectBuilder.Build(table.Rows[0]);
            info.memorys = MemoryObjectBuilder.Build(table.Rows[0]);
            info.conditions = ConditionObjectBuilder.Build(table.Rows[0]);
            info.arbeit = ArbeitObjectBuilder.Build(table.Rows[0]);
            info.@private = PrivateObjectBuilder.Build(table.Rows[0], table3);
            info.titles = TitleObjectBuilder.Build(table.Rows[0]);
            info.service = ServiceObjectBuilder.Build(table.Rows[0]);
            info.skills = SkillObjectBuilder.Build(table2);
            info.marriage = MarriageObjectBuilder.Build(table.Rows[0]);
            info.PVP = PVPObjectBuilder.Build(table10);
            info.farm = FarmObjectBuilder.Build(table.Rows[0]);
            info.heartSticker = HeartStickerObjectBuilder.Build(table.Rows[0]);
            info.joust = JoustObjectBuilder.Build(table.Rows[0]);
            info.macroChecker = MacroCheckerObjectBuilder.Build(table.Rows[0]);
            info.donation = DonationObjectBuilder.Build(table.Rows[0]);
            info.job = JobObjectBuilder.Build(table.Rows[0]);
            info.inventory = InventoryObjectBuilder.Build(table4, table5, table6, table7, table9);
            if ((table8.Rows != null) && (table8.Rows.Count > 0))
            {
                info.achievements = AchievementObjectBuilder.Build(table8.Rows[0]);
                return info;
            }
            info.achievements = new CharacterAchievements();
            return info;
        }
    }
}

