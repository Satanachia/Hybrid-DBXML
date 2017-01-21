namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CharacterSerializer
    {
        public static void Deserialize(CharacterInfo _character, Message _message)
        {
            _message.WriteS64(_character.id);
            _message.WriteString(_character.name);
            WriteAppearanceToMessage(_character.appearance, _message);
            WriteParameterToMessage(_character.parameter, _message);
            WriteParameterExToMessage(_character.parameterEx, _message);
            WriteTitleToMessage(_character.titles, _message);
            WriteMarriageToMessage(_character.marriage, _message);
            WriteJobToMessage(_character.job, _message);
            InventorySerializer.Deserialize(_character.inventory, _message);
            if (_character.keywords != null)
            {
                _message.WriteS32(_character.keywords.Length);
                foreach (CharacterKeyword keyword in _character.keywords)
                {
                    WriteKeywordToMessage(keyword, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
            if (_character.skills != null)
            {
                _message.WriteS32(_character.skills.Length);
                foreach (CharacterSkill skill in _character.skills)
                {
                    WriteSkillToMessage(skill, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
            WriteArbeitToMessage(_character.arbeit, _message);
            WriteConditionsToMessage(_character.conditions, _message);
            if (_character.memorys != null)
            {
                _message.WriteS32(_character.memorys.Length);
                foreach (CharacterMemory memory in _character.memorys)
                {
                    WriteMemoryToMessage(memory, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
            WriteUIToMessage(_character.data, _message);
            WriteCharQuestFromMessage(_character.@private, _message);
            WriteServiceToMessage(_character.service, _message);
            WritePVPToMessage(_character.PVP, _message);
            WriteFarmToMessage(_character.farm, _message);
            WriteHeartStickerToMessage(_character.heartSticker, _message);
            WriteJoustToMessage(_character.joust, _message);
            WriteAchievementToMessage(_character.achievements, _message);
            WriteMacroCheckerToMessage(_character.macroChecker, _message);
            WriteDonationToMessage(_character.donation, _message);
        }

        private static CharacterAchievements ReadAchievementFromMessage(Message _message)
        {
            CharacterAchievements achievements = new CharacterAchievements();
            achievements.totalscore = _message.ReadS32();
            int num = _message.ReadS32();
            if (num > 0)
            {
                achievements.achievement = new CharacterAchievementsAchievement[num];
                for (int i = 0; i < num; i++)
                {
                    CharacterAchievementsAchievement achievement = new CharacterAchievementsAchievement();
                    achievement.setid = _message.ReadS16();
                    achievement.bitflag = _message.ReadS32();
                    achievements.achievement[i] = achievement;
                }
                return achievements;
            }
            achievements.achievement = null;
            return achievements;
        }

        private static CharacterAppearance ReadAppearanceFromMessage(Message _message)
        {
            CharacterAppearance appearance = new CharacterAppearance();
            _message.ReadString();
            _message.ReadString();
            appearance.type = _message.ReadS32();
            appearance.skin_color = _message.ReadU8();
            appearance.eye_type = _message.ReadU8();
            appearance.eye_color = _message.ReadU8();
            appearance.mouth_type = _message.ReadU8();
            appearance.status = _message.ReadS32();
            appearance.height = _message.ReadFloat();
            appearance.fatness = _message.ReadFloat();
            appearance.upper = _message.ReadFloat();
            appearance.lower = _message.ReadFloat();
            appearance.region = _message.ReadS32();
            appearance.x = _message.ReadS32();
            appearance.y = _message.ReadS32();
            appearance.direction = _message.ReadU8();
            appearance.battle_state = _message.ReadS32();
            appearance.weapon_set = _message.ReadU8();
            return appearance;
        }

        private static CharacterArbeitInfo[] ReadArbeitCollectionFromMessage(Message _message)
        {
            uint num = _message.ReadU32();
            CharacterArbeitInfo[] infoArray = new CharacterArbeitInfo[num];
            for (uint i = 0; i < num; i++)
            {
                CharacterArbeitInfo info = new CharacterArbeitInfo();
                info.category = _message.ReadS16();
                info.total = _message.ReadS16();
                info.success = _message.ReadS16();
                infoArray[i] = info;
            }
            return infoArray;
        }

        private static CharacterArbeit ReadArbeitFromMessage(Message _message)
        {
            CharacterArbeit arbeit = new CharacterArbeit();
            arbeit.history = ReadArbeitHistoryFromMessage(_message);
            arbeit.collection = ReadArbeitCollectionFromMessage(_message);
            return arbeit;
        }

        private static CharacterArbeitDay[] ReadArbeitHistoryFromMessage(Message _message)
        {
            uint num = _message.ReadU32();
            CharacterArbeitDay[] dayArray = new CharacterArbeitDay[num];
            for (uint i = 0; i < num; i++)
            {
                CharacterArbeitDay day = new CharacterArbeitDay();
                day.daycount = _message.ReadS32();
                uint num3 = _message.ReadU32();
                day.info = new CharacterArbeitDayInfo[num3];
                for (uint j = 0; j < num3; j++)
                {
                    CharacterArbeitDayInfo info = new CharacterArbeitDayInfo();
                    info.category = _message.ReadS16();
                    day.info[j] = info;
                }
                dayArray[i] = day;
            }
            return dayArray;
        }

        private static CharacterInfo ReadCharacterFromMessage(Message _message)
        {
            CharacterInfo info = new CharacterInfo();
            info.id = _message.ReadS64();
            info.name = _message.ReadString();
            info.appearance = ReadAppearanceFromMessage(_message);
            info.parameter = ReadParameterFromMessage(_message);
            info.parameterEx = ReadParameterExFromMessage(_message);
            info.titles = ReadTitleFromMessage(_message);
            info.marriage = ReadMarriageFromMessage(_message);
            info.job = ReadJobFromMessage(_message);
            info.inventory = InventorySerializer.Serialize(_message);
            int num = _message.ReadS32();
            if (num > 0)
            {
                info.keywords = new CharacterKeyword[num];
                for (int i = 0; i < num; i++)
                {
                    info.keywords[i] = ReadKeywordFromMessage(_message);
                }
            }
            else
            {
                info.keywords = null;
            }
            num = _message.ReadS32();
            if (num > 0)
            {
                info.skills = new CharacterSkill[num];
                for (int j = 0; j < num; j++)
                {
                    info.skills[j] = ReadSkillFromMessage(_message);
                }
            }
            else
            {
                info.skills = null;
            }
            info.arbeit = ReadArbeitFromMessage(_message);
            info.conditions = ReadConditionFromMessage(_message);
            num = _message.ReadS32();
            if (num > 0)
            {
                info.memorys = new CharacterMemory[num];
                for (int k = 0; k < num; k++)
                {
                    info.memorys[k] = ReadMemoryFromMessage(_message);
                }
            }
            else
            {
                info.memorys = null;
            }
            info.data = ReadUIFromMessage(_message);
            info.@private = ReadCharQuestFromMessage(_message);
            info.service = ReadServiceFromMessage(_message);
            info.PVP = ReadPVPFromMessage(_message);
            info.farm = ReadFarmFromMessage(_message);
            info.heartSticker = ReadHeartStickerFromMessage(_message);
            info.joust = ReadJoustFromMessage(_message);
            info.achievements = ReadAchievementFromMessage(_message);
            info.macroChecker = ReadMacroCheckerFromMessage(_message);
            info.donation = ReadDonationFromMessage(_message);
            return info;
        }

        private static CharacterPrivate ReadCharQuestFromMessage(Message _message)
        {
            CharacterPrivate @private = new CharacterPrivate();
            @private.reserveds = ReadQuestReservedFromMessage(_message);
            @private.registereds = ReadQuestRegisteredFromMessage(_message);
            @private.books = ReadQuestBookFromMessage(_message);
            @private.npc_event_daycount = _message.ReadS32();
            @private.npc_event_bitflag = _message.ReadS64();
            return @private;
        }

        private static CharacterCondition[] ReadConditionFromMessage(Message _message)
        {
            uint num = _message.ReadU32();
            CharacterCondition[] conditionArray = new CharacterCondition[num];
            for (uint i = 0; i < num; i++)
            {
                CharacterCondition condition = new CharacterCondition();
                condition.flag = _message.ReadS8();
                condition.timemode = _message.ReadS8();
                condition.time = _message.ReadS64();
                condition.meta = _message.ReadString();
                if (condition.meta.Length == 0)
                {
                    condition.meta = null;
                }
                conditionArray[i] = condition;
            }
            return conditionArray;
        }

        private static CharacterDonation ReadDonationFromMessage(Message _message)
        {
            CharacterDonation donation = new CharacterDonation();
            donation.donationValue = _message.ReadS32();
            donation.donationUpdate = _message.ReadS64();
            return donation;
        }

        private static CharacterFarm ReadFarmFromMessage(Message _message)
        {
            CharacterFarm farm = new CharacterFarm();
            farm.farmID = _message.ReadS64();
            return farm;
        }

        private static CharacterHeartSticker ReadHeartStickerFromMessage(Message _message)
        {
            CharacterHeartSticker sticker = new CharacterHeartSticker();
            sticker.heartUpdateTime = _message.ReadS64();
            sticker.heartPoint = _message.ReadS16();
            sticker.heartTotalPoint = _message.ReadS16();
            return sticker;
        }

        private static CharacterJob ReadJobFromMessage(Message _message)
        {
            CharacterJob job = new CharacterJob();
            if (_message.CurrentFieldType == Message.MessageFieldType.VT_BYTE)
            {
                job.jobId = _message.ReadU8();
            }
            return job;
        }

        private static CharacterJoust ReadJoustFromMessage(Message _message)
        {
            CharacterJoust joust = new CharacterJoust();
            joust.joustPoint = _message.ReadS32();
            joust.joustLastWinYear = _message.ReadU8();
            joust.joustLastWinWeek = _message.ReadU8();
            joust.joustWeekWinCount = _message.ReadU8();
            joust.joustDailyWinCount = _message.ReadS16();
            joust.joustDailyLoseCount = _message.ReadS16();
            joust.joustServerWinCount = _message.ReadS16();
            joust.joustServerLoseCount = _message.ReadS16();
            return joust;
        }

        private static CharacterKeyword ReadKeywordFromMessage(Message _message)
        {
            CharacterKeyword keyword = new CharacterKeyword();
            keyword.keyword = _message.ReadS16();
            return keyword;
        }

        private static CharacterMacroChecker ReadMacroCheckerFromMessage(Message _message)
        {
            CharacterMacroChecker checker = new CharacterMacroChecker();
            checker.macroPoint = _message.ReadS32();
            return checker;
        }

        private static CharacterMarriage ReadMarriageFromMessage(Message _message)
        {
            CharacterMarriage marriage = new CharacterMarriage();
            marriage.mateid = _message.ReadS64();
            marriage.matename = _message.ReadString();
            marriage.marriagetime = _message.ReadS32();
            marriage.marriagecount = _message.ReadS16();
            return marriage;
        }

        private static CharacterMemory ReadMemoryFromMessage(Message _message)
        {
            CharacterMemory memory = new CharacterMemory();
            memory.target = _message.ReadString();
            memory.favor = _message.ReadU8();
            memory.memory = _message.ReadU8();
            memory.time_stamp = _message.ReadU8();
            return memory;
        }

        private static CharacterParameterEx ReadParameterExFromMessage(Message _message)
        {
            CharacterParameterEx ex = new CharacterParameterEx();
            ex.str_boost = _message.ReadU8();
            ex.dex_boost = _message.ReadU8();
            ex.int_boost = _message.ReadU8();
            ex.will_boost = _message.ReadU8();
            ex.luck_boost = _message.ReadU8();
            ex.height_boost = _message.ReadU8();
            ex.fatness_boost = _message.ReadU8();
            ex.upper_boost = _message.ReadU8();
            ex.lower_boost = _message.ReadU8();
            ex.life_boost = _message.ReadU8();
            ex.mana_boost = _message.ReadU8();
            ex.stamina_boost = _message.ReadU8();
            ex.toxic = _message.ReadFloat();
            ex.toxic_drunken_time = _message.ReadS64();
            ex.toxic_str = _message.ReadFloat();
            ex.toxic_int = _message.ReadFloat();
            ex.toxic_dex = _message.ReadFloat();
            ex.toxic_will = _message.ReadFloat();
            ex.toxic_luck = _message.ReadFloat();
            ex.lasttown = _message.ReadString();
            ex.lastdungeon = _message.ReadString();
            ex.exploLevel = _message.ReadS16();
            ex.exploMaxKeyLevel = _message.ReadS16();
            ex.exploCumLevel = _message.ReadS32();
            ex.exploExp = _message.ReadS64();
            ex.discoverCount = _message.ReadS32();
            return ex;
        }

        private static CharacterParameter ReadParameterFromMessage(Message _message)
        {
            CharacterParameter parameter = new CharacterParameter();
            parameter.life = _message.ReadFloat();
            parameter.life_damage = _message.ReadFloat();
            parameter.life_max = _message.ReadFloat();
            parameter.mana = _message.ReadFloat();
            parameter.mana_max = _message.ReadFloat();
            parameter.stamina = _message.ReadFloat();
            parameter.stamina_max = _message.ReadFloat();
            parameter.food = _message.ReadFloat();
            parameter.level = _message.ReadS16();
            parameter.cumulatedlevel = _message.ReadS32();
            parameter.maxlevel = _message.ReadS16();
            parameter.rebirthcount = _message.ReadS16();
            parameter.lifetimeskill = _message.ReadS16();
            parameter.experience = _message.ReadS64();
            parameter.age = _message.ReadS16();
            parameter.strength = _message.ReadFloat();
            parameter.dexterity = _message.ReadFloat();
            parameter.intelligence = _message.ReadFloat();
            parameter.will = _message.ReadFloat();
            parameter.luck = _message.ReadFloat();
            parameter.life_max_by_food = _message.ReadFloat();
            parameter.mana_max_by_food = _message.ReadFloat();
            parameter.stamina_max_by_food = _message.ReadFloat();
            parameter.strength_by_food = _message.ReadFloat();
            parameter.dexterity_by_food = _message.ReadFloat();
            parameter.intelligence_by_food = _message.ReadFloat();
            parameter.will_by_food = _message.ReadFloat();
            parameter.luck_by_food = _message.ReadFloat();
            parameter.ability_remain = _message.ReadS16();
            parameter.attack_min = _message.ReadS16();
            parameter.attack_max = _message.ReadS16();
            parameter.wattack_min = _message.ReadS16();
            parameter.wattack_max = _message.ReadS16();
            parameter.critical = _message.ReadFloat();
            parameter.protect = _message.ReadFloat();
            parameter.defense = _message.ReadS16();
            parameter.rate = _message.ReadS16();
            parameter.rank1 = _message.ReadS16();
            parameter.rank2 = _message.ReadS16();
            parameter.score = _message.ReadS64();
            return parameter;
        }

        private static CharacterPVP ReadPVPFromMessage(Message _message)
        {
            CharacterPVP rpvp = new CharacterPVP();
            rpvp.winCnt = _message.ReadS64();
            rpvp.loseCnt = _message.ReadS64();
            rpvp.penaltyPoint = _message.ReadS32();
            return rpvp;
        }

        private static CharacterPrivateBook[] ReadQuestBookFromMessage(Message _message)
        {
            uint num = _message.ReadU32();
            CharacterPrivateBook[] bookArray = new CharacterPrivateBook[num];
            for (uint i = 0; i < num; i++)
            {
                CharacterPrivateBook book = new CharacterPrivateBook();
                book.id = _message.ReadS32();
                bookArray[i] = book;
            }
            return bookArray;
        }

        private static CharacterPrivateRegistered[] ReadQuestRegisteredFromMessage(Message _message)
        {
            uint num = _message.ReadU32();
            CharacterPrivateRegistered[] registeredArray = new CharacterPrivateRegistered[num];
            for (uint i = 0; i < num; i++)
            {
                CharacterPrivateRegistered registered = new CharacterPrivateRegistered();
                registered.id = _message.ReadS32();
                registered.start = _message.ReadS64();
                registered.end = _message.ReadS64();
                registered.extra = _message.ReadS32();
                registeredArray[i] = registered;
            }
            return registeredArray;
        }

        private static CharacterPrivateReserved[] ReadQuestReservedFromMessage(Message _message)
        {
            uint num = _message.ReadU32();
            CharacterPrivateReserved[] reservedArray = new CharacterPrivateReserved[num];
            for (uint i = 0; i < num; i++)
            {
                CharacterPrivateReserved reserved = new CharacterPrivateReserved();
                reserved.id = _message.ReadS32();
                reserved.rate = _message.ReadS32();
                reservedArray[i] = reserved;
            }
            return reservedArray;
        }

        private static CharacterService ReadServiceFromMessage(Message _message)
        {
            CharacterService service = new CharacterService();
            service.nsrespawncount = _message.ReadU8();
            service.nslastrespawnday = _message.ReadS32();
            service.nsbombcount = _message.ReadU8();
            service.nsbombday = _message.ReadS32();
            service.nsgiftreceiveday = _message.ReadS32();
            service.apgiftreceiveday = _message.ReadS32();
            return service;
        }

        private static CharacterSkill ReadSkillFromMessage(Message _message)
        {
            CharacterSkill skill = new CharacterSkill();
            skill.id = _message.ReadS16();
            skill.version = _message.ReadS16();
            skill.level = _message.ReadU8();
            skill.maxlevel = _message.ReadU8();
            skill.experience = _message.ReadS32();
            skill.count = _message.ReadS16();
            skill.flag = _message.ReadS16();
            skill.subflag1 = _message.ReadS16();
            skill.subflag2 = _message.ReadS16();
            skill.subflag3 = _message.ReadS16();
            skill.subflag4 = _message.ReadS16();
            skill.subflag5 = _message.ReadS16();
            skill.subflag6 = _message.ReadS16();
            skill.subflag7 = _message.ReadS16();
            skill.subflag8 = _message.ReadS16();
            skill.subflag9 = _message.ReadS16();
            skill.lastPromotionTime = _message.ReadS64();
            skill.promotionConditionCount = _message.ReadS16();
            skill.promotionExperience = _message.ReadS32();
            return skill;
        }

        private static CharacterTitles ReadTitleFromMessage(Message _message)
        {
            CharacterTitles titles = new CharacterTitles();
            titles.selected = _message.ReadS16();
            titles.appliedtime = _message.ReadS64();
            ushort num = _message.ReadU16();
            if (num > 0)
            {
                titles.title = new CharacterTitlesTitle[num];
                for (ushort i = 0; i < num; i = (ushort) (i + 1))
                {
                    CharacterTitlesTitle title = new CharacterTitlesTitle();
                    title.id = _message.ReadS16();
                    title.state = _message.ReadU8();
                    title.validtime = _message.ReadS32();
                    titles.title[i] = title;
                }
            }
            else
            {
                titles.title = null;
            }
            titles.option = _message.ReadS16();
            return titles;
        }

        private static CharacterData ReadUIFromMessage(Message _message)
        {
            CharacterData data = new CharacterData();
            _message.ReadString();
            data.meta = _message.ReadString();
            data.nao_memory = _message.ReadS16();
            data.nao_favor = _message.ReadS16();
            data.nao_style = _message.ReadU8();
            data.birthday = new DateTime(_message.ReadS64());
            data.rebirthday = new DateTime(_message.ReadS64());
            data.rebirthage = _message.ReadS16();
            data.playtime = _message.ReadS32();
            data.wealth = _message.ReadS32();
            data.writeCounter = _message.ReadU8();
            return data;
        }

        public static CharacterInfo Serialize(Message _message)
        {
            return ReadCharacterFromMessage(_message);
        }

        private static void WriteAchievementToMessage(CharacterAchievements _achievement, Message _message)
        {
            if (_achievement == null)
            {
                _achievement = new CharacterAchievements();
            }
            _message.WriteS32(_achievement.totalscore);
            if (_achievement.achievement != null)
            {
                _message.WriteS32(_achievement.achievement.Length);
                foreach (CharacterAchievementsAchievement achievement in _achievement.achievement)
                {
                    if (achievement != null)
                    {
                        _message.WriteS16(achievement.setid);
                        _message.WriteS32(achievement.bitflag);
                    }
                    else
                    {
                        _message.WriteS16(0);
                        _message.WriteS32(0);
                    }
                }
            }
            else
            {
                _message.WriteS32(0);
            }
        }

        private static void WriteAppearanceToMessage(CharacterAppearance _appearance, Message _message)
        {
            if (_appearance == null)
            {
                _appearance = new CharacterAppearance();
            }
            _message.WriteString(string.Empty);
            _message.WriteString(string.Empty);
            _message.WriteS32(_appearance.type);
            _message.WriteU8(_appearance.skin_color);
            _message.WriteU8(_appearance.eye_type);
            _message.WriteU8(_appearance.eye_color);
            _message.WriteU8(_appearance.mouth_type);
            _message.WriteS32(_appearance.status);
            _message.WriteFloat(_appearance.height);
            _message.WriteFloat(_appearance.fatness);
            _message.WriteFloat(_appearance.upper);
            _message.WriteFloat(_appearance.lower);
            _message.WriteS32(_appearance.region);
            _message.WriteS32(_appearance.x);
            _message.WriteS32(_appearance.y);
            _message.WriteU8(_appearance.direction);
            _message.WriteS32(_appearance.battle_state);
            _message.WriteU8(_appearance.weapon_set);
        }

        private static void WriteArbeitCollectionToMessage(CharacterArbeitInfo[] _arbeitInfo, Message _message)
        {
            if (_arbeitInfo == null)
            {
                _message.WriteU32(0);
            }
            else
            {
                _message.WriteS32(_arbeitInfo.Length);
                foreach (CharacterArbeitInfo info in _arbeitInfo)
                {
                    CharacterArbeitInfo info2 = info;
                    if (info2 == null)
                    {
                        info2 = new CharacterArbeitInfo();
                    }
                    _message.WriteS16(info2.category);
                    _message.WriteS16(info2.total);
                    _message.WriteS16(info2.success);
                }
            }
        }

        private static void WriteArbeitDayInfoToMessage(CharacterArbeitDayInfo[] _arbeitDayInfo, Message _message)
        {
            if (_arbeitDayInfo == null)
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_arbeitDayInfo.Length);
                foreach (CharacterArbeitDayInfo info in _arbeitDayInfo)
                {
                    CharacterArbeitDayInfo info2 = info;
                    if (info2 == null)
                    {
                        info2 = new CharacterArbeitDayInfo();
                    }
                    _message.WriteS16(info2.category);
                }
            }
        }

        private static void WriteArbeitHistoryToMessage(CharacterArbeitDay[] _arbeitHistory, Message _message)
        {
            if (_arbeitHistory == null)
            {
                _message.WriteU32(0);
            }
            else
            {
                _message.WriteS32(_arbeitHistory.Length);
                foreach (CharacterArbeitDay day in _arbeitHistory)
                {
                    CharacterArbeitDay day2 = day;
                    if (day2 == null)
                    {
                        day2 = new CharacterArbeitDay();
                    }
                    _message.WriteS32(day2.daycount);
                    WriteArbeitDayInfoToMessage(day2.info, _message);
                }
            }
        }

        private static void WriteArbeitToMessage(CharacterArbeit _arbeit, Message _message)
        {
            if (_arbeit == null)
            {
                _arbeit = new CharacterArbeit();
            }
            WriteArbeitHistoryToMessage(_arbeit.history, _message);
            WriteArbeitCollectionToMessage(_arbeit.collection, _message);
        }

        private static void WriteCharQuestFromMessage(CharacterPrivate _private, Message _message)
        {
            WriteQuestReservedFromMessage(_private.reserveds, _message);
            WriteQuestRegisteredFromMessage(_private.registereds, _message);
            WriteQuestBookFromMessage(_private.books, _message);
            _message.WriteS32(_private.npc_event_daycount);
            _message.WriteS64(_private.npc_event_bitflag);
        }

        private static void WriteConditionsToMessage(CharacterCondition[] _condition, Message _message)
        {
            if (_condition == null)
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_condition.Length);
                foreach (CharacterCondition condition in _condition)
                {
                    CharacterCondition condition2 = condition;
                    if (condition2 == null)
                    {
                        condition2 = new CharacterCondition();
                    }
                    _message.WriteS8(condition2.flag);
                    _message.WriteS8(condition2.timemode);
                    _message.WriteS64(condition2.time);
                    if ((condition2.meta != null) && (condition2.meta.Length > 0))
                    {
                        _message.WriteString(condition2.meta);
                    }
                    else
                    {
                        _message.WriteString("");
                    }
                }
            }
        }

        private static void WriteDonationToMessage(CharacterDonation _donation, Message _message)
        {
            if (_donation == null)
            {
                _donation = new CharacterDonation();
            }
            _message.WriteS32(_donation.donationValue);
            _message.WriteS64(_donation.donationUpdate);
        }

        private static void WriteFarmToMessage(CharacterFarm _farm, Message _message)
        {
            if (_farm == null)
            {
                _farm = new CharacterFarm();
            }
            _message.WriteS64(_farm.farmID);
        }

        private static void WriteHeartStickerToMessage(CharacterHeartSticker _heartSticker, Message _message)
        {
            if (_heartSticker == null)
            {
                _heartSticker = new CharacterHeartSticker();
            }
            _message.WriteS64(_heartSticker.heartUpdateTime);
            _message.WriteS16(_heartSticker.heartPoint);
            _message.WriteS16(_heartSticker.heartTotalPoint);
        }

        private static void WriteJobToMessage(CharacterJob _job, Message _message)
        {
            if (_job == null)
            {
                _job = new CharacterJob();
                _job.jobId = 0;
            }
            _message.WriteU8(_job.jobId);
        }

        private static void WriteJoustToMessage(CharacterJoust _joust, Message _message)
        {
            if (_joust == null)
            {
                _joust = new CharacterJoust();
            }
            _message.WriteS32(_joust.joustPoint);
            _message.WriteU8(_joust.joustLastWinYear);
            _message.WriteU8(_joust.joustLastWinWeek);
            _message.WriteU8(_joust.joustWeekWinCount);
            _message.WriteS16(_joust.joustDailyWinCount);
            _message.WriteS16(_joust.joustDailyLoseCount);
            _message.WriteS16(_joust.joustServerWinCount);
            _message.WriteS16(_joust.joustServerLoseCount);
        }

        private static void WriteKeywordToMessage(CharacterKeyword _keyword, Message _message)
        {
            if (_keyword == null)
            {
                _keyword = new CharacterKeyword();
            }
            _message.WriteS16(_keyword.keyword);
        }

        private static void WriteMacroCheckerToMessage(CharacterMacroChecker _macro, Message _message)
        {
            if (_macro == null)
            {
                _macro = new CharacterMacroChecker();
            }
            _message.WriteS32(_macro.macroPoint);
        }

        private static void WriteMarriageToMessage(CharacterMarriage _marriage, Message _message)
        {
            if (_marriage == null)
            {
                _marriage = new CharacterMarriage();
                _marriage.matename = string.Empty;
            }
            _message.WriteS64(_marriage.mateid);
            _message.WriteString(_marriage.matename);
            _message.WriteS32(_marriage.marriagetime);
            _message.WriteS16(_marriage.marriagecount);
        }

        private static void WriteMemoryToMessage(CharacterMemory _memory, Message _message)
        {
            if (_memory == null)
            {
                _memory = new CharacterMemory();
            }
            _message.WriteString(_memory.target);
            _message.WriteU8(_memory.favor);
            _message.WriteU8(_memory.memory);
            _message.WriteU8(_memory.time_stamp);
        }

        private static void WriteParameterExToMessage(CharacterParameterEx _parameter, Message _message)
        {
            if (_parameter == null)
            {
                _parameter = new CharacterParameterEx();
            }
            _message.WriteU8(_parameter.str_boost);
            _message.WriteU8(_parameter.dex_boost);
            _message.WriteU8(_parameter.int_boost);
            _message.WriteU8(_parameter.will_boost);
            _message.WriteU8(_parameter.luck_boost);
            _message.WriteU8(_parameter.height_boost);
            _message.WriteU8(_parameter.fatness_boost);
            _message.WriteU8(_parameter.upper_boost);
            _message.WriteU8(_parameter.lower_boost);
            _message.WriteU8(_parameter.life_boost);
            _message.WriteU8(_parameter.mana_boost);
            _message.WriteU8(_parameter.stamina_boost);
            _message.WriteFloat(_parameter.toxic);
            _message.WriteS64(_parameter.toxic_drunken_time);
            _message.WriteFloat(_parameter.toxic_str);
            _message.WriteFloat(_parameter.toxic_int);
            _message.WriteFloat(_parameter.toxic_dex);
            _message.WriteFloat(_parameter.toxic_will);
            _message.WriteFloat(_parameter.toxic_luck);
            _message.WriteString(_parameter.lasttown);
            _message.WriteString(_parameter.lastdungeon);
            _message.WriteS16(_parameter.exploLevel);
            _message.WriteS16(_parameter.exploMaxKeyLevel);
            _message.WriteS32(_parameter.exploCumLevel);
            _message.WriteS64(_parameter.exploExp);
            _message.WriteS32(_parameter.discoverCount);
        }

        private static void WriteParameterToMessage(CharacterParameter _parameter, Message _message)
        {
            if (_parameter == null)
            {
                _parameter = new CharacterParameter();
            }
            _message.WriteFloat(_parameter.life);
            _message.WriteFloat(_parameter.life_damage);
            _message.WriteFloat(_parameter.life_max);
            _message.WriteFloat(_parameter.mana);
            _message.WriteFloat(_parameter.mana_max);
            _message.WriteFloat(_parameter.stamina);
            _message.WriteFloat(_parameter.stamina_max);
            _message.WriteFloat(_parameter.food);
            _message.WriteS16(_parameter.level);
            _message.WriteS32(_parameter.cumulatedlevel);
            _message.WriteS16(_parameter.maxlevel);
            _message.WriteS16(_parameter.rebirthcount);
            _message.WriteS16(_parameter.lifetimeskill);
            _message.WriteS64(_parameter.experience);
            _message.WriteS16(_parameter.age);
            _message.WriteFloat(_parameter.strength);
            _message.WriteFloat(_parameter.dexterity);
            _message.WriteFloat(_parameter.intelligence);
            _message.WriteFloat(_parameter.will);
            _message.WriteFloat(_parameter.luck);
            _message.WriteFloat(_parameter.life_max_by_food);
            _message.WriteFloat(_parameter.mana_max_by_food);
            _message.WriteFloat(_parameter.stamina_max_by_food);
            _message.WriteFloat(_parameter.strength_by_food);
            _message.WriteFloat(_parameter.dexterity_by_food);
            _message.WriteFloat(_parameter.intelligence_by_food);
            _message.WriteFloat(_parameter.will_by_food);
            _message.WriteFloat(_parameter.luck_by_food);
            _message.WriteS16(_parameter.ability_remain);
            _message.WriteS16(_parameter.attack_min);
            _message.WriteS16(_parameter.attack_max);
            _message.WriteS16(_parameter.wattack_min);
            _message.WriteS16(_parameter.wattack_max);
            _message.WriteFloat(_parameter.critical);
            _message.WriteFloat(_parameter.protect);
            _message.WriteS16(_parameter.defense);
            _message.WriteS16(_parameter.rate);
            _message.WriteS16(_parameter.rank1);
            _message.WriteS16(_parameter.rank2);
            _message.WriteS64(_parameter.score);
        }

        private static void WritePVPToMessage(CharacterPVP _pvp, Message _message)
        {
            if (_pvp == null)
            {
                _pvp = new CharacterPVP();
            }
            _message.WriteS64(_pvp.winCnt);
            _message.WriteS64(_pvp.loseCnt);
            _message.WriteS32(_pvp.penaltyPoint);
        }

        private static void WriteQuestBookFromMessage(CharacterPrivateBook[] _book, Message _message)
        {
            if (_book == null)
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_book.Length);
                foreach (CharacterPrivateBook book in _book)
                {
                    if (book == null)
                    {
                        _message.WriteS32(0);
                    }
                    else
                    {
                        _message.WriteS32(book.id);
                    }
                }
            }
        }

        private static void WriteQuestRegisteredFromMessage(CharacterPrivateRegistered[] _registered, Message _message)
        {
            if (_registered == null)
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_registered.Length);
                foreach (CharacterPrivateRegistered registered in _registered)
                {
                    CharacterPrivateRegistered registered2 = registered;
                    if (registered2 == null)
                    {
                        registered2 = new CharacterPrivateRegistered();
                    }
                    _message.WriteS32(registered2.id);
                    _message.WriteS64(registered2.start);
                    _message.WriteS64(registered2.end);
                    _message.WriteS32(registered2.extra);
                }
            }
        }

        private static void WriteQuestReservedFromMessage(CharacterPrivateReserved[] _reserved, Message _message)
        {
            if (_reserved == null)
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_reserved.Length);
                foreach (CharacterPrivateReserved reserved in _reserved)
                {
                    CharacterPrivateReserved reserved2 = reserved;
                    if (reserved2 == null)
                    {
                        reserved2 = new CharacterPrivateReserved();
                    }
                    _message.WriteS32(reserved2.id);
                    _message.WriteS32(reserved2.rate);
                }
            }
        }

        private static void WriteServiceToMessage(CharacterService _service, Message _message)
        {
            if (_service == null)
            {
                _service = new CharacterService();
            }
            _message.WriteU8(_service.nsrespawncount);
            _message.WriteS32(_service.nslastrespawnday);
            _message.WriteU8(_service.nsbombcount);
            _message.WriteS32(_service.nsbombday);
            _message.WriteS32(_service.nsgiftreceiveday);
            _message.WriteS32(_service.apgiftreceiveday);
        }

        private static void WriteSkillToMessage(CharacterSkill _skill, Message _message)
        {
            if (_skill == null)
            {
                _skill = new CharacterSkill();
            }
            _message.WriteS16(_skill.id);
            _message.WriteS16(_skill.version);
            _message.WriteU8(_skill.level);
            _message.WriteU8(_skill.maxlevel);
            _message.WriteS32(_skill.experience);
            _message.WriteS16(_skill.count);
            _message.WriteS16(_skill.flag);
            _message.WriteS16(_skill.subflag1);
            _message.WriteS16(_skill.subflag2);
            _message.WriteS16(_skill.subflag3);
            _message.WriteS16(_skill.subflag4);
            _message.WriteS16(_skill.subflag5);
            _message.WriteS16(_skill.subflag6);
            _message.WriteS16(_skill.subflag7);
            _message.WriteS16(_skill.subflag8);
            _message.WriteS16(_skill.subflag9);
            _message.WriteS64(_skill.lastPromotionTime);
            _message.WriteS16(_skill.promotionConditionCount);
            _message.WriteS32(_skill.promotionExperience);
        }

        private static void WriteTitleToMessage(CharacterTitles _title, Message _message)
        {
            if (_title == null)
            {
                _title = new CharacterTitles();
            }
            _message.WriteS16(_title.selected);
            _message.WriteS64(_title.appliedtime);
            if (_title.title != null)
            {
                _message.WriteU16((ushort) _title.title.Length);
                foreach (CharacterTitlesTitle title in _title.title)
                {
                    if (title != null)
                    {
                        _message.WriteS16(title.id);
                        _message.WriteU8(title.state);
                        _message.WriteS32(title.validtime);
                    }
                    else
                    {
                        _message.WriteS16(0);
                        _message.WriteU8(0);
                        _message.WriteS32(0);
                    }
                }
            }
            else
            {
                _message.WriteU16(0);
            }
            _message.WriteS16(_title.option);
        }

        private static void WriteUIToMessage(CharacterData _data, Message _message)
        {
            if (_data == null)
            {
                _data = new CharacterData();
                _data.meta = string.Empty;
            }
            _message.WriteString(string.Empty);
            _message.WriteString(_data.meta);
            _message.WriteS16(_data.nao_memory);
            _message.WriteS16(_data.nao_favor);
            _message.WriteU8(_data.nao_style);
            _message.WriteS64(_data.birthday.Ticks);
            _message.WriteS64(_data.rebirthday.Ticks);
            _message.WriteS16(_data.rebirthage);
            _message.WriteS32(_data.playtime);
            _message.WriteS32(_data.wealth);
            _message.WriteU8(_data.writeCounter);
        }
    }
}

