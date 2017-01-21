namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Xml.Serialization;

    [XmlRoot(Namespace="", IsNullable=false)]
    public class Bank : BankBase
    {
        [XmlIgnore]
        public const byte MaxBankRace = 3;
        [XmlIgnore]
        private bool[] raceLoadState = new bool[3];
        [XmlIgnore]
        public ArrayList slot = new ArrayList();
        [XmlIgnore]
        private int[] wealth = new int[3];

        public int GetWealth(BankRace _race)
        {
            if ((BankRace.Elf | BankRace.Giant) <= _race)
            {
                return 0;
            }
            return this.wealth[(int) _race];
        }

        public bool IsBankLoaded(BankRace _race)
        {
            if (_race != BankRace.None)
            {
                return this.raceLoadState[(int) _race];
            }
            return this.IsValid();
        }

        public bool IsValid()
        {
            if ((base.account == null) || (base.account == string.Empty))
            {
                return false;
            }
            if (base.data == null)
            {
                return false;
            }
            return true;
        }

        public void SetBankLoadState(BankRace _race)
        {
            if ((BankRace.Elf | BankRace.Giant) > _race)
            {
                this.raceLoadState[(int) _race] = true;
            }
        }

        public void SetBankLoadStateAll(bool _bLoad)
        {
            for (int i = 0; i < 3; i++)
            {
                this.raceLoadState[i] = _bLoad;
            }
        }

        public void SetWealth(BankRace _race, int _value)
        {
            if ((BankRace.Elf | BankRace.Giant) > _race)
            {
                this.wealth[(int) _race] = _value;
            }
        }

        [XmlElement("slot")]
        public BankSlot[] _slot
        {
            get
            {
                if (this.slot != null)
                {
                    BankSlot[] array = new BankSlot[this.slot.Count];
                    this.slot.CopyTo(array, 0);
                    return array;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.slot = new ArrayList(value.Length);
                    foreach (BankSlot slot in value)
                    {
                        this.slot.Add(slot);
                    }
                }
                else
                {
                    this.slot = new ArrayList();
                }
            }
        }

        [XmlAttribute]
        public int elfWealth
        {
            get
            {
                return this.GetWealth(BankRace.Elf);
            }
            set
            {
                this.SetWealth(BankRace.Elf, value);
            }
        }

        [XmlAttribute]
        public int giantWealth
        {
            get
            {
                return this.GetWealth(BankRace.Giant);
            }
            set
            {
                this.SetWealth(BankRace.Giant, value);
            }
        }

        [XmlAttribute]
        public int humanWealth
        {
            get
            {
                return this.GetWealth(BankRace.Human);
            }
            set
            {
                this.SetWealth(BankRace.Human, value);
            }
        }
    }
}

