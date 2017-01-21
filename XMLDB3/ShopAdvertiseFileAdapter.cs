namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;

    public class ShopAdvertiseFileAdapter : FileAdapter, ShopAdvertiseAdapter
    {
        public bool AddItem(string _account, string _server, ShopAdvertiseItem _item)
        {
            ShopAdvertiseItem[] itemArray;
            string fileName = this.GetFileName(_account, _server);
            ShopAdvertise advertise = this.Read(fileName);
            if (advertise == null)
            {
                return false;
            }
            if ((advertise.items == null) || (advertise.items.Length == 0))
            {
                itemArray = new ShopAdvertiseItem[1];
            }
            else
            {
                foreach (ShopAdvertiseItem item in advertise.items)
                {
                    if (item.id == _item.id)
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("Shop Advertise Item duplicated"), _item.id, _account);
                        return false;
                    }
                }
                itemArray = new ShopAdvertiseItem[advertise.items.Length + 1];
                advertise.items.CopyTo(itemArray, 0);
            }
            itemArray[advertise.items.Length] = _item;
            advertise.items = itemArray;
            base.WriteToDB(advertise, fileName);
            return true;
        }

        public bool DeleteItem(string _account, string _server, long _itemID)
        {
            string fileName = this.GetFileName(_account, _server);
            ShopAdvertise advertise = this.Read(fileName);
            if (advertise == null)
            {
                return false;
            }
            if ((advertise.items != null) && (advertise.items.Length != 0))
            {
                ArrayList list = new ArrayList();
                foreach (ShopAdvertiseItem item in advertise.items)
                {
                    if (_itemID != item.id)
                    {
                        list.Add(item);
                    }
                }
                advertise.items = (ShopAdvertiseItem[]) list.ToArray(typeof(ShopAdvertiseItem));
                base.WriteToDB(advertise, fileName);
            }
            return true;
        }

        private string GetFileName(string _account, string _server)
        {
            return (_account + "@" + _server);
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(ShopAdvertise), ConfigManager.GetFileDBPath("shopadvertise"), ".xml");
        }

        private ShopAdvertise Read(string _fileName)
        {
            if (base.IsExistData(_fileName))
            {
                return (ShopAdvertise) base.ReadFromDB(_fileName);
            }
            return null;
        }

        public ShopAdvertiseList Read(string _server, HouseAdapter _houseAdapter)
        {
            ShopAdvertiseList list = new ShopAdvertiseList();
            string[] files = System.IO.Directory.GetFiles(base.Directory, "*@" + _server + ".xml");
            if (files != null)
            {
                ArrayList list2 = new ArrayList();
                foreach (string str in files)
                {
                    ShopAdvertise advertise = this.Read(Path.GetFileNameWithoutExtension(str));
                    if (advertise != null)
                    {
                        ShopAdvertiseDetail detail = new ShopAdvertiseDetail();
                        detail.shopInfo = advertise.shopInfo;
                        if ((advertise.items != null) && (advertise.items.Length > 0))
                        {
                            ArrayList list3 = new ArrayList();
                            foreach (ShopAdvertiseItem item in advertise.items)
                            {
                                HouseItem item2 = ((HouseFileAdapter) _houseAdapter).ReadItem(advertise.shopInfo.account, item.id);
                                if (item2 != null)
                                {
                                    ShopAdvertiseItemDetail detail2 = new ShopAdvertiseItemDetail();
                                    detail2.item = item2.item;
                                    detail2.shopPrice = item.price;
                                    list3.Add(detail2);
                                }
                            }
                            if (list3.Count > 0)
                            {
                                detail.items = (ShopAdvertiseItemDetail[]) list3.ToArray(typeof(ShopAdvertiseItemDetail));
                            }
                        }
                        list2.Add(detail);
                    }
                }
                if (list2.Count > 0)
                {
                    list.advertises = (ShopAdvertiseDetail[]) list2.ToArray(typeof(ShopAdvertiseDetail));
                }
            }
            return list;
        }

        public bool Register(ShopAdvertise _advertise)
        {
            string fileName = this.GetFileName(_advertise.shopInfo.account, _advertise.shopInfo.server);
            if (!base.IsExistData(fileName))
            {
                base.WriteToDB(_advertise, fileName);
                return true;
            }
            return false;
        }

        public bool SetItemPrice(string _account, string _server, long _itemID, int _shopPrice)
        {
            string fileName = this.GetFileName(_account, _server);
            ShopAdvertise advertise = this.Read(fileName);
            if (((advertise != null) && (advertise.items != null)) && (advertise.items.Length != 0))
            {
                ShopAdvertiseItem item = null;
                foreach (ShopAdvertiseItem item2 in advertise.items)
                {
                    if (item2.id == _itemID)
                    {
                        item = item2;
                        break;
                    }
                }
                if (item != null)
                {
                    item.price = _shopPrice;
                    base.WriteToDB(advertise, fileName);
                    return true;
                }
                ExceptionMonitor.ExceptionRaised(new Exception("Shop Advertise Item not exists"), _itemID, _account);
            }
            return false;
        }

        public bool Unregister(string _account, string _server)
        {
            string fileName = this.GetFileName(_account, _server);
            if (base.IsExistData(fileName))
            {
                base.DeleteDB(fileName);
            }
            return true;
        }

        public bool UpdateShopAdvertise(ShopAdvertisebase _advertise)
        {
            string fileName = this.GetFileName(_advertise.account, _advertise.server);
            if (base.IsExistData(fileName))
            {
                ShopAdvertise advertise = this.Read(fileName);
                advertise.shopInfo = _advertise;
                base.WriteToDB(advertise, fileName);
                return true;
            }
            return false;
        }
    }
}

