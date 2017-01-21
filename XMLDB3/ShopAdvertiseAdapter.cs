namespace XMLDB3
{
    using System;

    public interface ShopAdvertiseAdapter
    {
        bool AddItem(string _account, string _server, ShopAdvertiseItem _item);
        bool DeleteItem(string _account, string _server, long _itemID);
        void Initialize(string _argument);
        ShopAdvertiseList Read(string _server, HouseAdapter _houseAdapter);
        bool Register(ShopAdvertise _advertise);
        bool SetItemPrice(string _account, string _server, long _itemID, int _shopPrice);
        bool Unregister(string _account, string _server);
        bool UpdateShopAdvertise(ShopAdvertisebase _advertise);
    }
}

