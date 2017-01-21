namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public class CharacterFileAdapter : FileAdapter, CharacterAdapter
    {
        private bool Create(CharacterInfo _character)
        {
            if (!base.IsExistData(_character.id))
            {
                base.WriteToDB(_character, _character.id);
                return true;
            }
            return false;
        }

        public bool CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo _character, AccountRefAdapter _accountref, BankAdapter _bank, WebSynchAdapter _websynch)
        {
            if (this.Create(_character))
            {
                AccountrefFileAdapter adapter = _accountref as AccountrefFileAdapter;
                adapter.AddCharacterSlot(_account, _supportRewardState, _character.id, _character.name, _server, _race, _supportCharacter);
                adapter.SetSupportRewardState(_account, _supportRewardState);
                ((BankFileAdapter) _bank).AddSlot(_account, _character.name, (BankRace) _race);
                return true;
            }
            return false;
        }

        public bool Delete(long _id)
        {
            return true;
        }

        public bool Delete(string _name)
        {
            return base.DeleteDB(_name);
        }

        public bool DeleteEx(string _account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string _server, long _idcharacter, AccountRefAdapter _accountref, BankAdapter _bank, GuildAdapter _guild, WebSynchAdapter _websynch)
        {
            if (this.Delete(_idcharacter))
            {
                ((BankFileAdapter) _bank).RemoveSlot(_account, _idcharacter);
                AccountrefFileAdapter adapter = _accountref as AccountrefFileAdapter;
                adapter.RemoveSlot(_account, _idcharacter, _server);
                adapter.SetSupportState(_account, _supportRace, _supportRewardState, _supportLastChangeTime);
                return true;
            }
            return false;
        }

        public bool DeleteItem(long _id, ItemList[] _list)
        {
            CharacterInfo info = this.Read(_id, null);
            if (info == null)
            {
                return false;
            }
            if ((info.inventory != null) && (info.inventory.Count > 0))
            {
                foreach (ItemList list in _list)
                {
                    info.inventory.Remove(list.itemID);
                }
                base.WriteToDB(info, _id);
            }
            return true;
        }

        public uint GetAccumLevel(string _account, string _name)
        {
            return 0;
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            if (base.IsExistData(_id))
            {
                CharacterInfo info = (CharacterInfo) base.ReadFromDB(_id);
                _counter = (info.data != null) ? info.data.writeCounter : ((byte) 0);
                return true;
            }
            _counter = 0;
            return false;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(CharacterInfo), ConfigManager.GetFileDBPath("character"), ".xml");
        }

        public bool IsUsableName(string _name, string _account)
        {
            return true;
        }

        public CharacterInfo Read(long _id, CharacterInfo _cache)
        {
            if (base.IsExistData(_id))
            {
                return (CharacterInfo) base.ReadFromDB(_id);
            }
            return null;
        }

        public bool RemoveReservedCharName(string _name, string _account)
        {
            return true;
        }

        public bool Write(CharacterInfo _character, CharacterInfo _cache)
        {
            if (base.IsExistData(_character.id))
            {
                base.WriteToDB(_character, _character.id);
                return true;
            }
            return false;
        }
    }
}

