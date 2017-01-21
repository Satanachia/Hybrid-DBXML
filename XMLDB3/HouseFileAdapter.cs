namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class HouseFileAdapter : FileAdapter, HouseAdapter
    {
        private HouseItemFileAdapter itemAdapter = null;

        public bool AutoRepay(long _houseID, string _account, HouseInventory _inventory)
        {
            House biddingHouse = this.GetBiddingHouse(_houseID);
            if (biddingHouse == null)
            {
                return false;
            }
            ArrayList list = new ArrayList();
            bool flag = false;
            if (biddingHouse.bidders != null)
            {
                foreach (HouseBidder bidder in biddingHouse.bidders)
                {
                    if (bidder.bidAccount == _account)
                    {
                        flag = true;
                    }
                    else
                    {
                        list.Add(bidder);
                    }
                }
            }
            if (!flag)
            {
                return false;
            }
            if (_inventory.item != null)
            {
                foreach (HouseItem item in _inventory.item)
                {
                    if (!this.UpdateItem(_account, item))
                    {
                        return false;
                    }
                }
            }
            biddingHouse.bidders = (HouseBidder[]) list.ToArray(typeof(HouseBidder));
            base.WriteToDB(biddingHouse, _houseID);
            return true;
        }

        public bool CreateBid(long _houseID, HouseBid _bid)
        {
            House house = this.Read(_houseID);
            if (house == null)
            {
                return false;
            }
            if (house.bid != null)
            {
                return false;
            }
            if ((house.bidders != null) && (house.bidders.Length > 0))
            {
                return false;
            }
            _bid.bidStartTime = DateTime.Now;
            house.bid = _bid;
            base.WriteToDB(house, _houseID);
            return true;
        }

        public REPLY_RESULT CreateBidder(long _houseID, HouseBidder _bidder, BankAdapter _bankAdapter, out byte _errorCode, out int _remainMoney)
        {
            House biddingHouse = this.GetBiddingHouse(_houseID);
            if (biddingHouse != null)
            {
                if (this.ReadItem(_bidder.bidAccount).item != null)
                {
                    _errorCode = 2;
                    _remainMoney = 0;
                    return REPLY_RESULT.FAIL_EX;
                }
                Bank bank = _bankAdapter.Read(_bidder.bidAccount, _bidder.bidCharName, BankRace.None, null);
                if ((bank == null) || (bank.data == null))
                {
                    _errorCode = 0;
                    _remainMoney = 0;
                    return REPLY_RESULT.FAIL_EX;
                }
                if (bank.data.deposit >= _bidder.bidPrice)
                {
                    int length;
                    HouseBidder[] bidderArray;
                    bank.data.deposit -= _bidder.bidPrice;
                    if (!_bankAdapter.Write(_bidder.bidCharName, bank, null))
                    {
                        _errorCode = 3;
                        _remainMoney = 0;
                        return REPLY_RESULT.ERROR;
                    }
                    if (biddingHouse.bidders == null)
                    {
                        bidderArray = new HouseBidder[1];
                        length = 0;
                    }
                    else
                    {
                        length = biddingHouse.bidders.Length;
                        bidderArray = new HouseBidder[length + 1];
                        biddingHouse.bidders.CopyTo(bidderArray, 0);
                    }
                    _bidder.bidTime = DateTime.Now;
                    _bidder.bidUpdateTime = DateTime.Now;
                    bidderArray[length] = _bidder;
                    biddingHouse.bidders = bidderArray;
                    base.WriteToDB(biddingHouse, _houseID);
                    _errorCode = 3;
                    _remainMoney = bank.data.deposit;
                    return REPLY_RESULT.SUCCESS;
                }
                _errorCode = 0;
                _remainMoney = bank.data.deposit;
                return REPLY_RESULT.FAIL_EX;
            }
            _errorCode = 3;
            _remainMoney = 0;
            return REPLY_RESULT.ERROR;
        }

        public REPLY_RESULT DeleteBidder(long _houseID, string _account, string _charName, int _repayMoney, BankAdapter _bankAdapter, int _maxRemainMoney, out int _remainMoney)
        {
            House biddingHouse = this.GetBiddingHouse(_houseID);
            if (biddingHouse != null)
            {
                ArrayList list = new ArrayList();
                bool flag = false;
                if (biddingHouse.bidders != null)
                {
                    foreach (HouseBidder bidder in biddingHouse.bidders)
                    {
                        if (bidder.bidAccount == _account)
                        {
                            flag = true;
                        }
                        else
                        {
                            list.Add(bidder);
                        }
                    }
                }
                if (flag)
                {
                    Bank bank = _bankAdapter.Read(_account, _charName, BankRace.None, null);
                    if (bank == null)
                    {
                        bank = new Bank();
                        bank.data = new BankData();
                    }
                    if (bank.data == null)
                    {
                        bank.data = new BankData();
                    }
                    bank.data.deposit += _repayMoney;
                    if (bank.data.deposit > _maxRemainMoney)
                    {
                        _remainMoney = bank.data.deposit - _repayMoney;
                        return REPLY_RESULT.FAIL_EX;
                    }
                    _remainMoney = bank.data.deposit;
                    _bankAdapter.Write(_charName, bank, null);
                    biddingHouse.bidders = (HouseBidder[]) list.ToArray(typeof(HouseBidder));
                    base.WriteToDB(biddingHouse, _houseID);
                    return REPLY_RESULT.SUCCESS;
                }
                _remainMoney = 0;
                return REPLY_RESULT.FAIL;
            }
            _remainMoney = 0;
            return REPLY_RESULT.ERROR;
        }

        public bool DeleteBlock(long _houseID)
        {
            House house = this.Read(_houseID);
            if (house != null)
            {
                house.block = null;
                base.WriteToDB(house, _houseID);
                return true;
            }
            return false;
        }

        public bool DeleteItem(long _houseID, string _account, Item _item, int _houseMoney)
        {
            HouseInventory inventory = this.ReadItem(_account);
            if ((inventory == null) || (inventory.item == null))
            {
                return false;
            }
            ArrayList list = new ArrayList();
            bool flag = false;
            foreach (HouseItem item in inventory.item)
            {
                if (item.item.id != _item.id)
                {
                    list.Add(item);
                }
                else
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return false;
            }
            inventory.item = (HouseItem[]) list.ToArray(typeof(HouseItem));
            if (_houseID != 0L)
            {
                House house = this.Read(_houseID);
                if (house == null)
                {
                    return false;
                }
                house.houseMoney = _houseMoney;
                base.WriteToDB(house, _houseID);
            }
            this.itemAdapter.WriteToDB(inventory, _account);
            return true;
        }

        public bool DeleteOwner(long _houseID, string _account, string _server)
        {
            House house = this.Read(_houseID);
            if (house != null)
            {
                house.account = string.Empty;
                house.houseMoney = 0;
                house.block = null;
                base.WriteToDB(house, _houseID);
                return true;
            }
            return false;
        }

        public bool EndBid(long _houseID, string _account, string _server)
        {
            House biddingHouse = this.GetBiddingHouse(_houseID);
            if (biddingHouse == null)
            {
                return false;
            }
            if (_account != string.Empty)
            {
                biddingHouse.houseID = _houseID;
                biddingHouse.account = _account;
                base.WriteToDB(biddingHouse, _houseID);
            }
            return true;
        }

        public REPLY_RESULT EndBidRepay(long _houseID)
        {
            House house = this.Read(_houseID);
            if (house == null)
            {
                return REPLY_RESULT.ERROR;
            }
            if (house.bid == null)
            {
                return REPLY_RESULT.FAIL_EX;
            }
            house.bidders = null;
            house.bid = null;
            base.WriteToDB(house, _houseID);
            return REPLY_RESULT.SUCCESS;
        }

        private House GetBiddingHouse(long _houseID)
        {
            if (base.IsExistData(_houseID))
            {
                House house = (House) base.ReadFromDB(_houseID);
                if ((house != null) && (house.bid != null))
                {
                    return house;
                }
            }
            return null;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(House), "house/", ".xml");
            this.itemAdapter = new HouseItemFileAdapter();
            this.itemAdapter.Initialize(null);
        }

        private House Read(long _houseID)
        {
            if (base.IsExistData(_houseID))
            {
                return (House) base.ReadFromDB(_houseID);
            }
            return null;
        }

        public bool Read(long _houseID, out House _house)
        {
            if (base.IsExistData(_houseID))
            {
                _house = (House) base.ReadFromDB(_houseID);
                return (_house != null);
            }
            _house = null;
            return true;
        }

        public HouseBlockList ReadBlock(long _houseID)
        {
            House house = this.Read(_houseID);
            if (house != null)
            {
                HouseBlockList list = new HouseBlockList();
                list.block = house.block;
                return list;
            }
            return null;
        }

        public HouseInventory ReadItem(string _account)
        {
            if (this.itemAdapter.IsExistData(_account))
            {
                return (HouseInventory) this.itemAdapter.ReadFromDB(_account);
            }
            return new HouseInventory();
        }

        public HouseItem ReadItem(string _account, long _itemID)
        {
            HouseInventory inventory = this.ReadItem(_account);
            if ((inventory != null) && (inventory.item != null))
            {
                foreach (HouseItem item in inventory.item)
                {
                    if (item.item.id == _itemID)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public bool UpdateBlock(long _houseID, HouseBlock[] _added, HouseBlock[] _deleted)
        {
            House house = this.Read(_houseID);
            if (house == null)
            {
                return false;
            }
            ArrayList list = new ArrayList();
            if (house.block != null)
            {
                Hashtable hashtable = new Hashtable();
                if (_deleted != null)
                {
                    foreach (HouseBlock block in _deleted)
                    {
                        string key = block.gameName + block.flag.ToString();
                        if (!hashtable.ContainsKey(key))
                        {
                            hashtable.Add(key, block);
                        }
                    }
                }
                foreach (HouseBlock block2 in house.block)
                {
                    if (!hashtable.ContainsKey(block2.gameName + block2.flag.ToString()))
                    {
                        list.Add(block2);
                    }
                }
            }
            if (_added != null)
            {
                foreach (HouseBlock block3 in _added)
                {
                    list.Add(block3);
                }
            }
            house.block = (HouseBlock[]) list.ToArray(typeof(HouseBlock));
            base.WriteToDB(house, _houseID);
            return true;
        }

        public bool UpdateItem(string _account, HouseItem _item)
        {
            HouseInventory inventory = this.ReadItem(_account);
            if (inventory == null)
            {
                inventory = new HouseInventory();
                inventory.account = _account;
            }
            if (inventory.item == null)
            {
                inventory.item = new HouseItem[] { _item };
            }
            else
            {
                int index = 0;
                int length = inventory.item.Length;
                index = 0;
                while (index < length)
                {
                    if (inventory.item[index].item.id == _item.item.id)
                    {
                        inventory.item[index] = _item;
                        break;
                    }
                    index++;
                }
                if (index >= length)
                {
                    HouseItem[] item = inventory.item;
                    inventory.item = new HouseItem[length + 1];
                    item.CopyTo(inventory.item, 0);
                    inventory.item[length] = _item;
                }
            }
            this.itemAdapter.WriteToDB(inventory, _account);
            return true;
        }

        public bool Write(House _house)
        {
            House house = this.Read(_house.houseID);
            if (house == null)
            {
                house = new House();
            }
            _house.block = house.block;
            _house.bid = house.bid;
            _house.bidders = house.bidders;
            house = _house;
            base.WriteToDB(house, _house.houseID);
            return true;
        }

        private class HouseItemFileAdapter : FileAdapter
        {
            public void Initialize(string _argument)
            {
                base.Initialize(typeof(HouseInventory), "houseItem/", ".xml");
            }
        }
    }
}

