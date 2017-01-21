namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class LoginIdPoolSqlAdapter : SqlAdapter, LoginIdPoolAdapter
    {
        public long GetIdPool(int _size)
        {
            long count;
            WorkSession.WriteStatus("LoginIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");
            try
            {
                LoginIDPool pool = (LoginIDPool) base.Read(0);
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UpdateLoginIdPool", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@count", SqlDbType.BigInt, 8).Value = pool.count + _size;
                    command.ExecuteNonQuery();
                    count = pool.count;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    count = 0L;
                }
                finally
                {
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                count = 0L;
            }
            return count;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("SelectLoginIdPool", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(LoginIDPool), _argument);
        }
    }
}

