namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class ChannelingKeyPoolSqlAdapter : SqlAdapter, ChannelingKeyPoolAdapter
    {
        public bool Do(ChannelingKey _chKey)
        {
            bool flag;
            if (_chKey == null)
            {
                return false;
            }
            WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 함수에 진입하였습니다");
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction("CHANNELING_KEY_POOL_INSERT");
                SqlCommand command = new SqlCommand("InsertChannelingKey", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@provider_code", SqlDbType.SmallInt, 2).Value = _chKey.provider;
                command.Parameters.Add("@keystring", SqlDbType.VarChar, 0xff).Value = _chKey.keystring;
                command.Transaction = transaction;
                WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 명령을 실행합니다");
                int num = command.ExecuteNonQuery();
                WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                if (num > 0)
                {
                    return true;
                }
                flag = false;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("CHANNELING_KEY_POOL_INSERT");
                }
                ExceptionMonitor.ExceptionRaised(exception, _chKey);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("CHANNELING_KEY_POOL_INSERT");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _chKey);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(ChannelingKey), _argument);
        }
    }
}

