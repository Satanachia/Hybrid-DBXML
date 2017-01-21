namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class BidSqlAdapter : SqlAdapter, BidAdapter
    {
        public bool Add(Bid _bid)
        {
            bool flag;
            WorkSession.WriteStatus("BidSqlAdapter.Add() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("BidSqlAdapter.Add() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("BID_ADD_APP");
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec dbo.BidAdd  @bidID=", _bid.bidID, ",@charID=", _bid.charID, ",@charName=", UpdateUtility.BuildString(_bid.charName), ",@auctionItemID=", _bid.auctionItemID, ",@price=", _bid.price, ",@time=", UpdateUtility.BuildDateTime(_bid.time), ",@bidState=", _bid.bidState, "\n" }), connection);
                WorkSession.WriteStatus("BidSqlAdapter.Add() : 명령을 실행합니다");
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("BID_ADD_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("BID_ADD_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("BidSqlAdapter.Add() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        private BidList Build(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("경매 테이블을 얻어오지 못햇습니다.");
            }
            if ((_table.Rows == null) || (_table.Rows.Count <= 0))
            {
                return new BidList();
            }
            BidList list = new BidList();
            list.bids = new Bid[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                list.bids[i] = new Bid();
                list.bids[i].bidID = (long) _table.Rows[i]["bidID"];
                list.bids[i].charID = (long) _table.Rows[i]["charID"];
                list.bids[i].charName = (string) _table.Rows[i]["charName"];
                list.bids[i].auctionItemID = (int) _table.Rows[i]["auctionItemID"];
                list.bids[i].price = (int) _table.Rows[i]["price"];
                list.bids[i].time = (DateTime) _table.Rows[i]["time"];
                list.bids[i].bidState = (byte) _table.Rows[i]["bidState"];
            }
            return list;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Bid), _argument);
        }

        public BidList Read()
        {
            WorkSession.WriteStatus("BidSqlAdapter.Read() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand selectCommand = new SqlCommand("dbo.BidSelect", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                WorkSession.WriteStatus("BidSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "bid");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("BidSqlAdapter.Read() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                return this.Build(dataSet.Tables["bid"]);
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
            }
            finally
            {
                WorkSession.WriteStatus("BidSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return null;
        }

        public REPLY_RESULT Remove(long _bidID, ref byte _errorCode)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("BidSqlAdapter.Remove() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("BidSqlAdapter.Remove() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("BID_REMOVE_APP");
                SqlCommand command = new SqlCommand("exec dbo.BidRemove  @bidID=" + _bidID + "\n", connection);
                WorkSession.WriteStatus("BidSqlAdapter.Remove() : 명령을 실행합니다");
                command.Transaction = transaction;
                if (command.ExecuteNonQuery() > 0)
                {
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                transaction.Rollback("BID_REMOVE_APP");
                _errorCode = 0;
                fAIL = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("BID_REMOVE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("BID_REMOVE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("BidSqlAdapter.Remove() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }

        public REPLY_RESULT Update(Bid _bid, ref byte _errorCode)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("BidSqlAdapter.Update() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("BidSqlAdapter.Update() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("BID_UPDATE_APP");
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec dbo.BidUpdate  @bidID=", _bid.bidID, ",@charID=", _bid.charID, ",@charName=", UpdateUtility.BuildString(_bid.charName), ",@auctionItemID=", _bid.auctionItemID, ",@price=", _bid.price, ",@time=", UpdateUtility.BuildDateTime(_bid.time), ",@bidState=", _bid.bidState, "\n" }), connection);
                WorkSession.WriteStatus("BidSqlAdapter.Update() : 명령을 실행합니다");
                command.Transaction = transaction;
                if (command.ExecuteNonQuery() > 0)
                {
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                transaction.Rollback("BID_UPDATE_APP");
                _errorCode = 0;
                fAIL = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("BID_UPDATE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("BID_UPDATE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("BidSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }
    }
}

