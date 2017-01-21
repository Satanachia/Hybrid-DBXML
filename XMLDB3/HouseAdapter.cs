namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public interface HouseAdapter
    {
        bool AutoRepay(long _houseID, string _account, HouseInventory _inventory);
        bool CreateBid(long _houseID, HouseBid _bid);
        REPLY_RESULT CreateBidder(long _houseID, HouseBidder _bidder, BankAdapter _bankAdapter, out byte _errorCode, out int _remainMoney);
        REPLY_RESULT DeleteBidder(long _houseID, string _account, string _charName, int _repayMoney, BankAdapter _bankAdapter, int _maxRemainMoney, out int _remainMoney);
        bool DeleteBlock(long _houseID);
        bool DeleteItem(long _houseID, string _account, Item _item, int _houseMoney);
        bool DeleteOwner(long _houseID, string _account, string _server);
        bool EndBid(long _houseID, string _account, string _server);
        REPLY_RESULT EndBidRepay(long _houseID);
        void Initialize(string _argument);
        bool Read(long _houseID, out House _house);
        HouseBlockList ReadBlock(long _houseID);
        HouseInventory ReadItem(string _account);
        bool UpdateBlock(long _houseID, HouseBlock[] _added, HouseBlock[] _deleted);
        bool UpdateItem(string _account, HouseItem _item);
        bool Write(House _house);
    }
}

