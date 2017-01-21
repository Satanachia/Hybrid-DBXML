namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertiseSerializer
    {
        public static ShopAdvertise Serialize(Message _message)
        {
            ShopAdvertise advertise = new ShopAdvertise();
            advertise.shopInfo = ShopAdvertisebaseSerializer.Serialize(_message);
            int num = _message.ReadS32();
            if (num == 0)
            {
                advertise.items = null;
                return advertise;
            }
            advertise.items = new ShopAdvertiseItem[num];
            for (int i = 0; i < num; i++)
            {
                advertise.items[i] = ShopAdvertiseItemSerializer.Serialize(_message);
            }
            return advertise;
        }
    }
}

