namespace XMLDB3
{
    using System;

    public class ShopAdvertiseEmptyAdapter : ShopAdvertiseAdapter
    {
        public bool AddItem(string _account, string _server, ShopAdvertiseItem _item)
        {
            return false;
        }

        public bool DeleteItem(string _account, string _server, long _itemID)
        {
            return false;
        }

        public void Initialize(string _argument)
        {
        }

        public ShopAdvertiseList Read(string _server, HouseAdapter _houseAdapter)
        {
            return null;
        }

        public bool Register(ShopAdvertise _advertise)
        {
            return false;
        }

        public bool SetItemPrice(string _account, string _server, long _itemID, int _shopPrice)
        {
            return false;
        }

        public bool Unregister(string _account, string _server)
        {
            return false;
        }

        public bool UpdateShopAdvertise(ShopAdvertisebase _advertise)
        {
            return false;
        }
    }
}

