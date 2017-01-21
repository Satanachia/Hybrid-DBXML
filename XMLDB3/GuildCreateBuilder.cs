namespace XMLDB3
{
    using System;

    public class GuildCreateBuilder
    {
        public static string Build(Guild _new)
        {
            if (_new == null)
            {
                return string.Empty;
            }
            long num = CheckGuildMaster(_new.member);
            string str = string.Concat(new object[] { 
                "exec CreateGuild3 @idGuild=", _new.id, ",@name=", UpdateUtility.BuildString(_new.name), ",@server=", UpdateUtility.BuildString(_new.server), ",@guildmoney=", _new.guildmoney, ",@drawablemoney=", _new.drawablemoney, ",@guildpoint=", _new.guildpoint, ",@guildtype=", _new.guildtype, ",@jointype=", _new.jointype, 
                ",@membernum=", _new.member.Length, ",@guildmasterid=", num, ",@expiration=", UpdateUtility.BuildDateTime(_new.expiration), ",@enable=", _new.enable, ",@profile=", UpdateUtility.BuildString(_new.profile), ",@greeting=", UpdateUtility.BuildString(_new.greeting), ",@leaving=", UpdateUtility.BuildString(_new.leaving), ",@refuse=", UpdateUtility.BuildString(_new.refuse), 
                "\n"
             });
            foreach (GuildMember member in _new.member)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "exec AddGuildCreateMember @idGuild=", _new.id, ",@servername=", UpdateUtility.BuildString(_new.server), ",@memberid=", member.memberid, ",@name=", UpdateUtility.BuildString(member.name), ",@account=", UpdateUtility.BuildString(member.account), ",@class=", member.@class, ",@point=", member.point, "\n" });
            }
            return str;
        }

        private static long CheckGuildMaster(GuildMember[] _guildmembers)
        {
            long memberid = 0L;
            bool flag = false;
            foreach (GuildMember member in _guildmembers)
            {
                if (member.@class == 0)
                {
                    if (flag)
                    {
                        throw new Exception("두 명 이상의 길드 마스터가 존재합니다.");
                    }
                    memberid = member.memberid;
                }
            }
            if (memberid == 0L)
            {
                throw new Exception("길드 마스터가 존재하지 않습니다.");
            }
            return memberid;
        }
    }
}

