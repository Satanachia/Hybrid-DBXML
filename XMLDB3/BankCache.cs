namespace XMLDB3
{
    using System;
    using System.Collections;

    public class BankCache
    {
        private string account;
        public BankData bank;
        private LinkedHybridCache[] inventory;
        private bool[] loadState;
        private int[] wealth;

        public BankCache()
        {
            this.account = null;
            this.bank = null;
            this.wealth = new int[3];
            this.loadState = new bool[3];
            this.inventory = new LinkedHybridCache[3];
        }

        public BankCache(Bank _bank)
        {
            this.wealth = new int[3];
            this.loadState = new bool[3];
            this.inventory = new LinkedHybridCache[3];
            this.Update(_bank);
        }

        public void AddItem(string _slotName, BankRace _race, BankItem _item)
        {
            this.inventory[(int) _race].AddItem(_slotName, _item.item.id, _item);
        }

        private LinkedHybridCache CreateInventory(ArrayList _slots, BankRace _race)
        {
            if (_slots == null)
            {
                return null;
            }
            LinkedHybridCache cache = new LinkedHybridCache();
            new ArrayList();
            foreach (BankSlot slot in _slots)
            {
                if (((slot != null) && slot.IsValid()) && (slot.Race == _race))
                {
                    cache.AddSection(slot.Name, slot.slot);
                    if (slot.item != null)
                    {
                        foreach (BankItem item in slot.item)
                        {
                            cache.AddItem(slot.Name, item.item.id, item);
                        }
                    }
                }
            }
            return cache;
        }

        public ISection FindSlot(string _slotName, BankRace _race)
        {
            return this.inventory[(int) _race].FindSection(_slotName);
        }

        public static BankSlotInfo GetSlotInfo(ILinkItem _item)
        {
            if ((_item.Section != null) && (_item.Section.Context != null))
            {
                return (BankSlotInfo) _item.Section.Context;
            }
            return null;
        }

        public int GetWealth(BankRace _race)
        {
            if ((BankRace.Elf | BankRace.Giant) <= _race)
            {
                return 0;
            }
            return this.wealth[(int) _race];
        }

        public void Invalidate()
        {
            this.bank = null;
            for (int i = 0; i < 3; i++)
            {
                this.loadState[i] = false;
            }
        }

        public bool IsRaceLoaded(BankRace _race)
        {
            if (_race != BankRace.None)
            {
                return this.loadState[(int) _race];
            }
            return this.IsValid();
        }

        public bool IsValid()
        {
            return (((this.account != null) && (this.account != string.Empty)) && (this.bank != null));
        }

        public void MoveSlot(string _slotName, BankRace _newRace, BankRace _oldRace, ILinkItem _item)
        {
            if (_newRace != _oldRace)
            {
                BankItem context = (BankItem) _item.Context;
                this.inventory[(int) _oldRace].RemoveItem(context.item.id);
                this.AddItem(_slotName, _newRace, context);
            }
            else
            {
                this.inventory[(int) _newRace].MoveSection(_slotName, _item);
            }
        }

        public void RemoveItem(BankRace _race, BankItem _item)
        {
            this.inventory[(int) _race].RemoveItem(_item.item.id);
        }

        public void SetWealth(BankRace _race, int _value)
        {
            if ((BankRace.Elf | BankRace.Giant) > _race)
            {
                this.wealth[(int) _race] = _value;
            }
        }

        public Bank ToBank(BankRace _race)
        {
            if (!this.IsValid() || !this.IsRaceLoaded(_race))
            {
                return null;
            }
            Bank bank = new Bank();
            bank.account = this.account;
            bank.data = this.bank;
            if (_race != BankRace.None)
            {
                LinkedHybridCache cache = this.inventory[(int) _race];
                if (cache != null)
                {
                    ICollection is2 = cache.GetSection();
                    if (is2.Count > 0)
                    {
                        foreach (ISection section in is2)
                        {
                            BankSlot slot = new BankSlot((BankSlotInfo) section.Context, (BankItem[]) section.ToArray(typeof(BankItem)));
                            bank.slot.Add(slot);
                        }
                    }
                }
                bank.SetBankLoadState(_race);
            }
            return bank;
        }

        public void Update(Bank _bank)
        {
            if ((_bank == null) || !_bank.IsValid())
            {
                throw new ArgumentException("유효하지 않은 은행 데이터입니다.", "_data");
            }
            this.account = _bank.account;
            this.bank = _bank.data;
            for (int i = 0; i < 3; i++)
            {
                if (_bank.IsBankLoaded((BankRace) ((byte) i)))
                {
                    this.inventory[i] = this.CreateInventory(_bank.slot, (BankRace) ((byte) i));
                    this.loadState[i] = true;
                }
            }
        }

        public string Account
        {
            get
            {
                return this.account;
            }
        }
    }
}

