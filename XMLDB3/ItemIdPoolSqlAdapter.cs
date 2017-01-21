namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class ItemIdPoolSqlAdapter : SqlAdapter, ItemIdPoolAdapter
    {
        public long GetIdPool()
        {
            long count;
            WorkSession.WriteStatus("ItemIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");
            try
            {
                ItemIdPoolMutex.Enter();
                ItemIDPool pool = (ItemIDPool) base.Read(0);
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UpdateItemIdPool", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@count", SqlDbType.BigInt, 8).Value = pool.count + 0x3e8L;
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
            finally
            {
                ItemIdPoolMutex.Leave();
            }
            return count;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("SelectItemIdPool", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(ItemIDPool), _argument);
        }
    }
}

