namespace XMLDB3
{
    using System;
    using System.Text;

    public class PetUpdateBuilder
    {
        public static string Build(Pet _new, Pet _old)
        {
            if (_new == null)
            {
                throw new ArgumentException("펫 데이터가 없습니다.", "_new");
            }
            if (_old == null)
            {
                throw new ArgumentException("펫 캐쉬 데이터가 없습니다.", "_old");
            }
            StringBuilder builder = new StringBuilder(0x3e8);
            builder.Append(PetAppearanceUpdateBuilder.Build(_new, _old));
            builder.Append(PetParameterUpdateBuilder.Build(_new, _old));
            builder.Append(PetParameterExUpdateBuilder.Build(_new, _old));
            builder.Append(PetDataUpdateBuilder.Build(_new, _old));
            builder.Append(PetMemoryUpdateBuilder.Build(_new, _old));
            builder.Append(PetConditionUpdateBuilder.Build(_new, _old));
            builder.Append(PetPrivateUpdateBuilder.Build(_new, _old));
            builder.Append(PetSummonUpdateBuilder.Build(_new, _old));
            builder.Append(PetMacroCheckerUpdateBuilder.Build(_new, _old));
            string str = PetSkillUpdateBuilder.Build(_new, _old);
            builder.Append(" where id=" + _new.id + "\n");
            builder.Append(str);
            return ("update pet set update_time=getdate()" + builder.ToString());
        }
    }
}

