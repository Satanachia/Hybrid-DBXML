﻿namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildSerializer
    {
        public static void Deserialize(Guild _guild, Message _messsage)
        {
            if (_guild == null)
            {
                _guild = new Guild();
            }
            _messsage.WriteS64(_guild.id);
            _messsage.WriteString(_guild.name);
            _messsage.WriteString(_guild.server);
            _messsage.WriteS32(_guild.guildmoney);
            _messsage.WriteS32(_guild.drawablemoney);
            _messsage.WriteS64(_guild.drawabledate.Ticks);
            _messsage.WriteS32(_guild.guildpoint);
            _messsage.WriteS32(_guild.guildtype);
            _messsage.WriteS32(_guild.jointype);
            _messsage.WriteS32(_guild.maxmember);
            _messsage.WriteString(_guild.profile);
            _messsage.WriteString(_guild.greeting);
            _messsage.WriteString(_guild.leaving);
            _messsage.WriteString(_guild.refuse);
            _messsage.WriteS64(_guild.expiration.Ticks);
            _messsage.WriteU8(_guild.enable);
            _messsage.WriteU8(_guild.battlegroundtype);
            _messsage.WriteU8(_guild.battlegroundwinnertype);
            _messsage.WriteU8(_guild.guildstatusflag);
            if (_guild.stone != null)
            {
                _messsage.WriteU8(1);
                GuildStoneSerializer.Deserialize(_guild.stone, _messsage);
            }
            else
            {
                _messsage.WriteU8(0);
            }
            if (_guild.member != null)
            {
                _messsage.WriteS32(_guild.member.Length);
                foreach (GuildMember member in _guild.member)
                {
                    WriteGuildMemberToMsg(member, _messsage);
                }
            }
            else
            {
                _messsage.WriteS32(0);
            }
            if (_guild.robe != null)
            {
                _messsage.WriteU8(1);
                GuildRobeSerializer.Deserialize(_guild.robe, _messsage);
            }
            else
            {
                _messsage.WriteU8(0);
            }
            _messsage.WriteString(_guild.guildtitle);
        }

        public static GuildMember ReadGuildMemberFromMsg(Message _message)
        {
            GuildMember member = new GuildMember();
            member.memberid = _message.ReadS64();
            member.name = _message.ReadString();
            member.account = _message.ReadString();
            member.@class = _message.ReadS32();
            member.point = _message.ReadS32();
            return member;
        }

        public static Guild Serialize(Message _message)
        {
            Guild guild = new Guild();
            guild.id = _message.ReadS64();
            guild.name = _message.ReadString();
            guild.server = _message.ReadString();
            guild.guildmoney = _message.ReadS32();
            guild.drawablemoney = _message.ReadS32();
            guild.drawabledate = new DateTime(_message.ReadS64());
            guild.guildpoint = _message.ReadS32();
            guild.guildtype = _message.ReadS32();
            guild.jointype = _message.ReadS32();
            guild.maxmember = _message.ReadS32();
            guild.profile = _message.ReadString();
            guild.greeting = _message.ReadString();
            guild.leaving = _message.ReadString();
            guild.refuse = _message.ReadString();
            guild.expiration = new DateTime(_message.ReadS64());
            guild.enable = _message.ReadU8();
            guild.battlegroundtype = _message.ReadU8();
            guild.battlegroundwinnertype = _message.ReadU8();
            guild.guildstatusflag = _message.ReadU8();
            if (_message.ReadU8() == 1)
            {
                guild.stone = GuildStoneSerializer.Serialize(_message);
            }
            int num = _message.ReadS32();
            if (num > 0)
            {
                guild.member = new GuildMember[num];
                for (int i = 0; i < num; i++)
                {
                    guild.member[i] = ReadGuildMemberFromMsg(_message);
                }
            }
            else
            {
                guild.member = null;
            }
            if (_message.ReadU8() == 1)
            {
                guild.robe = GuildRobeSerializer.Serialize(_message);
            }
            guild.guildtitle = _message.ReadString();
            return guild;
        }

        private static void WriteGuildMemberToMsg(GuildMember _member, Message _messsage)
        {
            if (_member == null)
            {
                _member = new GuildMember();
            }
            _messsage.WriteS64(_member.memberid);
            _messsage.WriteString(_member.name);
            _messsage.WriteString(_member.account);
            _messsage.WriteS32(_member.@class);
            _messsage.WriteS32(_member.point);
        }
    }
}

