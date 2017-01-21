namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class HuskySqlAdapter : HuskyAdapter
    {
        private string strConnection;

        public bool Callprocedure(string _account, long _charId, string _charName)
        {
            WorkSession.WriteStatus("HuskySqlAdapter.Callprocedure() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(this.strConnection);
            try
            {
                SqlCommand command = new SqlCommand("__update_mabi_event_history", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@account", SqlDbType.VarChar, 0x40).Value = _account;
                command.Parameters.Add("@characterid", SqlDbType.BigInt, 8).Value = _charId;
                command.Parameters.Add("@charactername", SqlDbType.NVarChar, 0x40).Value = _charName;
                WorkSession.WriteStatus("HuskySqlAdapter.Callprocedure() : 데이터 베이스에 연결합니다.");
                connection.Open();
                command.ExecuteScalar();
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
                WorkSession.WriteStatus("HuskySqlAdapter.Callprocedure() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return true;
        }

        public void Initialize(string _argument)
        {
            this.strConnection = _argument;
        }
    }
}

