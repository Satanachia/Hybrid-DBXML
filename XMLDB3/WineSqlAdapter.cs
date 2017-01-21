namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class WineSqlAdapter : SqlAdapter, WineAdapter
    {
        public bool Delete(long _charID)
        {
            bool flag;
            WorkSession.WriteStatus("WineSqlAdapter.Delete() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("WineSqlAdapter.Delete() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("WINE_DELETE_APP");
                SqlCommand command = new SqlCommand("dbo.WineDelete", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idChar", SqlDbType.BigInt, 8).Value = _charID;
                command.Transaction = transaction;
                WorkSession.WriteStatus("WineSqlAdapter.Delete() : 명령을 실행합니다");
                command.ExecuteScalar();
                WorkSession.WriteStatus("WineSqlAdapter.Delete() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("WineSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("WINE_DELETE_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _charID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("WineSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("WINE_DELETE_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("WineSqlAdapter.Delete() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Wine), _argument);
        }

        public REPLY_RESULT Read(long _charID, out Wine _wine)
        {
            REPLY_RESULT fAIL;
            _wine = null;
            WorkSession.WriteStatus("WineSqlAdapter.Read() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand command = new SqlCommand("dbo.WineSelect", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idChar", SqlDbType.BigInt, 8).Value = _charID;
                WorkSession.WriteStatus("WineSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                connection.Open();
                WorkSession.WriteStatus("WineSqlAdapter.Read() : 데이터를 채웁니다.");
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        _wine = new Wine();
                        _wine.charID = _charID;
                        _wine.wineType = (byte) reader["wineType"];
                        _wine.agingCount = (short) reader["agingCount"];
                        _wine.agingStartTime = (DateTime) reader["agingStartTime"];
                        _wine.lastRackingTime = (DateTime) reader["lastRackingTime"];
                        _wine.acidity = (int) reader["acidity"];
                        _wine.purity = (int) reader["purity"];
                        _wine.freshness = (int) reader["freshness"];
                        return REPLY_RESULT.SUCCESS;
                    }
                    fAIL = REPLY_RESULT.FAIL_EX;
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
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _charID);
                WorkSession.WriteStatus(exception2.Message);
                fAIL = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("WineSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return fAIL;
        }

        public bool Update(Wine _wine)
        {
            bool flag;
            WorkSession.WriteStatus("WineSqlAdapter.Update() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("WineSqlAdapter.Update() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("WINE_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.WineUpdate", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idChar", SqlDbType.BigInt).Value = _wine.charID;
                command.Parameters.Add("@wineType", SqlDbType.TinyInt).Value = _wine.wineType;
                command.Parameters.Add("@agingCount", SqlDbType.SmallInt).Value = _wine.agingCount;
                command.Parameters.Add("@agingStartTime", SqlDbType.DateTime).Value = UpdateUtility.BuildDateTime(_wine.agingStartTime);
                command.Parameters.Add("@lastRackingTime", SqlDbType.DateTime).Value = UpdateUtility.BuildDateTime(_wine.lastRackingTime);
                command.Parameters.Add("@acidity", SqlDbType.Int).Value = _wine.acidity;
                command.Parameters.Add("@purity", SqlDbType.Int).Value = _wine.purity;
                command.Parameters.Add("@freshness", SqlDbType.Int).Value = _wine.freshness;
                command.Transaction = transaction;
                WorkSession.WriteStatus("WineSqlAdapter.Update() : 명령을 실행합니다");
                command.ExecuteScalar();
                WorkSession.WriteStatus("WineSqlAdapter.Update() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _wine.charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("WineSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("WINE_UPDATE_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _wine.charID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("WineSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("WINE_UPDATE_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("WineSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

