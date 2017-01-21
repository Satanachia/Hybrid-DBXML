namespace XMLDB3
{
    using System;
    using System.Data;

    public class HouseInventoryObjectBuilder
    {
        public static HouseInventory Build(DataTable _itemLarge, DataTable _itemSmall, DataTable _itemHuge, DataTable _itemQuest)
        {
            if (((_itemLarge == null) || (_itemSmall == null)) || ((_itemHuge == null) || (_itemQuest == null)))
            {
                throw new Exception("집 아이템 테이블을 얻지 못하였습니다.");
            }
            HouseInventory inventory = new HouseInventory();
            int num = 0;
            if ((_itemLarge != null) && (_itemLarge.Rows != null))
            {
                num += _itemLarge.Rows.Count;
            }
            if ((_itemSmall != null) && (_itemSmall.Rows != null))
            {
                num += _itemSmall.Rows.Count;
            }
            if ((_itemHuge != null) && (_itemHuge.Rows != null))
            {
                num += _itemHuge.Rows.Count;
            }
            if ((_itemQuest != null) && (_itemQuest.Rows != null))
            {
                num += _itemQuest.Rows.Count;
            }
            if (num > 0)
            {
                int num2 = 0;
                HouseItem[] itemArray = new HouseItem[num];
                if ((_itemLarge != null) && (_itemLarge.Rows != null))
                {
                    foreach (DataRow row in _itemLarge.Rows)
                    {
                        itemArray[num2++] = ItemSqlBuilder.GetHouseItem(Item.StoredType.IstLarge, row);
                    }
                }
                if ((_itemSmall != null) && (_itemSmall.Rows != null))
                {
                    foreach (DataRow row2 in _itemSmall.Rows)
                    {
                        itemArray[num2++] = ItemSqlBuilder.GetHouseItem(Item.StoredType.IstSmall, row2);
                    }
                }
                if ((_itemHuge != null) && (_itemHuge.Rows != null))
                {
                    foreach (DataRow row3 in _itemHuge.Rows)
                    {
                        itemArray[num2++] = ItemSqlBuilder.GetHouseItem(Item.StoredType.IstHuge, row3);
                    }
                }
                if ((_itemQuest != null) && (_itemQuest.Rows != null))
                {
                    foreach (DataRow row4 in _itemQuest.Rows)
                    {
                        itemArray[num2++] = ItemSqlBuilder.GetHouseItem(Item.StoredType.IstQuest, row4);
                    }
                }
                inventory.item = itemArray;
            }
            return inventory;
        }
    }
}

