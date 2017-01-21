namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PetSerializer
    {
        public static void Deserialize(PetInfo _info, Message _message)
        {
            WritePetToMessage(_info, _message);
        }

        private static PetAppearance ReadAppearanceFromMessage(Message _message)
        {
            PetAppearance appearance = new PetAppearance();
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
            appearance.extra_01 = _message.ReadS32();
            appearance.extra_02 = _message.ReadS32();
            appearance.extra_03 = _message.ReadS32();
            return appearance;
        }

        private static PetCondition[] ReadConditionFromMessage(Message _message)
        {
            int num = _message.ReadS32();
            if (num <= 0)
            {
                return null;
            }
            PetCondition[] conditionArray = new PetCondition[num];
            for (int i = 0; i < num; i++)
            {
                PetCondition condition = new PetCondition();
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

        private static PetData ReadDataFromMessage(Message _message)
        {
            PetData data = new PetData();
            data.ui = _message.ReadString();
            data.meta = _message.ReadString();
            data.birthday = new DateTime(_message.ReadS64());
            data.rebirthday = new DateTime(_message.ReadS64());
            data.rebirthage = _message.ReadS16();
            data.playtime = _message.ReadS32();
            data.wealth = _message.ReadS32();
            data.writeCounter = _message.ReadU8();
            return data;
        }

        private static PetMacroChecker ReadMacroCheckerFromMessage(Message _message)
        {
            PetMacroChecker checker = new PetMacroChecker();
            checker.macroPoint = _message.ReadS32();
            return checker;
        }

        private static PetMemory ReadMemoryFromMessage(Message _message)
        {
            PetMemory memory = new PetMemory();
            memory.target = _message.ReadString();
            memory.favor = _message.ReadU8();
            memory.memory = _message.ReadU8();
            memory.time_stamp = _message.ReadU8();
            return memory;
        }

        private static PetParameterEx ReadParameterExFromMessage(Message _message)
        {
            PetParameterEx ex = new PetParameterEx();
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
            return ex;
        }

        private static PetParameter ReadParameterFromMessage(Message _message)
        {
            PetParameter parameter = new PetParameter();
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
            parameter.experience = _message.ReadS64();
            parameter.age = _message.ReadS16();
            parameter.strength = _message.ReadFloat();
            parameter.dexterity = _message.ReadFloat();
            parameter.intelligence = _message.ReadFloat();
            parameter.will = _message.ReadFloat();
            parameter.luck = _message.ReadFloat();
            parameter.attack_min = _message.ReadS16();
            parameter.attack_max = _message.ReadS16();
            parameter.wattack_min = _message.ReadS16();
            parameter.wattack_max = _message.ReadS16();
            parameter.critical = _message.ReadFloat();
            parameter.protect = _message.ReadFloat();
            parameter.defense = _message.ReadS16();
            parameter.rate = _message.ReadS16();
            return parameter;
        }

        private static PetPrivate ReadQuestFromMessage(Message _message)
        {
            PetPrivate @private = new PetPrivate();
            @private.reserveds = ReadQuestReservedFromMessage(_message);
            @private.registereds = ReadQuestRegisteredFromMessage(_message);
            return @private;
        }

        private static PetPrivateRegistered[] ReadQuestRegisteredFromMessage(Message _message)
        {
            int num = _message.ReadS32();
            if (num <= 0)
            {
                return null;
            }
            PetPrivateRegistered[] registeredArray = new PetPrivateRegistered[num];
            for (int i = 0; i < num; i++)
            {
                PetPrivateRegistered registered = new PetPrivateRegistered();
                registered.id = _message.ReadS32();
                registered.start = _message.ReadS64();
                registered.end = _message.ReadS64();
                registered.extra = _message.ReadS32();
                registeredArray[i] = registered;
            }
            return registeredArray;
        }

        private static PetPrivateReserved[] ReadQuestReservedFromMessage(Message _message)
        {
            int num = _message.ReadS32();
            if (num <= 0)
            {
                return null;
            }
            PetPrivateReserved[] reservedArray = new PetPrivateReserved[num];
            for (int i = 0; i < num; i++)
            {
                PetPrivateReserved reserved = new PetPrivateReserved();
                reserved.id = _message.ReadS32();
                reserved.rate = _message.ReadS32();
                reservedArray[i] = reserved;
            }
            return reservedArray;
        }

        private static PetSkill ReadSkillFromMessage(Message _message)
        {
            PetSkill skill = new PetSkill();
            skill.id = _message.ReadS16();
            skill.level = _message.ReadU8();
            skill.flag = _message.ReadS16();
            return skill;
        }

        private static PetSummon ReadSummonFromMessage(Message _message)
        {
            PetSummon summon = new PetSummon();
            summon.remaintime = _message.ReadS32();
            summon.lasttime = _message.ReadS64();
            summon.expiretime = _message.ReadS64();
            summon.loyalty = _message.ReadU8();
            summon.favor = _message.ReadU8();
            return summon;
        }

        public static PetInfo Serialize(Message _message)
        {
            PetInfo info = new PetInfo();
            info.id = _message.ReadS64();
            info.name = _message.ReadString();
            info.appearance = ReadAppearanceFromMessage(_message);
            info.parameter = ReadParameterFromMessage(_message);
            info.parameterEx = ReadParameterExFromMessage(_message);
            info.inventory = InventorySerializer.Serialize(_message);
            int num = _message.ReadS32();
            if (num > 0)
            {
                info.skills = new PetSkill[num];
                for (int i = 0; i < num; i++)
                {
                    info.skills[i] = ReadSkillFromMessage(_message);
                }
            }
            else
            {
                info.skills = null;
            }
            info.conditions = ReadConditionFromMessage(_message);
            num = _message.ReadS32();
            if (num > 0)
            {
                info.memorys = new PetMemory[num];
                for (int j = 0; j < num; j++)
                {
                    info.memorys[j] = ReadMemoryFromMessage(_message);
                }
            }
            else
            {
                info.memorys = null;
            }
            info.data = ReadDataFromMessage(_message);
            info.@private = ReadQuestFromMessage(_message);
            info.summon = ReadSummonFromMessage(_message);
            info.macroChecker = ReadMacroCheckerFromMessage(_message);
            return info;
        }

        private static void WriteAppearanceToMessage(PetAppearance _appearance, Message _message)
        {
            if (_appearance == null)
            {
                _appearance = new PetAppearance();
            }
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
            _message.WriteS32(_appearance.extra_01);
            _message.WriteS32(_appearance.extra_02);
            _message.WriteS32(_appearance.extra_03);
        }

        private static void WriteConditionsToMessage(PetCondition[] _condition, Message _message)
        {
            if (_condition == null)
            {
                _message.WriteU32(0);
            }
            else
            {
                _message.WriteS32(_condition.Length);
                foreach (PetCondition condition in _condition)
                {
                    _message.WriteS8(condition.flag);
                    _message.WriteS8(condition.timemode);
                    _message.WriteS64(condition.time);
                    if ((condition.meta != null) && (condition.meta.Length > 0))
                    {
                        _message.WriteString(condition.meta);
                    }
                    else
                    {
                        _message.WriteString("");
                    }
                }
            }
        }

        private static void WriteDataToMessage(PetData _data, Message _message)
        {
            if (_data == null)
            {
                _data = new PetData();
            }
            _message.WriteString(_data.ui);
            _message.WriteString(_data.meta);
            _message.WriteS64(_data.birthday.Ticks);
            _message.WriteS64(_data.rebirthday.Ticks);
            _message.WriteS16(_data.rebirthage);
            _message.WriteS32(_data.playtime);
            _message.WriteS32(_data.wealth);
            _message.WriteU8(_data.writeCounter);
        }

        private static void WriteMacroCheckerToMessage(PetMacroChecker _summon, Message _message)
        {
            if (_summon == null)
            {
                _summon = new PetMacroChecker();
            }
            _message.WriteS32(_summon.macroPoint);
        }

        private static void WriteMemoryToMessage(PetMemory _memory, Message _message)
        {
            if (_memory == null)
            {
                _memory = new PetMemory();
            }
            _message.WriteString(_memory.target);
            _message.WriteU8(_memory.favor);
            _message.WriteU8(_memory.memory);
            _message.WriteU8(_memory.time_stamp);
        }

        private static void WriteParameterExToMessage(PetParameterEx _parameterEx, Message _message)
        {
            if (_parameterEx == null)
            {
                _parameterEx = new PetParameterEx();
            }
            _message.WriteU8(_parameterEx.str_boost);
            _message.WriteU8(_parameterEx.dex_boost);
            _message.WriteU8(_parameterEx.int_boost);
            _message.WriteU8(_parameterEx.will_boost);
            _message.WriteU8(_parameterEx.luck_boost);
            _message.WriteU8(_parameterEx.height_boost);
            _message.WriteU8(_parameterEx.fatness_boost);
            _message.WriteU8(_parameterEx.upper_boost);
            _message.WriteU8(_parameterEx.lower_boost);
            _message.WriteU8(_parameterEx.life_boost);
            _message.WriteU8(_parameterEx.mana_boost);
            _message.WriteU8(_parameterEx.stamina_boost);
            _message.WriteFloat(_parameterEx.toxic);
            _message.WriteS64(_parameterEx.toxic_drunken_time);
            _message.WriteFloat(_parameterEx.toxic_str);
            _message.WriteFloat(_parameterEx.toxic_int);
            _message.WriteFloat(_parameterEx.toxic_dex);
            _message.WriteFloat(_parameterEx.toxic_will);
            _message.WriteFloat(_parameterEx.toxic_luck);
            _message.WriteString(_parameterEx.lasttown);
            _message.WriteString(_parameterEx.lastdungeon);
        }

        private static void WriteParameterToMessage(PetParameter _parameter, Message _message)
        {
            if (_parameter == null)
            {
                _parameter = new PetParameter();
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
            _message.WriteS64(_parameter.experience);
            _message.WriteS16(_parameter.age);
            _message.WriteFloat(_parameter.strength);
            _message.WriteFloat(_parameter.dexterity);
            _message.WriteFloat(_parameter.intelligence);
            _message.WriteFloat(_parameter.will);
            _message.WriteFloat(_parameter.luck);
            _message.WriteS16(_parameter.attack_min);
            _message.WriteS16(_parameter.attack_max);
            _message.WriteS16(_parameter.wattack_min);
            _message.WriteS16(_parameter.wattack_max);
            _message.WriteFloat(_parameter.critical);
            _message.WriteFloat(_parameter.protect);
            _message.WriteS16(_parameter.defense);
            _message.WriteS16(_parameter.rate);
        }

        private static void WritePetToMessage(PetInfo _pet, Message _message)
        {
            _message.WriteS64(_pet.id);
            _message.WriteString(_pet.name);
            WriteAppearanceToMessage(_pet.appearance, _message);
            WriteParameterToMessage(_pet.parameter, _message);
            WriteParameterExToMessage(_pet.parameterEx, _message);
            InventorySerializer.Deserialize(_pet.inventory, _message);
            if (_pet.skills != null)
            {
                _message.WriteS32(_pet.skills.Length);
                foreach (PetSkill skill in _pet.skills)
                {
                    WriteSkillToMessage(skill, _message);
                }
            }
            else
            {
                _message.WriteU32(0);
            }
            WriteConditionsToMessage(_pet.conditions, _message);
            if (_pet.memorys != null)
            {
                _message.WriteS32(_pet.memorys.Length);
                foreach (PetMemory memory in _pet.memorys)
                {
                    WriteMemoryToMessage(memory, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
            WriteDataToMessage(_pet.data, _message);
            WriteQuestToMessage(_pet.@private, _message);
            WriteSummonToMessage(_pet.summon, _message);
            WriteMacroCheckerToMessage(_pet.macroChecker, _message);
        }

        private static void WriteQuestRegisteredFromMessage(PetPrivateRegistered[] _registereds, Message _message)
        {
            if (_registereds == null)
            {
                _message.WriteU32(0);
            }
            else
            {
                _message.WriteS32(_registereds.Length);
                foreach (PetPrivateRegistered registered in _registereds)
                {
                    PetPrivateRegistered registered2 = registered;
                    if (registered2 == null)
                    {
                        registered2 = new PetPrivateRegistered();
                    }
                    _message.WriteS32(registered2.id);
                    _message.WriteS64(registered2.start);
                    _message.WriteS64(registered2.end);
                    _message.WriteS32(registered2.extra);
                }
            }
        }

        private static void WriteQuestReservedFromMessage(PetPrivateReserved[] _reserveds, Message _message)
        {
            if (_reserveds == null)
            {
                _message.WriteU32(0);
            }
            else
            {
                _message.WriteS32(_reserveds.Length);
                foreach (PetPrivateReserved reserved in _reserveds)
                {
                    PetPrivateReserved reserved2 = reserved;
                    if (reserved2 == null)
                    {
                        reserved2 = new PetPrivateReserved();
                    }
                    _message.WriteS32(reserved2.id);
                    _message.WriteS32(reserved2.rate);
                }
            }
        }

        private static void WriteQuestToMessage(PetPrivate _private, Message _message)
        {
            if (_private == null)
            {
                _private = new PetPrivate();
            }
            WriteQuestReservedFromMessage(_private.reserveds, _message);
            WriteQuestRegisteredFromMessage(_private.registereds, _message);
        }

        private static void WriteSkillToMessage(PetSkill _skill, Message _message)
        {
            if (_skill == null)
            {
                _skill = new PetSkill();
            }
            _message.WriteS16(_skill.id);
            _message.WriteU8(_skill.level);
            _message.WriteS16(_skill.flag);
        }

        private static void WriteSummonToMessage(PetSummon _summon, Message _message)
        {
            if (_summon == null)
            {
                _summon = new PetSummon();
            }
            _message.WriteS32(_summon.remaintime);
            _message.WriteS64(_summon.lasttime);
            _message.WriteS64(_summon.expiretime);
            _message.WriteU8(_summon.loyalty);
            _message.WriteU8(_summon.favor);
        }
    }
}

