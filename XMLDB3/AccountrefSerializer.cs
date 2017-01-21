namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountrefSerializer
    {
        public static void Deserialize(Accountref _accountref, Message _message)
        {
            if (_accountref == null)
            {
                _accountref = new Accountref();
            }
            _message.WriteString(_accountref.account);
            _message.WriteS32(_accountref.flag);
            _message.WriteS16(_accountref.maxslot);
            _message.WriteS64(_accountref.@in.Ticks);
            _message.WriteS64(_accountref.@out.Ticks);
            _message.WriteS32(_accountref.playabletime);
            _message.WriteS32(_accountref.supportLastChangeTime);
            _message.WriteU8(_accountref.supportRace);
            _message.WriteU8(_accountref.supportRewardState);
            _message.WriteS32(_accountref.lobbyOption);
            if (_accountref.character != null)
            {
                _message.WriteS16((short) _accountref.character.Length);
                foreach (AccountrefCharacter character in _accountref.character)
                {
                    _message.WriteS64(character.id);
                    _message.WriteString(character.name);
                    _message.WriteString(character.server);
                    _message.WriteS64(character.deleted);
                    _message.WriteU8(character.groupID);
                    _message.WriteU8(character.race);
                    _message.WriteU8(character.supportCharacter ? ((byte) 1) : ((byte) 0));
                    _message.WriteU8(character.tab ? ((byte) 1) : ((byte) 0));
                }
            }
            else
            {
                _message.WriteS16(0);
            }
            if (_accountref.pet != null)
            {
                _message.WriteS16((short) _accountref.pet.Length);
                foreach (AccountrefPet pet in _accountref.pet)
                {
                    _message.WriteS64(pet.id);
                    _message.WriteString(pet.name);
                    _message.WriteString(pet.server);
                    _message.WriteS64(pet.deleted);
                    _message.WriteS32(pet.remaintime);
                    _message.WriteS64(pet.lasttime);
                    _message.WriteS64(pet.expiretime);
                    _message.WriteU8(pet.groupID);
                    _message.WriteU8(pet.tab ? ((byte) 1) : ((byte) 0));
                }
            }
            else
            {
                _message.WriteS16(0);
            }
            _message.WriteU8(_accountref.macroCheckFailure);
            _message.WriteU8(_accountref.macroCheckSuccess);
            _message.WriteU8(_accountref.beginnerFlag ? ((byte) 1) : ((byte) 0));
        }

        public static Accountref Serialize(Message _message)
        {
            Accountref accountref = new Accountref();
            accountref.account = _message.ReadString();
            accountref.flag = _message.ReadS32();
            accountref.maxslot = 0;
            accountref.@in = new DateTime(_message.ReadS64());
            accountref.@out = new DateTime(_message.ReadS64());
            accountref.playabletime = _message.ReadS32();
            accountref.supportLastChangeTime = _message.ReadS32();
            accountref.supportRace = _message.ReadU8();
            accountref.supportRewardState = _message.ReadU8();
            accountref.lobbyOption = _message.ReadS32();
            int num = _message.ReadS16();
            accountref.character = new AccountrefCharacter[num];
            for (int i = 0; i < num; i++)
            {
                accountref.character[i] = new AccountrefCharacter();
                accountref.character[i].id = (long) _message.ReadU64();
                accountref.character[i].name = _message.ReadString();
                accountref.character[i].server = _message.ReadString();
                accountref.character[i].deleted = (long) _message.ReadU64();
                accountref.character[i].groupID = _message.ReadU8();
                accountref.character[i].race = _message.ReadU8();
                accountref.character[i].supportCharacter = _message.ReadU8() != 0;
                accountref.character[i].tab = _message.ReadU8() != 0;
            }
            num = _message.ReadS16();
            accountref.pet = new AccountrefPet[num];
            for (int j = 0; j < num; j++)
            {
                accountref.pet[j] = new AccountrefPet();
                accountref.pet[j].id = (long) _message.ReadU64();
                accountref.pet[j].name = _message.ReadString();
                accountref.pet[j].server = _message.ReadString();
                accountref.pet[j].deleted = _message.ReadS64();
                accountref.pet[j].remaintime = (int) _message.ReadU32();
                accountref.pet[j].lasttime = (long) _message.ReadU64();
                accountref.pet[j].expiretime = (long) _message.ReadU64();
                accountref.pet[j].groupID = _message.ReadU8();
                accountref.pet[j].tab = _message.ReadU8() != 0;
            }
            accountref.macroCheckFailure = _message.ReadU8();
            accountref.macroCheckSuccess = _message.ReadU8();
            accountref.beginnerFlag = _message.ReadU8() != 0;
            return accountref;
        }
    }
}

