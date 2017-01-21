namespace XMLDB3
{
    using System;
    using System.Text;

    public class QuestUpdateBuilder
    {
        private static QuestComparer comparer = null;

        static QuestUpdateBuilder()
        {
            comparer = new QuestComparer();
        }

        public static string Build(Character _new, Character _old)
        {
            StringBuilder builder = new StringBuilder();
            CharacterPrivateRegistered[] registereds = _new.@private.registereds;
            CharacterPrivateRegistered[] array = _old.@private.registereds;
            if (registereds == null)
            {
                registereds = new CharacterPrivateRegistered[0];
            }
            if (array == null)
            {
                array = new CharacterPrivateRegistered[0];
            }
            Array.Sort(registereds, comparer);
            Array.Sort(array, comparer);
            int index = 0;
            int num2 = 0;
            index = 0;
            num2 = 0;
            while ((index < registereds.Length) && (num2 < array.Length))
            {
                if (registereds[index].id == array[num2].id)
                {
                    builder.Append(BuildQuest(registereds[index], array[num2], _new.id));
                    index++;
                    num2++;
                }
                else
                {
                    if (registereds[index].id > array[num2].id)
                    {
                        builder.Append(string.Concat(new object[] { "exec dbo.DeleteCharacterQuest @idCharacter=", _new.id, ",@idQuest=", array[num2].id, "\n" }));
                        num2++;
                        continue;
                    }
                    builder.Append(BuildQuest(registereds[index], null, _new.id));
                    index++;
                }
            }
            while (index < registereds.Length)
            {
                builder.Append(BuildQuest(registereds[index], null, _new.id));
                index++;
            }
            while (num2 < array.Length)
            {
                builder.Append(string.Concat(new object[] { "exec dbo.DeleteCharacterQuest @idCharacter=", _new.id, ",@idQuest=", array[num2].id, "\n" }));
                num2++;
            }
            return builder.ToString();
        }

        private static string BuildQuest(CharacterPrivateRegistered _new, CharacterPrivateRegistered _old, long _idchar)
        {
            if (((_new != null) && (_old != null)) && (_new.id == _old.id))
            {
                bool flag = false;
                string str = "update characterQuest set ";
                if (_new.start != _old.start)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[characterQuest].[start]=" + _new.start;
                }
                if (_new.end != _old.end)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[characterQuest].[end]=" + _new.end;
                }
                if (_new.extra != _old.extra)
                {
                    if (flag)
                    {
                        str = str + ",";
                    }
                    else
                    {
                        flag = true;
                    }
                    str = str + "[characterQuest].[extra]=" + _new.extra;
                }
                object obj2 = str;
                str = string.Concat(new object[] { obj2, " where [characterQuest].[id]=", _idchar, " and [characterQuest].[questId]=", _new.id });
                if (flag)
                {
                    return (str + "\n");
                }
                return string.Empty;
            }
            if ((_new != null) && (_old == null))
            {
                return string.Concat(new object[] { "exec dbo.AddCharacterQuest @idCharacter=", _idchar, ",@idQuest=", _new.id, ",@start=", _new.start, ",@end=", _new.end, ",@extra=", _new.extra, "\n" });
            }
            return string.Empty;
        }
    }
}

