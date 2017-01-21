namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class ShopAdvertiseSqlAdapter : SqlAdapter, ShopAdvertiseAdapter
    {
        private bool _Read(out Hashtable _shopTable, out Hashtable _itemTable, string _server)
        {
            bool flag;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter._Read() : 데이터베이스에 연결합니다");
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("dbo.SelectShopAdvertiseList", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.Add("@server", SqlDbType.NVarChar, 0x80).Value = _server;
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "ShopTable");
                adapter.TableMappings.Add("Table1", "ItemTable");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter._Read() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                _shopTable = this.BuildShop(dataSet.Tables["ShopTable"]);
                _itemTable = this.BuildItem(dataSet.Tables["ItemTable"]);
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _server);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _shopTable = null;
                _itemTable = null;
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter._Read() : 데이터 베이스에 연결을 해제합니다");
                connection.Close();
            }
            return flag;
        }

        public bool AddItem(string _account, string _server, ShopAdvertiseItem _item)
        {
            bool flag;
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    string cmdText = string.Concat(new object[] { 
                        "exec dbo.CreateShopAdvertiseItem  @strAccount=", UpdateUtility.BuildString(_account), ",@server=", UpdateUtility.BuildString(_server), ",@itemID=", _item.id, ",@storedtype=", _item.storedtype, ",@itemname=", UpdateUtility.BuildString(_item.itemName), ",@price=", _item.price, ",@class=", _item.@class, ",@color_01=", _item.color_01, 
                        ",@color_02=", _item.color_02, ",@color_03=", _item.color_03, "\n"
                     });
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 명령을 수행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account, _server, _item.id);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account, _server, _item.id);
                WorkSession.WriteStatus(exception2.Message, _account);
                flag = false;
            }
            return flag;
        }

        private Hashtable BuildItem(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("Shop Advertise Item table is NULL");
            }
            Hashtable hashtable = new Hashtable();
            if ((_table.Rows != null) && (_table.Rows.Count > 0))
            {
                foreach (DataRow row in _table.Rows)
                {
                    string str = (string) row["account"];
                    ArrayList list = (ArrayList) hashtable[str];
                    if (list == null)
                    {
                        list = new ArrayList();
                        hashtable[str] = list;
                    }
                    ShopAdvertiseItem item = new ShopAdvertiseItem();
                    item.id = (long) row["itemID"];
                    item.storedtype = (byte) row["storedtype"];
                    item.price = (int) row["price"];
                    list.Add(item);
                }
            }
            return hashtable;
        }

        private Hashtable BuildShop(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("Shop Advertise table is NULL");
            }
            Hashtable hashtable = new Hashtable();
            if ((_table.Rows != null) && (_table.Rows.Count > 0))
            {
                foreach (DataRow row in _table.Rows)
                {
                    ShopAdvertiseDetail detail = new ShopAdvertiseDetail();
                    detail.shopInfo = new ShopAdvertisebase();
                    detail.shopInfo.account = (string) row["account"];
                    detail.shopInfo.server = (string) row["server"];
                    detail.shopInfo.shopName = (string) row["shopname"];
                    detail.shopInfo.area = (string) row["area"];
                    detail.shopInfo.characterName = (string) row["charactername"];
                    detail.shopInfo.comment = (string) row["comment"];
                    detail.shopInfo.startTime = (long) row["starttime"];
                    detail.shopInfo.region = (int) row["region"];
                    detail.shopInfo.x = (int) row["x"];
                    detail.shopInfo.y = (int) row["y"];
                    detail.shopInfo.leafletCount = (int) row["leafletCount"];
                    hashtable[detail.shopInfo.account] = detail;
                }
            }
            return hashtable;
        }

        public bool DeleteItem(string _account, string _server, long _itemID)
        {
            bool flag;
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    string cmdText = string.Concat(new object[] { "exec dbo.DeleteShopAdvertiseItem  @strAccount=", UpdateUtility.BuildString(_account), ",@server=", UpdateUtility.BuildString(_server), ",@itemID=", _itemID, "\n" });
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.DeleteItem() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.DeleteItem() : 명령을 수행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account, _server, _itemID);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account, _server, _itemID);
                WorkSession.WriteStatus(exception2.Message, _account);
                flag = false;
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(ShopAdvertise), _argument);
        }

        public ShopAdvertiseList Read(string _server, HouseAdapter _houseAdapter)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                Hashtable hashtable;
                Hashtable hashtable2;
                if (this._Read(out hashtable, out hashtable2, _server))
                {
                    if ((hashtable2 != null) && (hashtable2.Count > 0))
                    {
                        foreach (string str in hashtable2.Keys)
                        {
                            ShopAdvertiseDetail detail = (ShopAdvertiseDetail) hashtable[str];
                            ArrayList list = (ArrayList) hashtable2[str];
                            if (((list != null) && (list.Count > 0)) && (detail != null))
                            {
                                ArrayList list2 = new ArrayList();
                                foreach (ShopAdvertiseItem item in list)
                                {
                                    ShopAdvertiseItemDetail detail2 = this.ReadHouseItem(str, item, (HouseSqlAdapter) _houseAdapter);
                                    if (detail2 != null)
                                    {
                                        list2.Add(detail2);
                                    }
                                    else
                                    {
                                        ExceptionMonitor.ExceptionRaised(new Exception("Fail to read shop item"), item.id, str);
                                        if (Console.Out != null)
                                        {
                                            Console.WriteLine("Fail to read shop item [{0}][{1}]", str, item.id);
                                        }
                                    }
                                }
                                if (list2.Count > 0)
                                {
                                    detail.items = (ShopAdvertiseItemDetail[]) list2.ToArray(typeof(ShopAdvertiseItemDetail));
                                }
                            }
                        }
                    }
                    ShopAdvertiseList list3 = new ShopAdvertiseList();
                    list3.advertises = new ShopAdvertiseDetail[hashtable.Values.Count];
                    hashtable.Values.CopyTo(list3.advertises, 0);
                    return list3;
                }
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Read() : 광고 테이블을 읽지 못했습니다.");
                return null;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _server);
                WorkSession.WriteStatus(exception.Message, _server);
                return null;
            }
        }

        private ShopAdvertiseItemDetail ReadHouseItem(string _account, ShopAdvertiseItem _item, HouseSqlAdapter _houseAdapter)
        {
            ShopAdvertiseItemDetail detail2;
            SqlConnection connection = new SqlConnection(_houseAdapter.ConnectionString);
            try
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.ReadHouseItem() : 데이터베이스에 연결합니다");
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("dbo." + ItemSqlBuilder.GetHouseItemSelectProc(_item.Type), connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                selectCommand.Parameters.Add("@itemID", SqlDbType.BigInt).Value = _item.id;
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.ReadHouseItem() : 데이터를 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                if (((dataSet.Tables.Count > 0) && (dataSet.Tables[0] != null)) && ((dataSet.Tables[0].Rows != null) && (dataSet.Tables[0].Rows.Count > 0)))
                {
                    HouseItem houseItem = ItemSqlBuilder.GetHouseItem(_item.Type, dataSet.Tables[0].Rows[0]);
                    ShopAdvertiseItemDetail detail = new ShopAdvertiseItemDetail();
                    detail.item = houseItem.item;
                    detail.shopPrice = _item.price;
                    return detail;
                }
                detail2 = null;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                detail2 = null;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.ReadHouseItem() : 데이터베이스에 연결을 해제합니다");
                connection.Close();
            }
            return detail2;
        }

        public bool Register(ShopAdvertise _advertise)
        {
            bool flag;
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    string cmdText = string.Concat(new object[] { 
                        "exec dbo.CreateShopAdvertise  @strAccount=", UpdateUtility.BuildString(_advertise.shopInfo.account), ",@server=", UpdateUtility.BuildString(_advertise.shopInfo.server), ",@shopname=", UpdateUtility.BuildString(_advertise.shopInfo.shopName), ",@area=", UpdateUtility.BuildString(_advertise.shopInfo.area), ",@charactername=", UpdateUtility.BuildString(_advertise.shopInfo.characterName), ",@comment=", UpdateUtility.BuildString(_advertise.shopInfo.comment), ",@starttime=", _advertise.shopInfo.startTime, ",@region=", _advertise.shopInfo.region, 
                        ",@x=", _advertise.shopInfo.x, ",@y=", _advertise.shopInfo.y, ",@leafletCount=", _advertise.shopInfo.leafletCount, "\n"
                     });
                    if ((_advertise.items != null) && (_advertise.items.Length > 0))
                    {
                        foreach (ShopAdvertiseItem item in _advertise.items)
                        {
                            object obj2 = cmdText;
                            cmdText = string.Concat(new object[] { 
                                obj2, "exec dbo.CreateShopAdvertiseItem  @strAccount=", UpdateUtility.BuildString(_advertise.shopInfo.account), ",@server=", UpdateUtility.BuildString(_advertise.shopInfo.server), ",@itemID=", item.id, ",@storedtype=", item.storedtype, ",@itemname=", UpdateUtility.BuildString(item.itemName), ",@price=", item.price, ",@class=", item.@class, ",@color_01=", 
                                item.color_01, ",@color_02=", item.color_02, ",@color_03=", item.color_03, "\n"
                             });
                        }
                    }
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("SHOPADVERTISE_REGISTER_APP");
                    SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 명령을 수행합니다.");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _advertise);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    transaction.Rollback("SHOPADVERTISE_REGISTER_APP");
                    flag = false;
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2, _advertise);
                    WorkSession.WriteStatus(exception2.Message, _advertise);
                    transaction.Rollback("SHOPADVERTISE_REGISTER_APP");
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _advertise);
                WorkSession.WriteStatus(exception3.Message, _advertise);
                flag = false;
            }
            return flag;
        }

        public bool SetItemPrice(string _account, string _server, long _itemID, int _shopPrice)
        {
            bool flag;
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    string cmdText = string.Concat(new object[] { "exec dbo.UpdateShopAdvertiseItemPrice  @strAccount=", UpdateUtility.BuildString(_account), ",@server=", UpdateUtility.BuildString(_server), ",@itemID=", _itemID, ",@price=", _shopPrice, "\n" });
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 명령을 수행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account, _server, _itemID);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account, _server, _itemID);
                WorkSession.WriteStatus(exception2.Message, _account);
                flag = false;
            }
            return flag;
        }

        public bool Unregister(string _account, string _server)
        {
            bool flag;
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("dbo.DeleteShopAdvertise", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                    command.Parameters.Add("@server", SqlDbType.NVarChar, 0x80).Value = _server;
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 명령을 수행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account, _server);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account, _server);
                WorkSession.WriteStatus(exception2.Message, _account);
                flag = false;
            }
            return flag;
        }

        public bool UpdateShopAdvertise(ShopAdvertisebase _advertise)
        {
            bool flag;
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    string cmdText = string.Concat(new object[] { 
                        "exec dbo.UpdateShopAdvertise  @strAccount=", UpdateUtility.BuildString(_advertise.account), ",@server=", UpdateUtility.BuildString(_advertise.server), ",@shopname=", UpdateUtility.BuildString(_advertise.shopName), ",@area=", UpdateUtility.BuildString(_advertise.area), ",@charactername=", UpdateUtility.BuildString(_advertise.characterName), ",@comment=", UpdateUtility.BuildString(_advertise.comment), ",@starttime=", _advertise.startTime, ",@region=", _advertise.region, 
                        ",@x=", _advertise.x, ",@y=", _advertise.y, ",@leafletCount=", _advertise.leafletCount, "\n"
                     });
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 명령을 수행합니다.");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _advertise.account, _advertise.server);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2, _advertise.account, _advertise.server);
                    WorkSession.WriteStatus(exception2.Message, _advertise.account);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _advertise);
                WorkSession.WriteStatus(exception3.Message, _advertise);
                flag = false;
            }
            return flag;
        }
    }
}

