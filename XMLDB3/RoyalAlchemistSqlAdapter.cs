namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class RoyalAlchemistSqlAdapter : SqlAdapter, RoyalAlchemistAdapter
    {
        public REPLY_RESULT Add(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("ROYALALCHEMIST_ADD_APP");
                SqlCommand command = new SqlCommand("dbo.RoyalAlchemistAdd", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@charID", SqlDbType.BigInt).Value = _royalAlchemist.charID;
                command.Parameters.Add("@charName", SqlDbType.NVarChar, 50).Value = _royalAlchemist.charName;
                command.Parameters.Add("@registrationFlag", SqlDbType.SmallInt).Value = _royalAlchemist.registrationFlag;
                command.Parameters.Add("@rank", SqlDbType.SmallInt).Value = _royalAlchemist.rank;
                command.Parameters.Add("@meta", SqlDbType.NVarChar, 200).Value = _royalAlchemist.meta;
                command.Transaction = transaction;
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 명령을 실행합니다");
                command.ExecuteScalar();
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _royalAlchemist.charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("ROYALALCHEMIST_ADD_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _royalAlchemist.charID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("ROYALALCHEMIST_ADD_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        private RoyalAlchemistList Build(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("왕성 연금술사 테이블을 얻어오지 못햇습니다.");
            }
            if ((_table.Rows == null) || (_table.Rows.Count <= 0))
            {
                return new RoyalAlchemistList();
            }
            RoyalAlchemistList list = new RoyalAlchemistList();
            list.alchemists = new RoyalAlchemist[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                list.alchemists[i] = new RoyalAlchemist();
                list.alchemists[i].charID = (long) _table.Rows[i]["charID"];
                list.alchemists[i].charName = (string) _table.Rows[i]["charName"];
                list.alchemists[i].registrationFlag = (byte) _table.Rows[i]["registrationFlag"];
                list.alchemists[i].rank = (ushort) ((short) _table.Rows[i]["rank"]);
                list.alchemists[i].meta = (string) _table.Rows[i]["meta"];
            }
            return list;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(RoyalAlchemist), _argument);
        }

        public RoyalAlchemist Read(long _charID)
        {
            RoyalAlchemist alchemist2;
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand command = new SqlCommand("dbo.RoyalAlchemistSelect", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@charID", SqlDbType.BigInt).Value = _charID;
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                connection.Open();
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 데이터를 채웁니다.");
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        RoyalAlchemist alchemist = new RoyalAlchemist();
                        alchemist.charID = _charID;
                        alchemist.charName = (string) reader["charName"];
                        alchemist.registrationFlag = (byte) reader["registrationFlag"];
                        alchemist.rank = (ushort) ((short) reader["rank"]);
                        alchemist.meta = (string) reader["meta"];
                        return alchemist;
                    }
                    alchemist2 = null;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                alchemist2 = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _charID);
                WorkSession.WriteStatus(exception2.Message);
                alchemist2 = null;
            }
            finally
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return alchemist2;
        }

        public RoyalAlchemistList ReadList()
        {
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand selectCommand = new SqlCommand("dbo.RoyalAlchemistSelectAll", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "RoyalAlchemist");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                return this.Build(dataSet.Tables["RoyalAlchemist"]);
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
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return null;
        }

        public REPLY_RESULT Remove(long[] _removeIDs, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Delete() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("ROYALALCHEMIST_REMOVE_APP");
                foreach (long num in _removeIDs)
                {
                    SqlCommand command = new SqlCommand("dbo.RoyalAlchemistRemove", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@charID", SqlDbType.BigInt).Value = num;
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 명령을 실행합니다");
                    command.ExecuteScalar();
                }
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("ROYALALCHEMIST_REMOVE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("ROYALALCHEMIST_REMOVE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        public REPLY_RESULT Update(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("ROYALALCHEMIST_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.RoyalAlchemistUpdate", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@charID", SqlDbType.BigInt).Value = _royalAlchemist.charID;
                command.Parameters.Add("@charName", SqlDbType.NVarChar, 50).Value = _royalAlchemist.charName;
                command.Parameters.Add("@registrationFlag", SqlDbType.SmallInt).Value = _royalAlchemist.registrationFlag;
                command.Parameters.Add("@rank", SqlDbType.SmallInt).Value = _royalAlchemist.rank;
                command.Parameters.Add("@meta", SqlDbType.NVarChar, 200).Value = _royalAlchemist.meta;
                command.Transaction = transaction;
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 명령을 실행합니다");
                command.ExecuteScalar();
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _royalAlchemist.charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("ROYALALCHEMIST_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _royalAlchemist.charID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("ROYALALCHEMIST_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }
    }
}

