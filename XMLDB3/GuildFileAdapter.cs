namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;

    public class GuildFileAdapter : FileAdapter, GuildAdapter
    {
        public bool AddMember(long _id, GuildMember _member, string _joinmsg)
        {
            if (base.IsExistData(_id))
            {
                Guild guild = (Guild) base.ReadFromDB(_id);
                if (guild != null)
                {
                    GuildMember[] member = guild.member;
                    if (member == null)
                    {
                        guild.member = new GuildMember[] { _member };
                    }
                    else
                    {
                        guild.member = new GuildMember[member.Length + 1];
                        for (int i = 0; i < member.Length; i++)
                        {
                            guild.member[i] = member[i];
                        }
                        guild.member[member.Length] = _member;
                    }
                    return this.Write(guild);
                }
            }
            return false;
        }

        public bool AddMoney(long _id, int _iAddedMoney)
        {
            Guild guild = (Guild) base.ReadFromDB(_id);
            if (guild != null)
            {
                guild.guildmoney += _iAddedMoney;
                return this.Write(guild);
            }
            return false;
        }

        public bool AddMoney(long _id, int _iAddedMoney, ref int _remainMoney)
        {
            Guild guild = (Guild) base.ReadFromDB(_id);
            if (guild != null)
            {
                _remainMoney = guild.guildmoney += _iAddedMoney;
                return this.Write(guild);
            }
            return false;
        }

        public bool AddPoint(long _id, int _iAddedPoint)
        {
            Guild guild = (Guild) base.ReadFromDB(_id);
            if (guild != null)
            {
                guild.guildpoint += _iAddedPoint;
                return this.Write(guild);
            }
            return false;
        }

        public int ChangeGuildStone(long _idGuild, int _iType, int _iGold, int _iGP)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild == null)
            {
                return -1;
            }
            if (guild.guildmoney < _iGold)
            {
                return 1;
            }
            if (guild.guildpoint < _iGP)
            {
                return 2;
            }
            guild.guildpoint -= _iGP;
            guild.guildmoney -= _iGold;
            guild.stone.type = _iType;
            if (guild.guildmoney < guild.drawablemoney)
            {
                guild.drawablemoney = guild.guildmoney;
            }
            this.Write(guild);
            return 0;
        }

        public bool CheckMemberJointime(long _idMemeber, string _server)
        {
            return true;
        }

        public bool ClearBattleGroundType(string _server, ArrayList _GuildList)
        {
            if (_GuildList == null)
            {
                return false;
            }
            foreach (long num in _GuildList)
            {
                Guild guild = (Guild) base.ReadFromDB(num);
                if ((guild != null) && (guild.battlegroundtype != 0))
                {
                    guild.battlegroundtype = 0;
                    this.Write(guild);
                }
            }
            return true;
        }

        public bool Create(Guild _data)
        {
            if (!base.IsExistData(_data.id))
            {
                base.WriteToDB(_data, _data.id);
                return true;
            }
            return false;
        }

        public bool Delete(long _id)
        {
            return base.DeleteDB(_id.ToString());
        }

        public bool DeleteGuildRobe(long _idGuild)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                guild.robe = null;
                return this.Write(guild);
            }
            return false;
        }

        public bool DeleteGuildStone(long _idGuild)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                guild.stone = null;
                return this.Write(guild);
            }
            return false;
        }

        public DateTime GetDBCurrentTime()
        {
            return DateTime.Now;
        }

        public bool GetJoinedMemberCount(long _idGuild, DateTime _startTime, DateTime _endTime, out int _count)
        {
            _count = 1;
            return true;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Guild), ConfigManager.GetFileDBPath("guild"), ".xml");
        }

        public bool IsUsableName(string _name)
        {
            return true;
        }

        public GuildIDList LoadDeletedGuildList(string _server, DateTime _overTime)
        {
            GuildIDList list = new GuildIDList();
            list.guildID = null;
            return list;
        }

        public GuildIDList LoadGuildList(string _server)
        {
            GuildIDList list = new GuildIDList();
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if (files != null)
            {
                list.guildID = new long[files.Length];
                int index = 0;
                foreach (string str in files)
                {
                    list.guildID[index] = Convert.ToInt64(Path.GetFileNameWithoutExtension(str));
                    index++;
                }
            }
            return list;
        }

        public GuildIDList LoadGuildList(string _server, DateTime _overTime)
        {
            return this.LoadGuildList(_server);
        }

        public Guild Read(long _id)
        {
            if (base.IsExistData(_id))
            {
                return (Guild) base.ReadFromDB(_id.ToString());
            }
            return null;
        }

        public bool SetGuildStone(long _idGuild, GuildStone _guildStone)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                guild.stone = _guildStone;
                return this.Write(guild);
            }
            return false;
        }

        public bool TransferGuildMaster(long _idGuild, long _idOldMaster, long _idNewMaster)
        {
            return false;
        }

        public int UpdateBattleGroundType(long _idGuild, int _guildPoint, int _guildMoney, byte _battleGroundType)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                if (guild.battlegroundtype == _battleGroundType)
                {
                    return 3;
                }
                if (guild.guildpoint < _guildPoint)
                {
                    return 1;
                }
                if (guild.guildmoney < _guildMoney)
                {
                    return 2;
                }
                guild.guildpoint -= _guildPoint;
                guild.guildmoney -= _guildMoney;
                guild.battlegroundtype = _battleGroundType;
                if (this.Write(guild))
                {
                    return 0;
                }
            }
            return -1;
        }

        public bool UpdateBattleGroundWinnerType(long _idGuild, byte _BattleGroundWinnerType)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                guild.battlegroundwinnertype = _BattleGroundWinnerType;
                if (this.Write(guild))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateGuildProperties(long _idGuild, int _iGP, int _iMoney, int _iLevel)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild == null)
            {
                return false;
            }
            switch (_iLevel)
            {
                case -1:
                    if (guild.maxmember > 20)
                    {
                        if (guild.maxmember <= 50)
                        {
                            guild.maxmember = 20;
                        }
                        else if (guild.maxmember <= 250)
                        {
                            guild.maxmember = 50;
                        }
                        break;
                    }
                    guild.maxmember = 5;
                    break;

                case 0:
                    guild.guildmoney += _iMoney;
                    guild.guildpoint += _iGP;
                    if (guild.guildmoney < 0)
                    {
                        guild.guildmoney = 0;
                    }
                    if (guild.guildpoint < 0)
                    {
                        guild.guildpoint = 0;
                    }
                    break;

                case 1:
                    if (guild.maxmember >= 5)
                    {
                        if (guild.maxmember < 20)
                        {
                            guild.maxmember = 20;
                        }
                        else if (guild.maxmember < 50)
                        {
                            guild.maxmember = 50;
                        }
                        else if (guild.maxmember < 250)
                        {
                            guild.maxmember = 250;
                        }
                        break;
                    }
                    guild.maxmember = 5;
                    break;
            }
            this.Write(guild);
            return true;
        }

        public REPLY_RESULT UpdateGuildRobe(long _idGuild, int _guildPoint, int _guildMoney, GuildRobe _guildRobe, out byte _errorCode)
        {
            _errorCode = 0;
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                if (guild.guildpoint < _guildPoint)
                {
                    _errorCode = 1;
                    return REPLY_RESULT.FAIL;
                }
                if (guild.guildmoney < _guildMoney)
                {
                    _errorCode = 2;
                    return REPLY_RESULT.FAIL;
                }
                guild.guildpoint -= _guildPoint;
                guild.guildmoney -= _guildMoney;
                guild.robe = _guildRobe;
                if (this.Write(guild))
                {
                    return REPLY_RESULT.SUCCESS;
                }
            }
            return REPLY_RESULT.ERROR;
        }

        public bool UpdateGuildStatus(long _idGuild, byte _statusFlag, bool _set, int _guildPointRequired)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if ((guild != null) && (guild.guildpoint >= _guildPointRequired))
            {
                guild.guildpoint -= _guildPointRequired;
                if (_set)
                {
                    guild.guildstatusflag = (byte) (guild.guildstatusflag | _statusFlag);
                }
                else
                {
                    guild.guildstatusflag = (byte) (guild.guildstatusflag & ~_statusFlag);
                }
                if (this.Write(guild))
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateTitle(long _idGuild, string _strGuildTitle, bool _bUsable)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                guild.guildtitle = _strGuildTitle;
                byte num = 8;
                if (_bUsable)
                {
                    guild.guildstatusflag = (byte) (guild.guildstatusflag | num);
                }
                else
                {
                    guild.guildstatusflag = (byte) (guild.guildstatusflag & ~num);
                }
                if (this.Write(guild))
                {
                    return true;
                }
            }
            return false;
        }

        public REPLY_RESULT WithdrawDrawableMoney(long _idGuild, int _money, out int _remainMoney, out int _remainDrawableMoney)
        {
            Guild guild = (Guild) base.ReadFromDB(_idGuild);
            if (guild != null)
            {
                _remainDrawableMoney = guild.drawablemoney -= _money;
                _remainMoney = guild.guildmoney -= _money;
                if ((_remainMoney >= 0) && (_remainDrawableMoney >= 0))
                {
                    this.Write(guild);
                    return REPLY_RESULT.SUCCESS;
                }
                _remainMoney += _money;
                _remainDrawableMoney += _money;
                return REPLY_RESULT.FAIL;
            }
            _remainMoney = 0;
            _remainDrawableMoney = 0;
            return REPLY_RESULT.ERROR;
        }

        public bool WithdrawMoney(long _id, int _iWithdrawedMoney, ref int _remainMoney)
        {
            Guild guild = (Guild) base.ReadFromDB(_id);
            if (guild == null)
            {
                return false;
            }
            _remainMoney = guild.guildmoney -= _iWithdrawedMoney;
            if (_remainMoney >= 0)
            {
                return this.Write(guild);
            }
            return true;
        }

        public bool Write(Guild _data)
        {
            if (base.IsExistData(_data.id))
            {
                base.WriteToDB(_data, _data.id);
                return true;
            }
            return false;
        }
    }
}

