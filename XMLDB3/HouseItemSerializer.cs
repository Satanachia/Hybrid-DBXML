namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseItemSerializer
    {
        public static void Deserialize(HouseItem _item, Message _message)
        {
            _message.WriteU8(_item.posX);
            _message.WriteU8(_item.posY);
            _message.WriteU8(_item.direction);
            _message.WriteS32(_item.userprice);
            _message.WriteU8(_item.pocket);
            ItemSerializer.Deserialize(_item.item, _message);
        }

        public static HouseItem Serialize(Message _message)
        {
            HouseItem item = new HouseItem();
            item.posX = _message.ReadU8();
            item.posY = _message.ReadU8();
            item.direction = _message.ReadU8();
            item.userprice = _message.ReadS32();
            item.pocket = _message.ReadU8();
            item.item = ItemSerializer.Serialize(_message);
            return item;
        }
    }
}

