namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class BankSerializer
    {
        public static void Deserialize(Bank _bank, BankRace _race, Message _message)
        {
            if (((_bank != null) && _bank.IsValid()) && _bank.IsBankLoaded(_race))
            {
                _message.WriteString(_bank.account);
                WriteDataToMsg(_bank.data, _message);
                if (_bank.slot != null)
                {
                    int count = _bank.slot.Count;
                    _message.WriteS32(count);
                    foreach (BankSlot slot in _bank.slot)
                    {
                        WriteSlotToMsg(slot, _message);
                    }
                }
                else
                {
                    _message.WriteU32(0);
                }
            }
        }

        private static BankData ReadDataFromMsg(Message _message)
        {
            BankData data = new BankData();
            data.deposit = _message.ReadS32();
            data.password = _message.ReadString();
            return data;
        }

        private static BankItem ReadItemFromMsg(Message _message)
        {
            BankItem item = new BankItem();
            item.location = _message.ReadString();
            item.time = _message.ReadS64();
            item.extraTime = _message.ReadS64();
            item.item = ItemSerializer.Serialize(_message);
            return item;
        }

        private static BankSlot ReadSlotFromMsg(Message _message, BankRace _race)
        {
            BankSlot slot = new BankSlot(_message.ReadString(), _race);
            int num = _message.ReadS32();
            slot.item = new BankItem[num];
            for (int i = 0; i < num; i++)
            {
                slot.item[i] = ReadItemFromMsg(_message);
            }
            return slot;
        }

        private static void ReadSlotsFromMsg(Message _message, Bank _bank, BankRace _race)
        {
            int num = _message.ReadS32();
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    _bank.slot.Add(ReadSlotFromMsg(_message, _race));
                }
                _bank.SetWealth(_race, _message.ReadS32());
                _bank.SetBankLoadState(_race);
            }
        }

        public static Bank Serialize(Message _message)
        {
            Bank bank = new Bank();
            bank.account = _message.ReadString();
            bank.data = ReadDataFromMsg(_message);
            for (int i = 0; i < 3; i++)
            {
                ReadSlotsFromMsg(_message, bank, (BankRace) ((byte) i));
            }
            return bank;
        }

        private static void WriteDataToMsg(BankData _bankdata, Message _message)
        {
            if (_bankdata == null)
            {
                _bankdata = new BankData();
            }
            _message.WriteS32(_bankdata.deposit);
            _message.WriteString((_bankdata.password == null) ? string.Empty : _bankdata.password);
        }

        private static void WriteItemToMsg(BankItem _item, Message _message)
        {
            if (_item == null)
            {
                _item = new BankItem();
            }
            _message.WriteString(_item.location);
            _message.WriteS64(_item.time);
            _message.WriteS64(_item.extraTime);
            ItemSerializer.Deserialize(_item.item, _message);
        }

        private static void WriteSlotToMsg(BankSlot _bankslot, Message _message)
        {
            if (_bankslot == null)
            {
                _bankslot = new BankSlot();
                _bankslot.slot = new BankSlotInfo();
                _bankslot.slot.name = string.Empty;
            }
            _message.WriteString(_bankslot.Name);
            _message.WriteU8((byte) _bankslot.Race);
            if (_bankslot.item != null)
            {
                int length = _bankslot.item.Length;
                _message.WriteS32(length);
                for (int i = 0; i < length; i++)
                {
                    WriteItemToMsg(_bankslot.item[i], _message);
                }
            }
            else
            {
                _message.WriteU32(0);
            }
        }
    }
}

