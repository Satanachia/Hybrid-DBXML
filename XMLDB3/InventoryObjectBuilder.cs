namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;

    public class InventoryObjectBuilder
    {
        public static Hashtable Build(DataTable _itemLarge_table, DataTable _itemSmall_table, DataTable _itemHuge_table, DataTable _itemQuest_table, DataTable _itemEgo_table)
        {
            int capacity = 0;
            if ((_itemLarge_table != null) && (_itemLarge_table.Rows != null))
            {
                capacity += _itemLarge_table.Rows.Count;
            }
            if ((_itemSmall_table != null) && (_itemSmall_table.Rows != null))
            {
                capacity += _itemSmall_table.Rows.Count;
            }
            if ((_itemHuge_table != null) && (_itemHuge_table.Rows != null))
            {
                capacity += _itemHuge_table.Rows.Count;
            }
            if ((_itemQuest_table != null) && (_itemQuest_table.Rows != null))
            {
                capacity += _itemQuest_table.Rows.Count;
            }
            if ((_itemEgo_table != null) && (_itemEgo_table.Rows != null))
            {
                capacity += _itemEgo_table.Rows.Count;
            }
            if (capacity <= 0)
            {
                return null;
            }
            Hashtable hashtable = new Hashtable(capacity);
            Item charItem = null;
            if ((_itemLarge_table != null) && (_itemLarge_table.Rows != null))
            {
                foreach (DataRow row in _itemLarge_table.Rows)
                {
                    charItem = ItemSqlBuilder.GetCharItem(Item.StoredType.IstLarge, row);
                    hashtable.Add(charItem.id, charItem);
                }
            }
            if ((_itemSmall_table != null) && (_itemSmall_table.Rows != null))
            {
                foreach (DataRow row2 in _itemSmall_table.Rows)
                {
                    charItem = ItemSqlBuilder.GetCharItem(Item.StoredType.IstSmall, row2);
                    hashtable.Add(charItem.id, charItem);
                }
            }
            if ((_itemHuge_table != null) && (_itemHuge_table.Rows != null))
            {
                foreach (DataRow row3 in _itemHuge_table.Rows)
                {
                    charItem = ItemSqlBuilder.GetCharItem(Item.StoredType.IstHuge, row3);
                    hashtable.Add(charItem.id, charItem);
                }
            }
            if ((_itemQuest_table != null) && (_itemQuest_table.Rows != null))
            {
                foreach (DataRow row4 in _itemQuest_table.Rows)
                {
                    charItem = ItemSqlBuilder.GetCharItem(Item.StoredType.IstQuest, row4);
                    hashtable.Add(charItem.id, charItem);
                }
            }
            if ((_itemEgo_table != null) && (_itemEgo_table.Rows != null))
            {
                foreach (DataRow row5 in _itemEgo_table.Rows)
                {
                    charItem = ItemSqlBuilder.GetCharItem(Item.StoredType.IstEgo, row5);
                    hashtable.Add(charItem.id, charItem);
                }
            }
            return hashtable;
        }
    }
}

