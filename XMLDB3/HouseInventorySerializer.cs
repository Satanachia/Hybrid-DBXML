namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseInventorySerializer
    {
        public static void Deserialize(HouseInventory _inventory, Message _message)
        {
            if (_inventory.item != null)
            {
                _message.WriteS32(_inventory.item.Length);
                foreach (HouseItem item in _inventory.item)
                {
                    HouseItemSerializer.Deserialize(item, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
        }

        public static HouseInventory Serialize(Message _message)
        {
            int num = _message.ReadS32();
            HouseInventory inventory = new HouseInventory();
            if (num > 0)
            {
                inventory.item = new HouseItem[num];
                for (int i = 0; i < num; i++)
                {
                    inventory.item[i] = HouseItemSerializer.Serialize(_message);
                }
            }
            return inventory;
        }
    }
}

