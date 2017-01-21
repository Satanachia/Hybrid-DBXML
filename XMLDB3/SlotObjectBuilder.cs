namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class SlotObjectBuilder
    {
        public static void Build(Bank _bank, DataSet _ds)
        {
            DataTable table = _ds.Tables["slot"];
            DataTable table2 = _ds.Tables["itemLarge"];
            DataTable table3 = _ds.Tables["itemSmall"];
            DataTable table4 = _ds.Tables["itemHuge"];
            DataTable table5 = _ds.Tables["itemQuest"];
            if (((table == null) || (table2 == null)) || (((table3 == null) || (table4 == null)) || (table5 == null)))
            {
                throw new Exception("은행 아이템 테이블이 없습니다.");
            }
            if (table.Rows != null)
            {
                Hashtable hashtable = new Hashtable(table.Rows.Count);
                ArrayList list = new ArrayList(table.Rows.Count);
                foreach (DataRow row in table.Rows)
                {
                    string str = (string) row["name"];
                    BankRace race = (BankRace) ((byte) row["race"]);
                    BankSlot slot = new BankSlot(str, race);
                    hashtable.Add(slot.Name, new ArrayList());
                    list.Add(slot);
                }
                if ((table2 != null) && (table2.Rows != null))
                {
                    BuildBankItem(table2.Rows, hashtable, Item.StoredType.IstLarge);
                }
                if ((table3 != null) && (table3.Rows != null))
                {
                    BuildBankItem(table3.Rows, hashtable, Item.StoredType.IstSmall);
                }
                if ((table4 != null) && (table4.Rows != null))
                {
                    BuildBankItem(table4.Rows, hashtable, Item.StoredType.IstHuge);
                }
                if ((table5 != null) && (table5.Rows != null))
                {
                    BuildBankItem(table5.Rows, hashtable, Item.StoredType.IstQuest);
                }
                foreach (BankSlot slot2 in list)
                {
                    if (hashtable.Contains(slot2.Name))
                    {
                        ArrayList list2 = (ArrayList) hashtable[slot2.slot.name];
                        slot2.item = (BankItem[]) list2.ToArray(typeof(BankItem));
                    }
                    else
                    {
                        slot2.item = null;
                    }
                }
                _bank.slot = list;
            }
        }

        private static void BuildBankItem(DataRowCollection _rows, Hashtable _slotContainer, Item.StoredType _storedType)
        {
            if (_rows != null)
            {
                foreach (DataRow row in _rows)
                {
                    string key = (string) row["slotname"];
                    BankItem bankItem = ItemSqlBuilder.GetBankItem(_storedType, row);
                    if (_slotContainer.Contains(key))
                    {
                        ((ArrayList) _slotContainer[key]).Add(bankItem);
                    }
                }
            }
        }
    }
}

