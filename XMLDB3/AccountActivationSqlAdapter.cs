namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class AccountActivationSqlAdapter : SqlAdapter, AccountActivationAdapter
    {
        public bool Create(AccountActivation _data)
        {
            bool flag;
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("AccountActivationSqlAdapter.Create() : 데이터 베이스에 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("CreateAccountActivation", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter parameter = command.Parameters.Add("@id", SqlDbType.NVarChar, 50);
                    SqlParameter parameter2 = command.Parameters.Add("@pass", SqlDbType.NVarChar, 50);
                    SqlParameter parameter3 = command.Parameters.Add("@name", SqlDbType.NVarChar, 50);
                    SqlParameter parameter4 = command.Parameters.Add("@ssn", SqlDbType.NVarChar, 0x40);
                    SqlParameter parameter5 = command.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                    SqlParameter parameter6 = command.Parameters.Add("@flag", SqlDbType.SmallInt, 2);
                    SqlParameter parameter7 = command.Parameters.Add("@blocking_date", SqlDbType.DateTime);
                    SqlParameter parameter8 = command.Parameters.Add("@blocking_duration", SqlDbType.SmallInt, 2);
                    SqlParameter parameter9 = command.Parameters.Add("@auth", SqlDbType.TinyInt);
                    SqlParameter parameter10 = command.Parameters.Add("@provider_code", SqlDbType.SmallInt, 2);
                    parameter.Value = _data.id;
                    parameter2.Value = _data.password;
                    parameter3.Value = _data.name;
                    parameter4.Value = _data.serialnumber;
                    parameter5.Value = _data.email;
                    parameter6.Value = _data.flag;
                    parameter7.Value = _data.blocking_date;
                    parameter8.Value = _data.blocking_duration;
                    parameter9.Value = _data.authority;
                    parameter10.Value = _data.provider_code;
                    WorkSession.WriteStatus("AccountActivationSqlAdapter.Create() : 명령을 수행합니다");
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
                    WorkSession.WriteStatus("AccountActivationSqlAdapter.Create() : 데이터 베이스에 연결을 해제합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _data);
                WorkSession.WriteStatus(exception2.Message, _data);
                flag = false;
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(AccountActivation), _argument);
        }
    }
}

