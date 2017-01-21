namespace XMLDB3
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    public class AccountrefFileAdapter : FileAdapter, AccountRefAdapter
    {
        public bool AddCharacterSlot(string _account, byte _supportRewardState, long _id, string _name, string _server, byte _race, bool _supportCharacter)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.AddCharacterSlot() : 함수에 진입하였습니다");
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    AccountrefCharacter[] characterArray;
                    int num = 1;
                    if (accountref.character != null)
                    {
                        num = accountref.character.Length + 1;
                        characterArray = new AccountrefCharacter[num];
                        Array.Copy(accountref.character, characterArray, (int) (num - 1));
                    }
                    else
                    {
                        characterArray = new AccountrefCharacter[1];
                    }
                    AccountrefCharacter character = new AccountrefCharacter();
                    character.id = _id;
                    character.name = _name;
                    character.server = _server;
                    character.deleted = 0L;
                    character.groupID = 0;
                    character.race = _race;
                    character.supportCharacter = _supportCharacter;
                    characterArray[num - 1] = character;
                    accountref.character = characterArray;
                    base.WriteToDB(accountref, accountref.account);
                    return true;
                }
                WorkSession.WriteStatus("AccountrefFileAdapter.AddCharacterSlot() : 데이터 파일 [" + _account + "] 를 로드하는데 실패하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.AddCharacterSlot() : 데이터 파일 [" + _account + "] 가 존재하지 않습니다");
            }
            return false;
        }

        public bool AddPetSlot(string _account, long _id, string _name, string _server, int _remaintime, long _lasttime, long _expiretime)
        {
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    AccountrefPet[] petArray;
                    int num = 1;
                    if (accountref.pet != null)
                    {
                        num = accountref.pet.Length + 1;
                        petArray = new AccountrefPet[num];
                        Array.Copy(accountref.pet, petArray, (int) (num - 1));
                    }
                    else
                    {
                        petArray = new AccountrefPet[1];
                    }
                    AccountrefPet pet = new AccountrefPet();
                    pet.id = _id;
                    pet.name = _name;
                    pet.server = _server;
                    pet.deleted = 0L;
                    pet.remaintime = _remaintime;
                    pet.lasttime = _lasttime;
                    pet.expiretime = _expiretime;
                    pet.groupID = 0;
                    petArray[num - 1] = pet;
                    accountref.pet = petArray;
                    base.WriteToDB(accountref, accountref.account);
                    return true;
                }
                WorkSession.WriteStatus("AccountrefFileAdapter.AddPetSlot() : 데이터 파일 [" + _account + "] 를 로드하는데 실패하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.AddPetSlot() : 데이터 파일 [" + _account + "] 가 존재하지 않습니다");
            }
            return false;
        }

        public bool CacheKeyCheck(string _account, int _cacheKey, out int _oldCacheKey)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 데이터파일이 존재하는지 체크합니다");
            if (File.Exists(this.GetCacheFileName(_account)))
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 기존 키 값을 읽습니다");
                StreamReader reader = new StreamReader(this.GetCacheFileName(_account));
                _oldCacheKey = Convert.ToInt32(reader.ReadToEnd());
                reader.Close();
                WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 새 키 값을 적습니다");
                StreamWriter writer = new StreamWriter(this.GetCacheFileName(_account), false, Encoding.Unicode);
                writer.WriteLine(_cacheKey);
                writer.Close();
                return true;
            }
            WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 데이터 파일이 존재하지 않습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 데이터 파일을 생성합니다");
            _oldCacheKey = 0;
            WorkSession.WriteStatus("AccountrefFileAdapter.UpdateCacheKey() : 새 키 값을 적습니다");
            StreamWriter writer2 = new StreamWriter(this.GetCacheFileName(_account), false, Encoding.Unicode);
            writer2.WriteLine(_cacheKey);
            writer2.Close();
            return true;
        }

        public bool Create(Accountref _data)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.Create() : 함수에 진입하였습니다");
            string account = _data.account;
            WorkSession.WriteStatus("AccountrefFileAdapter.Create() : 데이터 파일 [" + account + "]가 존재하는지 확인합니다");
            if (!base.IsExistData(account))
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.Create() : 데이터 파일 [" + account + "]를 생성합니다");
                base.WriteToDB(_data, account);
                return true;
            }
            WorkSession.WriteStatus("AccountrefFileAdapter.Create() : 데이터 파일 [" + account + "]가 존재합니다. 생성에 실패하였습니다");
            return false;
        }

        private string GetCacheFileName(string _name)
        {
            return (base.Directory + _name + "_cache.xml");
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Accountref), ConfigManager.GetFileDBPath("accountref"), ".xml");
        }

        private AccountrefCharacter IsExistLogin(AccountrefCharacter[] _characters, long _id)
        {
            if (_characters != null)
            {
                foreach (AccountrefCharacter character in _characters)
                {
                    if (character.id == _id)
                    {
                        return character;
                    }
                }
            }
            return null;
        }

        private AccountrefPet IsExistLogin(AccountrefPet[] _pets, long _id)
        {
            if (_pets != null)
            {
                foreach (AccountrefPet pet in _pets)
                {
                    if (pet.id == _id)
                    {
                        return pet;
                    }
                }
            }
            return null;
        }

        public bool PlayIn(string _account, int _remainTime)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.PlayIn() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.PlayIn() : 로그를 추가한다");
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    accountref.@in = DateTime.Now;
                    accountref.playabletime = _remainTime;
                    base.WriteToDB(accountref, accountref.account);
                    return true;
                }
            }
            return false;
        }

        public bool PlayOut(string _account, int _remainTime, string _server, GroupIDList _charGroupID, GroupIDList _petGroupID, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, byte _macroCheckFailure, byte _macroCheckSuccess, bool _beginnerFlag)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.PlayOut() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.PlayOut() : 로그를 추가한다");
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    accountref.@out = DateTime.Now;
                    accountref.playabletime = _remainTime;
                    accountref.supportRace = _supportRace;
                    accountref.supportRewardState = _supportRewardState;
                    accountref.supportLastChangeTime = _supportLastChangeTime;
                    accountref.macroCheckFailure = _macroCheckFailure;
                    accountref.macroCheckSuccess = _macroCheckSuccess;
                    accountref.beginnerFlag = _beginnerFlag;
                    if (_charGroupID.group != null)
                    {
                        foreach (GroupID pid in _charGroupID.group)
                        {
                            foreach (AccountrefCharacter character in accountref.character)
                            {
                                if ((character.id == pid.charID) && (character.server == _server))
                                {
                                    character.groupID = pid.groupID;
                                }
                            }
                        }
                    }
                    if (_petGroupID.group != null)
                    {
                        foreach (GroupID pid2 in _petGroupID.group)
                        {
                            foreach (AccountrefPet pet in accountref.pet)
                            {
                                if ((pet.id == pid2.charID) && (pet.server == _server))
                                {
                                    pet.groupID = pid2.groupID;
                                }
                            }
                        }
                    }
                    base.WriteToDB(accountref, accountref.account);
                    return true;
                }
            }
            return false;
        }

        public Accountref Read(string _account)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.Read() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.Read() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.Read() : 데이터 파일 [" + _account + "]를 읽습니다");
                return (Accountref) base.ReadFromDB(_account);
            }
            WorkSession.WriteStatus("AccountrefFileAdapter.Read() : 데이터 파일 [" + _account + "]가 존재하지 않습니다. 읽기에 실패하였습니다");
            return null;
        }

        public bool RemovePetSlot(string _account, long _id, string _server)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.RemovePetSlot() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.RemovePetSlot() : 로그를 추가한다");
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    AccountrefPet[] petArray = null;
                    AccountrefPet[] pet = null;
                    if (this.IsExistLogin(accountref.pet, _id) != null)
                    {
                        petArray = new AccountrefPet[accountref.pet.Length - 1];
                        pet = accountref.pet;
                        accountref.pet = petArray;
                        int index = 0;
                        foreach (AccountrefPet pet2 in pet)
                        {
                            if ((pet2.id != _id) && (index < petArray.Length))
                            {
                                petArray[index] = pet2;
                                index++;
                            }
                        }
                        base.WriteToDB(accountref, accountref.account);
                        return true;
                    }
                }
                else
                {
                    WorkSession.WriteStatus("AccountrefFileAdapter.RemovePetSlot() : 데이터 파일 [" + _account + "] 를 로드하는데 실패하였습니다");
                }
            }
            else
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.RemovePetSlot() : 데이터 파일 [" + _account + "] 가 존재하지 않습니다");
            }
            return false;
        }

        public bool RemoveSlot(string _account, long _id, string _server)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.RemoveSlot() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.RemoveSlot() : 로그를 추가한다");
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    AccountrefCharacter[] characterArray = null;
                    AccountrefCharacter[] character = null;
                    if (this.IsExistLogin(accountref.character, _id) != null)
                    {
                        characterArray = new AccountrefCharacter[accountref.character.Length - 1];
                        character = accountref.character;
                        accountref.character = characterArray;
                        int index = 0;
                        foreach (AccountrefCharacter character2 in character)
                        {
                            if ((character2.id != _id) && (index < characterArray.Length))
                            {
                                characterArray[index] = character2;
                                index++;
                            }
                        }
                        base.WriteToDB(accountref, accountref.account);
                        return true;
                    }
                }
                else
                {
                    WorkSession.WriteStatus("AccountrefFileAdapter.RemoveSlot() : 데이터 파일 [" + _account + "] 를 로드하는데 실패하였습니다");
                }
            }
            else
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.RemoveSlot() : 데이터 파일 [" + _account + "] 가 존재하지 않습니다");
            }
            return false;
        }

        public bool SetFlag(string _account, int _flag)
        {
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    accountref.flag = _flag;
                    base.WriteToDB(accountref, _account);
                    return true;
                }
            }
            return false;
        }

        public bool SetLobbyOption(string _account, int _lobbyOption, LobbyTabList _charLobbyTabList, LobbyTabList _petLobbyTabList)
        {
            if (!base.IsExistData(_account))
            {
                return false;
            }
            Accountref accountref = (Accountref) base.ReadFromDB(_account);
            if (accountref == null)
            {
                return false;
            }
            accountref.lobbyOption = _lobbyOption;
            if (_charLobbyTabList.tabInfo != null)
            {
                foreach (LobbyTab tab in _charLobbyTabList.tabInfo)
                {
                    foreach (AccountrefCharacter character in accountref.character)
                    {
                        if ((character.server == tab.server) && (character.id == tab.charID))
                        {
                            character.tab = tab.tab;
                        }
                    }
                }
            }
            if (_petLobbyTabList.tabInfo != null)
            {
                foreach (LobbyTab tab2 in _petLobbyTabList.tabInfo)
                {
                    foreach (AccountrefPet pet in accountref.pet)
                    {
                        if ((pet.server == tab2.server) && (pet.id == tab2.charID))
                        {
                            pet.tab = tab2.tab;
                        }
                    }
                }
            }
            base.WriteToDB(accountref, _account);
            return true;
        }

        public bool SetPetSlotFlag(string _account, long _id, string _server, long _flag)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.SetPetSlotFlag() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [" + _account + "]를 읽습니다");
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    WorkSession.WriteStatus(string.Concat(new object[] { "AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [", _account, "] 안에 캐릭터 슬롯 [", _id, "] 가 있는지 검사합니다" }));
                    AccountrefPet pet = null;
                    pet = this.IsExistLogin(accountref.pet, _id);
                    if (pet != null)
                    {
                        WorkSession.WriteStatus(string.Concat(new object[] { "AccountrefFileAdapter.SetPetSlotFlag() : 캐릭터 슬롯 [", _id, "] 의 플래그 값을 ", _flag, " 로 설정합니다" }));
                        pet.deleted = _flag;
                        WorkSession.WriteStatus("AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [" + _account + "]를 저장합니다");
                        base.WriteToDB(accountref, accountref.account);
                        return true;
                    }
                    WorkSession.WriteStatus(string.Concat(new object[] { "AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [", _account, "] 안에 캐릭터 슬롯 [", _id, "] 의 데이터가 없습니다" }));
                }
                else
                {
                    WorkSession.WriteStatus("AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [" + _account + "] 를 로드하는데 실패하였습니다");
                }
            }
            else
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.SetPetSlotFlag() : 데이터 파일 [" + _account + "] 가 존재하지 않습니다");
            }
            return false;
        }

        public bool SetSlotFlag(string _account, long _id, string _server, long _flag)
        {
            WorkSession.WriteStatus("AccountrefFileAdapter.SetSlotFlag() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [" + _account + "]를 읽습니다");
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    WorkSession.WriteStatus(string.Concat(new object[] { "AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [", _account, "] 안에 캐릭터 슬롯 [", _id, "] 가 있는지 검사합니다" }));
                    AccountrefCharacter character = null;
                    character = this.IsExistLogin(accountref.character, _id);
                    if (character != null)
                    {
                        WorkSession.WriteStatus(string.Concat(new object[] { "AccountrefFileAdapter.SetSlotFlag() : 캐릭터 슬롯 [", _id, "] 의 플래그 값을 ", _flag, " 로 설정합니다" }));
                        character.deleted = _flag;
                        WorkSession.WriteStatus("AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [" + _account + "]를 저장합니다");
                        base.WriteToDB(accountref, accountref.account);
                        return true;
                    }
                    WorkSession.WriteStatus(string.Concat(new object[] { "AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [", _account, "] 안에 캐릭터 슬롯 [", _id, "] 의 데이터가 없습니다" }));
                }
                else
                {
                    WorkSession.WriteStatus("AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [" + _account + "] 를 로드하는데 실패하였습니다");
                }
            }
            else
            {
                WorkSession.WriteStatus("AccountrefFileAdapter.SetSlotFlag() : 데이터 파일 [" + _account + "] 가 존재하지 않습니다");
            }
            return false;
        }

        public bool SetSupportRewardState(string _account, byte _supportRewardState)
        {
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    accountref.supportRewardState = _supportRewardState;
                    base.WriteToDB(accountref, _account);
                    return true;
                }
            }
            return false;
        }

        public bool SetSupportState(string _account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime)
        {
            if (base.IsExistData(_account))
            {
                Accountref accountref = (Accountref) base.ReadFromDB(_account);
                if (accountref != null)
                {
                    accountref.supportRace = _supportRace;
                    accountref.supportRewardState = _supportRewardState;
                    accountref.supportLastChangeTime = _supportLastChangeTime;
                    base.WriteToDB(accountref, _account);
                    return true;
                }
            }
            return false;
        }
    }
}

