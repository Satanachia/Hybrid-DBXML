namespace XMLDB3
{
    using System;

    public class BankSlot : BankSlotBase
    {
        public BankSlot()
        {
            base.slot = null;
            base.item = null;
        }

        public BankSlot(BankSlotInfo _info, BankItem[] _item)
        {
            base.slot = _info;
            base.item = _item;
        }

        public BankSlot(string _name, BankRace _race)
        {
            if ((_name == null) || (_name.Length == 0))
            {
                throw new Exception("Bank slot name is null or empty");
            }
            base.slot = new BankSlotInfo();
            base.slot.name = _name;
            base.slot.race = (byte) _race;
            base.item = null;
        }

        public bool IsValid()
        {
            if (base.slot == null)
            {
                return false;
            }
            return ((base.slot.name != null) && !(base.slot.name == string.Empty));
        }

        public string Name
        {
            get
            {
                return base.slot.name;
            }
        }

        public BankRace Race
        {
            get
            {
                return (BankRace) base.slot.race;
            }
        }
    }
}

