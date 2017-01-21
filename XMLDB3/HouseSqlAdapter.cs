namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class HouseSqlAdapter : SqlAdapter, HouseAdapter
    {
        private string houseGuestBookConnectionString = null;

        public HouseSqlAdapter(string _houseGuestBookConnectionString)
        {
            this.houseGuestBookConnectionString = _houseGuestBookConnectionString;
        }

        public bool AutoRepay(long _houseID, string _account, HouseInventory _inventory)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("HOUSE_BIDDER_AUTOREPAY_APP");
                    StringBuilder builder = new StringBuilder(100);
                    StringBuilder builder2 = new StringBuilder(100);
                    builder.AppendFormat("exec dbo.DeleteHouseBidder @idHouse={0}, @strAccount={1}\n", _houseID, UpdateUtility.BuildString(_account));
                    if (_inventory.item != null)
                    {
                        foreach (HouseItem item in _inventory.item)
                        {
                            builder2.Append(ItemSqlBuilder.HouseSelfUpdateItem(_account, item));
                        }
                    }
                    SqlCommand command = new SqlCommand(builder.ToString() + builder2.ToString(), connection);
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 명령을 수행합니다.");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 트랜잭션을 롤백합니다");
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_BIDDER_AUTOREPAY_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _houseID, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 트랜잭션을 롤백합니다");
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_BIDDER_AUTOREPAY_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _houseID, _account);
                    WorkSession.WriteStatus(exception2.Message);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _houseID, _account);
                WorkSession.WriteStatus(exception3.Message);
                flag = false;
            }
            return flag;
        }

        private BidState CheckBiddableAccount(string _account)
        {
            BidState unknown;
            WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("dbo.CheckBiddableAccount", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                    WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 명령을 실행합니다.");
                    unknown = (BidState) command.ExecuteScalar();
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    unknown = BidState.Unknown;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message, _account);
                ExceptionMonitor.ExceptionRaised(exception2);
                unknown = BidState.Unknown;
            }
            return unknown;
        }

        public bool CreateBid(long _houseID, HouseBid _bid)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("dbo.CreateHouseBid", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter parameter = command.Parameters.Add("@idHouse", SqlDbType.BigInt, 0x40);
                    SqlParameter parameter2 = command.Parameters.Add("@bidEndTime", SqlDbType.DateTime, 0x40);
                    SqlParameter parameter3 = command.Parameters.Add("@bidRepayEndTime", SqlDbType.DateTime, 0x40);
                    SqlParameter parameter4 = command.Parameters.Add("@minBidPrice", SqlDbType.Int, 0x20);
                    parameter.Value = _houseID;
                    parameter2.Value = _bid.bidEndTime;
                    parameter3.Value = _bid.bidRepayEndTime;
                    parameter4.Value = _bid.minBidPrice;
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, exception.Number, _houseID, _bid);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _houseID, _bid);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public REPLY_RESULT CreateBidder(long _houseID, HouseBidder _bidder, BankAdapter _bankAdapter, out byte _errorCode, out int _remainMoney)
        {
            REPLY_RESULT eRROR;
            WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _bankAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 집 아이템을 체크합니다.");
                BidState state = this.CheckBiddableAccount(_bidder.bidAccount);
                if (state != BidState.Biddable)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 집에 아이템이 있습니다.");
                    _errorCode = (byte) state;
                    _remainMoney = 0;
                    return REPLY_RESULT.FAIL_EX;
                }
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("HOUSE_BIDDER_CREATE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("HOUSE_BIDDER_CREATE_APP");
                SqlCommand command = new SqlCommand("dbo.WithdrawBankDeposit", connection2);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction2;
                SqlParameter parameter = command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20);
                SqlParameter parameter2 = command.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter.Value = _bidder.bidAccount;
                parameter2.Value = _bidder.bidPrice;
                SqlCommand command2 = new SqlCommand(string.Concat(new object[] { "exec dbo.CreateHouseBidder @idHouse=", _houseID, ",@bidAccount=", UpdateUtility.BuildString(_bidder.bidAccount), ",@bidPrice=", _bidder.bidPrice, ",@bidOrder=", _bidder.bidOrder, ",@bidCharacter=", _bidder.bidCharacter, ",@bidCharName=", UpdateUtility.BuildString(_bidder.bidCharName) }), connection);
                command2.Transaction = transaction;
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 은행에서 돈을 인출합니다.");
                _remainMoney = (int) command.ExecuteScalar();
                if (_remainMoney >= 0)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 입찰자를 생성합니다.");
                    command2.ExecuteNonQuery();
                    transaction2.Commit();
                    transaction.Commit();
                    _errorCode = 3;
                    return REPLY_RESULT.SUCCESS;
                }
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 은행 잔고가 부족합니다.");
                _errorCode = 0;
                _remainMoney += _bidder.bidPrice;
                transaction2.Commit();
                transaction.Commit();
                eRROR = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("HOUSE_BIDDER_CREATE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("HOUSE_BIDDER_CREATE_APP");
                }
                if (exception.Number == 0xc350)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 계정 [" + _bidder.bidAccount + "] 이미 집을 소유하고 있거나, 입찰에 참여중입니다.");
                    _errorCode = 1;
                    _remainMoney = 0;
                    return REPLY_RESULT.FAIL_EX;
                }
                ExceptionMonitor.ExceptionRaised(exception, _houseID, _bidder);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _errorCode = 3;
                _remainMoney = 0;
                eRROR = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("HOUSE_BIDDER_CREATE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("HOUSE_BIDDER_CREATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _houseID, _bidder);
                WorkSession.WriteStatus(exception2.Message);
                _errorCode = 3;
                _remainMoney = 0;
                eRROR = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return eRROR;
        }

        private bool CreateHouseGuestBook(string _account, string _server)
        {
            if (this.houseGuestBookConnectionString != null)
            {
                SqlConnection connection = new SqlConnection(this.houseGuestBookConnectionString);
                try
                {
                    connection.Open();
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateHouseGuestBook() : 데이터베이스와 연결합니다");
                    SqlCommand command = new SqlCommand("dbo.CreateGuestBook", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@strServer", SqlDbType.NVarChar, 0x80).Value = _server;
                    command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                    command.Parameters.Add("@strMsg", SqlDbType.NVarChar, 0x3e8).Value = string.Empty;
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateHouseGuestBook() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    return false;
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2, _account);
                    WorkSession.WriteStatus(exception2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            return false;
        }

        public REPLY_RESULT DeleteBidder(long _houseID, string _account, string _charName, int _repayMoney, BankAdapter _bankAdapter, int _maxRemainMoney, out int _remainMoney)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _bankAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("HOUSE_BIDDER_DELETE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("HOUSE_BIDDER_DELETE_APP");
                SqlCommand command = new SqlCommand("dbo.AddBankDeposit", connection2);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction2;
                SqlParameter parameter = command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20);
                SqlParameter parameter2 = command.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter.Value = _account;
                parameter2.Value = _repayMoney;
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 은행에 입금합니다.");
                _remainMoney = (int) command.ExecuteScalar();
                if (_remainMoney > _maxRemainMoney)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 은행에 돈이 너무 많습니다.");
                    _remainMoney -= _repayMoney;
                    transaction2.Rollback("HOUSE_BIDDER_DELETE_APP");
                    transaction.Rollback("HOUSE_BIDDER_DELETE_APP");
                    return REPLY_RESULT.FAIL_EX;
                }
                SqlCommand command2 = new SqlCommand("dbo.DeleteHouseBidder", connection);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.Transaction = transaction;
                SqlParameter parameter3 = command2.Parameters.Add("@idHouse", SqlDbType.BigInt, 0x40);
                SqlParameter parameter4 = command2.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20);
                parameter3.Value = _houseID;
                parameter4.Value = _account;
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 입찰자를 삭제합니다.");
                if (command2.ExecuteNonQuery() < 1)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 계정이 입찰하지 않았습니다.");
                    ExceptionMonitor.ExceptionRaised(new Exception("계정 [" + _account + "] 는 입찰하지 않았습니다."));
                    transaction2.Rollback("HOUSE_BIDDER_DELETE_APP");
                    transaction.Rollback("HOUSE_BIDDER_DELETE_APP");
                    return REPLY_RESULT.FAIL;
                }
                transaction2.Commit();
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("HOUSE_BIDDER_DELETE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("HOUSE_BIDDER_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _houseID, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _remainMoney = 0;
                sUCCESS = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("HOUSE_BIDDER_DELETE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("HOUSE_BIDDER_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _houseID, _account);
                WorkSession.WriteStatus(exception2.Message);
                _remainMoney = 0;
                sUCCESS = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return sUCCESS;
        }

        public bool DeleteBlock(long _houseID)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ClearBlock() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("dbo.DeleteHouseBlock", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@idHouse", SqlDbType.BigInt).Value = _houseID;
                    WorkSession.WriteStatus("HouseSqlAdapter.ClearBlock() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _houseID);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ClearBlock() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _houseID);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        private bool DeleteHouseGuestBook(string _account, string _server)
        {
            if (this.houseGuestBookConnectionString != null)
            {
                SqlConnection connection = new SqlConnection(this.houseGuestBookConnectionString);
                try
                {
                    connection.Open();
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteHouseGuestBook() : 데이터베이스와 연결합니다");
                    SqlCommand command = new SqlCommand("dbo.DeleteGuestBook", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@strServer", SqlDbType.NVarChar, 0x80).Value = _server;
                    command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteHouseGuestBook() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    return false;
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2, _account);
                    WorkSession.WriteStatus(exception2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteHouseGuestBook() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            return false;
        }

        public bool DeleteItem(long _houseID, string _account, Item _item, int _houseMoney)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(ItemSqlBuilder.HouseDeleteItem(_account, _houseID, _item, _houseMoney), connection);
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account, _houseMoney);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account, _houseMoney);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public bool DeleteOwner(long _houseID, string _account, string _server)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 함수에 진입하였습니다.");
            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 지울 아이템 목록을 읽습니다.");
                if (this.ReadItem(_account) == null)
                {
                    throw new Exception("집 [" + _houseID.ToString() + "]인벤토리를 읽는데 실패하였습니다.");
                }
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("HOUSE_DELETE_OWNER_APP");
                    SqlCommand command = new SqlCommand("exec dbo.DeleteHouseOwner @idHouse=" + _houseID.ToString() + ",@strAccount=" + UpdateUtility.BuildString(_account) + "\n", connection);
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    this.DeleteHouseGuestBook(_account, _server);
                    flag = true;
                }
                catch (SqlException exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_DELETE_OWNER_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _houseID, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_DELETE_OWNER_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _houseID, _account);
                    WorkSession.WriteStatus(exception2.Message);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _houseID, _account);
                WorkSession.WriteStatus(exception3.Message);
                flag = false;
            }
            return flag;
        }

        public bool EndBid(long _houseID, string _account, string _server)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("HOUSE_END_BID_APP");
                    SqlCommand command = new SqlCommand("dbo.EndHouseBid", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Transaction = transaction;
                    SqlParameter parameter = command.Parameters.Add("@idHouse", SqlDbType.BigInt, 0x40);
                    SqlParameter parameter2 = command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20);
                    parameter.Value = _houseID;
                    parameter2.Value = _account;
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    this.CreateHouseGuestBook(_account, _server);
                    flag = true;
                }
                catch (SqlException exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_END_BID_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _houseID, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_END_BID_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _houseID, _account);
                    WorkSession.WriteStatus(exception2.Message);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _houseID, _account);
                WorkSession.WriteStatus(exception3.Message);
                flag = false;
            }
            return flag;
        }

        public REPLY_RESULT EndBidRepay(long _houseID)
        {
            REPLY_RESULT eRROR;
            WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("dbo.EndHouseBidRepay", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@idHouse", SqlDbType.BigInt, 0x40).Value = _houseID;
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 명령을 실행합니다");
                    if (((int) command.ExecuteScalar()) > 0)
                    {
                        return REPLY_RESULT.SUCCESS;
                    }
                    eRROR = REPLY_RESULT.FAIL_EX;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _houseID);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    eRROR = REPLY_RESULT.ERROR;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _houseID);
                WorkSession.WriteStatus(exception2.Message);
                eRROR = REPLY_RESULT.ERROR;
            }
            return eRROR;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(House), _argument);
        }

        public bool Read(long _houseID, out House _house)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.Read() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand selectCommand = new SqlCommand("dbo.SelectHouse", connection);
                    selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    selectCommand.Parameters.Add("@idHouse", SqlDbType.BigInt, 8).Value = _houseID;
                    WorkSession.WriteStatus("HouseSqlAdapter.Read() : 명령을 실행합니다");
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.TableMappings.Add("Table", "houseBid");
                    adapter.TableMappings.Add("Table1", "houseBidder");
                    adapter.TableMappings.Add("Table2", "house");
                    DataSet dataSet = new DataSet();
                    WorkSession.WriteStatus("HouseSqlAdapter.Read() : DataSet 에 집 정보를 채웁니다");
                    adapter.Fill(dataSet);
                    adapter.Dispose();
                    _house = HouseObjectBuilder.Build(dataSet);
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, exception.Number, _houseID);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    _house = null;
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.Read() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _houseID);
                WorkSession.WriteStatus(exception2.Message);
                _house = null;
                flag = false;
            }
            return flag;
        }

        public HouseBlockList ReadBlock(long _houseID)
        {
            HouseBlockList list2;
            WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand selectCommand = new SqlCommand("dbo.SelectHouseBlock", connection);
                    selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    selectCommand.Parameters.Add("@idHouse", SqlDbType.BigInt, 8).Value = _houseID;
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    DataSet dataSet = new DataSet();
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : DataSet 에 집 출입 정보를 채웁니다.");
                    adapter.Fill(dataSet);
                    adapter.Dispose();
                    list2 = HouseBlockObjectBuilder.Build(dataSet.Tables["Table"]);
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _houseID);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    list2 = null;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _houseID);
                WorkSession.WriteStatus(exception2.Message);
                list2 = null;
            }
            return list2;
        }

        public HouseInventory ReadItem(string _account)
        {
            HouseInventory inventory2;
            WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand selectCommand = new SqlCommand("dbo.SelectHouseItem", connection);
                    selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    selectCommand.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.TableMappings.Add("Table", "itemLarge");
                    adapter.TableMappings.Add("Table1", "itemSmall");
                    adapter.TableMappings.Add("Table2", "itemHuge");
                    adapter.TableMappings.Add("Table3", "itemQuest");
                    DataSet dataSet = new DataSet();
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : DataSet 에 집 아이템 정보를 채웁니다.");
                    adapter.Fill(dataSet);
                    adapter.Dispose();
                    inventory2 = HouseInventoryObjectBuilder.Build(dataSet.Tables["itemLarge"], dataSet.Tables["itemSmall"], dataSet.Tables["itemHuge"], dataSet.Tables["itemQuest"]);
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    inventory2 = null;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message, _account);
                ExceptionMonitor.ExceptionRaised(exception2);
                inventory2 = null;
            }
            return inventory2;
        }

        public bool UpdateBlock(long _houseID, HouseBlock[] _added, HouseBlock[] _deleted)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                StringBuilder builder = new StringBuilder(null, 100);
                if (_added != null)
                {
                    foreach (HouseBlock block in _added)
                    {
                        builder.AppendFormat("insert into house_block (houseID, gameName, flag) values({0}, {1}, {2})\n", _houseID, UpdateUtility.BuildString(block.gameName), block.flag);
                    }
                }
                if (_deleted != null)
                {
                    foreach (HouseBlock block2 in _deleted)
                    {
                        builder.AppendFormat("delete house_block where houseID={0} and gameName={1} and flag={2}\n", _houseID, UpdateUtility.BuildString(block2.gameName), block2.flag);
                    }
                }
                if (builder.ToString() != string.Empty)
                {
                    try
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 데이터베이스와 연결합니다");
                        connection.Open();
                        SqlCommand command = new SqlCommand(builder.ToString(), connection);
                        WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 명령을 실행합니다");
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception, _houseID);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 생성된 명령이 없어 종료합니다.");
                return true;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _houseID);
                WorkSession.WriteStatus(exception2.Message);
                return false;
            }
        }

        public bool UpdateItem(string _account, HouseItem _item)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("HOUSE_ITEM_UPDATE_APP");
                    SqlCommand command = new SqlCommand(ItemSqlBuilder.HouseSelfUpdateItem(_account, _item), connection);
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_ITEM_UPDATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _account, _item);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("HOUSE_ITEM_UPDATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _account, _item);
                    WorkSession.WriteStatus(exception2.Message);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _account, _item);
                WorkSession.WriteStatus(exception3.Message);
                flag = false;
            }
            return flag;
        }

        public bool Write(House _data)
        {
            bool flag;
            WorkSession.WriteStatus("HouseSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.Write() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(string.Concat(new object[] { 
                        "exec dbo.SelfUpdateHouse @idHouse=", _data.houseID, ",@strAccount=", UpdateUtility.BuildString(_data.account), ",@constructed=", _data.constructed, ",@updateTime=", UpdateUtility.BuildDateTime(_data.updateTime), ",@charName=", UpdateUtility.BuildString(_data.charName), ",@houseName=", UpdateUtility.BuildString(_data.houseName), ",@houseClass=", _data.houseClass, ",@roofSkin=", _data.roofSkin, 
                        ",@roofColor1=", _data.roofColor1, ",@roofColor2=", _data.roofColor2, ",@roofColor3=", _data.roofColor3, ",@wallSkin=", _data.wallSkin, ",@wallColor1=", _data.wallColor1, ",@wallColor2=", _data.wallColor2, ",@wallColor3=", _data.wallColor3, ",@innerSkin=", _data.innerSkin, 
                        ",@innerColor1=", _data.innerColor1, ",@innerColor2=", _data.innerColor2, ",@innerColor3=", _data.innerColor3, ",@width=", _data.width, ",@height=", _data.height, ",@bidSuccessDate=", (_data.bidSuccessDate == DateTime.MinValue) ? "null" : UpdateUtility.BuildDateTime(_data.bidSuccessDate), ",@taxPrevDate=", (_data.taxPrevDate == DateTime.MinValue) ? "null" : UpdateUtility.BuildDateTime(_data.taxPrevDate), ",@taxNextDate=", (_data.taxNextDate == DateTime.MinValue) ? "null" : UpdateUtility.BuildDateTime(_data.taxNextDate), 
                        ",@taxPrice=", _data.taxPrice, ",@taxAutopay=", _data.taxAutopay, ",@houseMoney=", _data.houseMoney, ",@deposit=", _data.deposit, ",@flag=", _data.flag
                     }), connection);
                    WorkSession.WriteStatus("HouseSqlAdapter.Write() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _data);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.Write() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message, _data);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            return flag;
        }
    }
}

