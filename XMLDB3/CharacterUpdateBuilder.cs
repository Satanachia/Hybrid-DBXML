namespace XMLDB3
{
    using System;
    using System.Text;

    public class CharacterUpdateBuilder
    {
        public static string Build(Character _new, Character _old)
        {
            if (_new == null)
            {
                throw new ArgumentException("캐릭터 데이터가 없습니다.", "_new");
            }
            if (_old == null)
            {
                throw new ArgumentException("캐릭터 캐쉬 데이터가 없습니다.", "_old");
            }
            StringBuilder builder = new StringBuilder(0x3e8);
            builder.Append(AppearanceUpdateBuilder.Build(_new, _old));
            builder.Append(ParameterUpdateBuilder.Build(_new, _old));
            builder.Append(ParameterExUpdateBuilder.Build(_new, _old));
            builder.Append(UserDataUpdateBuilder.Build(_new, _old));
            builder.Append(TitleUpdateBuilder.Build(_new, _old));
            builder.Append(MarriageUpdateBuilder.Build(_new, _old));
            builder.Append(MemoryUpdateBuilder.Build(_new, _old));
            builder.Append(KeywordUpdateBuilder.Build(_new, _old));
            builder.Append(ArbeitUpdateBuilder.Build(_new, _old));
            builder.Append(ConditionUpdateBuilder.Build(_new, _old));
            builder.Append(PrivateUpdateBuilder.Build(_new, _old));
            builder.Append(ServiceUpdateBuilder.Build(_new, _old));
            builder.Append(FarmUpdateBuilder.Build(_new, _old));
            builder.Append(HeartStickerUpdateBuilder.Build(_new, _old));
            builder.Append(JoustUpdateBuilder.Build(_new, _old));
            builder.Append(MacroCheckerUpdateBuilder.Build(_new, _old));
            builder.Append(DonationUpdateBuilder.Build(_new, _old));
            builder.Append(JobUpdateBuilder.Build(_new, _old));
            string str = SkillUpdateBuilder.Build(_new, _old);
            string str2 = QuestUpdateBuilder.Build(_new, _old);
            string str3 = AchievementUpdateBuilder.Build(_new, _old);
            builder.Append(" where id=" + _new.id + "\n");
            builder.Append(str);
            builder.Append(str2);
            builder.Append(str3);
            if (ConfigManager.IsPVPable)
            {
                builder.Append(PVPUpdateBuilder.Build(_new, _old));
            }
            return ("update character set update_time=getdate()" + builder.ToString());
        }
    }
}

