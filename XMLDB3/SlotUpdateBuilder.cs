namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;
    using System.Text;

    public sealed class SlotUpdateBuilder
    {
        public static string Build(Bank _bank, BankCache _cache)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            Build(_bank, _cache, out str, out str2);
            return (str + str2);
        }

        public static void Build(Bank _bank, BankCache _cache, out string _deleteSql, out string _updateSql)
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if ((_bank.slot != null) && (_bank.slot.Count > 0))
            {
                Hashtable hashtable = new Hashtable();
                foreach (BankSlot slot in _bank.slot)
                {
                    ISection section = _cache.FindSlot(slot.Name, slot.Race);
                    if (section == null)
                    {
                        throw new Exception("Bank slot [" + slot.Name + "] does'nt exist!");
                    }
                    for (ILinkItem item = section.First; item != null; item = item.Next)
                    {
                        BankItem context = (BankItem) item.Context;
                        hashtable.Add(((BankItem) item.Context).item.id, item);
                    }
                }
                foreach (BankSlot slot2 in _bank.slot)
                {
                    foreach (BankItem item2 in slot2.item)
                    {
                        ILinkItem item3 = (ILinkItem) hashtable[item2.item.id];
                        if (item3 == null)
                        {
                            builder.Append(ItemSqlBuilder.BankSelfUpdateItem(_bank.account, slot2.Name, slot2.Race, item2));
                            _cache.AddItem(slot2.Name, slot2.Race, item2);
                        }
                        else
                        {
                            BankSlotInfo slotInfo = BankCache.GetSlotInfo(item3);
                            builder.Append(ItemSqlBuilder.BankUpdateItem(slot2.slot, slotInfo, item2, (BankItem) item3.Context));
                            item3.Context = item2;
                            if (slot2.Name != slotInfo.name)
                            {
                                _cache.MoveSlot(slot2.Name, slot2.Race, (BankRace) slotInfo.race, item3);
                                item3 = null;
                            }
                            hashtable.Remove(item2.item.id);
                        }
                    }
                }
                foreach (ILinkItem item4 in hashtable.Values)
                {
                    BankItem item5 = (BankItem) item4.Context;
                    builder2.Append(ItemSqlBuilder.BankDeleteItem(_bank.account, item5));
                    BankSlotInfo info2 = BankCache.GetSlotInfo(item4);
                    if (info2 != null)
                    {
                        _cache.RemoveItem((BankRace) info2.race, item5);
                    }
                    else
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("슬롯이 없습니다."), _bank.account, item5.item.id);
                    }
                }
                _deleteSql = builder2.ToString();
                _updateSql = builder.ToString();
            }
            else
            {
                _deleteSql = string.Empty;
                _updateSql = string.Empty;
            }
        }

        private static string BuildNewSlot(BankSlot _slot, string _account)
        {
            return ("exec dbo.AddBankSlot  @strAccount=" + UpdateUtility.BuildString(_account) + ",@slotname=" + UpdateUtility.BuildString(_slot.slot.name) + "\n");
        }
    }
}

