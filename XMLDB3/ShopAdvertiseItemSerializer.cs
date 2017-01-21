namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseItemSerializer
    {
        public static ShopAdvertiseItem Serialize(Message _message)
        {
            ShopAdvertiseItem item = new ShopAdvertiseItem();
            item.id = _message.ReadS64();
            item.storedtype = _message.ReadU8();
            item.itemName = _message.ReadString();
            item.price = _message.ReadS32();
            item.@class = _message.ReadS32();
            item.color_01 = _message.ReadS32();
            item.color_02 = _message.ReadS32();
            item.color_03 = _message.ReadS32();
            return item;
        }
    }
}

