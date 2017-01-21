namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public interface GuildAdapter
    {
        bool AddMember(long _id, GuildMember _member, string _joinmsg);
        bool AddMoney(long _id, int _iAddedMoney);
        bool AddPoint(long _id, int _iAddedPoint);
        int ChangeGuildStone(long _idGuild, int _iType, int _iGold, int _iGP);
        bool CheckMemberJointime(long _idMember, string _server);
        bool ClearBattleGroundType(string _server, ArrayList _GuildList);
        bool Create(Guild _data);
        bool Delete(long _id);
        bool DeleteGuildRobe(long _idGuild);
        bool DeleteGuildStone(long _idGuild);
        DateTime GetDBCurrentTime();
        bool GetJoinedMemberCount(long _idGuild, DateTime _startTime, DateTime _endTime, out int _count);
        void Initialize(string _argument);
        bool IsUsableName(string _name);
        GuildIDList LoadDeletedGuildList(string _server, DateTime _overTime);
        GuildIDList LoadGuildList(string _server, DateTime _overTime);
        Guild Read(long _id);
        bool SetGuildStone(long _idGuild, GuildStone _guildStone);
        bool TransferGuildMaster(long _idGuild, long _idOldMaster, long _idNewMaster);
        int UpdateBattleGroundType(long _idGuild, int _guildPoint, int _guildMoney, byte _battleGroundType);
        bool UpdateBattleGroundWinnerType(long _idGuild, byte _BattleGroundWinnerType);
        bool UpdateGuildProperties(long _idGuild, int _iGP, int _iMoney, int _iLevel);
        REPLY_RESULT UpdateGuildRobe(long _idGuild, int _guildPoint, int _guildMoney, GuildRobe _guildRobe, out byte _errorCode);
        bool UpdateGuildStatus(long _idGuild, byte _statusFlag, bool _set, int _guildPointRequired);
        bool UpdateTitle(long _idGuild, string _strGuildTitle, bool _bUsable);
        REPLY_RESULT WithdrawDrawableMoney(long _idGuild, int _money, out int _remainMoney, out int _remainDrawableMoney);
        bool Write(Guild _data);
    }
}

