namespace XMLDB3
{
    using System;
    using System.Collections;

    public class BankFileAdapter : FileAdapter, BankAdapter
    {
        private BankSlotComparer slotComparer;

        private Bank _Read(string _id)
        {
            if (base.IsExistData(_id))
            {
                return (Bank) base.ReadFromDB(_id);
            }
            return null;
        }

        public bool AddSlot(string _account, string _name, BankRace _race)
        {
            Bank bank = null;
            this.CreateBank(_account);
            bank = (Bank) base.ReadFromDB(_account);
            if (bank == null)
            {
                return false;
            }
            if (bank.slot == null)
            {
                bank.slot = new ArrayList();
            }
            bank.slot.Add(new BankSlot(_name, _race));
            base.WriteToDB(bank, _account);
            return true;
        }

        public bool Create(Bank _data)
        {
            if (!base.IsExistData(_data.account))
            {
                base.WriteToDB(_data, _data.account);
                return true;
            }
            return false;
        }

        public bool CreateBank(string _account)
        {
            Bank bank = null;
            if (!base.IsExistData(_account))
            {
                bank = new Bank();
                bank.account = _account;
                bank.data = new BankData();
                bank.data.deposit = 0;
                bank.data.password = string.Empty;
                bank.humanWealth = 0;
                bank.elfWealth = 0;
                bank.giantWealth = 0;
                return this.Create(bank);
            }
            return true;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Bank), ConfigManager.GetFileDBPath("bank"), ".xml");
            this.slotComparer = new BankSlotComparer();
        }

        public Bank Read(string _id, string _charName, BankRace _race, BankCache _cache)
        {
            Bank bank = this._Read(_id);
            if (bank == null)
            {
                return null;
            }
            if (_race == BankRace.None)
            {
                bank.slot.Clear();
                return bank;
            }
            for (int i = 0; i < bank.slot.Count; i++)
            {
                BankSlot slot = (BankSlot) bank.slot[i];
                if (slot.Race != _race)
                {
                    bank.slot.RemoveAt(i--);
                }
            }
            bank.SetBankLoadState(_race);
            return bank;
        }

        public bool RemoveSlot(string _account, long _id)
        {
            return true;
        }

        public bool Write(string _charName, Bank _data, BankCache _cache)
        {
            Bank bank = this._Read(_data.account);
            if (bank == null)
            {
                return false;
            }
            bank.data = _data.data;
            for (int i = 0; i < 3; i++)
            {
                BankRace race = (BankRace) ((byte) i);
                if (_data.IsBankLoaded(race))
                {
                    bank.SetWealth(race, _data.GetWealth(race));
                }
            }
            if (_data.slot != null)
            {
                bank.slot.Sort(this.slotComparer);
                foreach (BankSlot slot in _data.slot)
                {
                    int num2 = bank.slot.BinarySearch(slot, this.slotComparer);
                    if (num2 >= 0)
                    {
                        bank.slot[num2] = slot;
                    }
                }
            }
            base.WriteToDB(bank, bank.account);
            return true;
        }

        public bool WriteEx(Bank _bank, CharacterInfo _character, BankCache _bankCache, CharacterInfo _charCache, CharacterAdapter _charAdapter)
        {
            if (base.IsExistData(_bank.account) && (_charAdapter.Read(_character.id, null) != null))
            {
                this.Write(_character.name, _bank, _bankCache);
                _charAdapter.Write(_character, null);
                return true;
            }
            return false;
        }
    }
}

