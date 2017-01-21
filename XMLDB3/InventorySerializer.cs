namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Collections;

    public class InventorySerializer
    {
        public static void Deserialize(Hashtable _inventory, Message _message)
        {
            if ((_inventory != null) && (_inventory.Count > 0))
            {
                _message.WriteS32(_inventory.Count);
                foreach (Item item in _inventory.Values)
                {
                    ItemSerializer.Deserialize(item, _message);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
        }

        public static Hashtable Serialize(Message _message)
        {
            int capacity = _message.ReadS32();
            if (capacity <= 0)
            {
                return null;
            }
            Hashtable hashtable = new Hashtable(capacity);
            for (int i = 0; i < capacity; i++)
            {
                Item item = ItemSerializer.Serialize(_message);
                hashtable.Add(item.id, item);
            }
            return hashtable;
        }
    }
}

