namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class AccountSqlAdapter : SqlAdapter, AccountAdapter
    {
        public bool Ban(string _account, short _bantype, string _manager, short _duration, string _purpose)
        {
            bool flag;
            WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("GameBanAccount", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@account", SqlDbType.NVarChar, 50).Value = _account;
                command.Parameters.Add("@blockType", SqlDbType.Int, 4).Value = _bantype;
                command.Parameters.Add("@blockTime", SqlDbType.Int, 4).Value = _duration;
                command.Parameters.Add("@blockText", SqlDbType.NVarChar, 200).Value = _purpose;
                command.Parameters.Add("@AdminAccount", SqlDbType.NVarChar, 50).Value = _manager;
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool Create(Account _data)
        {
            return base.Create(_data);
        }

        protected override SqlCommand GetCreateProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("CreateAccount", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlAccount", SqlDbType.VarChar, 0xfa0).Value = _argument;
            return command;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("SelectAccount2", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idAccount", SqlDbType.VarChar, 0xff).Value = _argument;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Account), _argument);
        }

        public bool LoginSignal(string _account, long _sessionKey, string _address, int _ispCode)
        {
            bool flag;
            WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("LoginSignal", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@strAccount", SqlDbType.NChar, 0x20).Value = _account;
                command.Parameters.Add("@biSessionKey", SqlDbType.BigInt, 9).Value = _sessionKey;
                command.Parameters.Add("@strAddress", SqlDbType.NChar, 0x10).Value = _address;
                command.Parameters.Add("@intISPCode", SqlDbType.Int, 4).Value = _ispCode;
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool LogoutSignal(string _account)
        {
            bool flag;
            WorkSession.WriteStatus("AccountSqlAdapter.LogoutSignal() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LogoutSignal() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("AccountSqlAdapter.LogoutSignal() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("LogoutSignal", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@strAccount", SqlDbType.NChar, 0x20).Value = _account;
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public Account Read(string _id)
        {
            return (Account) base.Read(_id);
        }

        public Account ReadSMS(string _id)
        {
            SqlCommand command = new SqlCommand("SelectAccountSMS");
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idAccount", SqlDbType.VarChar, 0xff).Value = _id;
            Account account = (Account) base.Read(command);
            if ((account != null) && (account.SMSAuth != null))
            {
                if (account.SMSAuth.cPhone == null)
                {
                    account.SMSAuth.cPhone = string.Empty;
                }
                if (account.SMSAuth.carrier == null)
                {
                    account.SMSAuth.carrier = string.Empty;
                }
                if (account.SMSAuth.lastIP == null)
                {
                    account.SMSAuth.lastIP = string.Empty;
                }
            }
            return account;
        }

        public bool Unban(string _account, string _manager)
        {
            bool flag;
            WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("GameUnBanAccount", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@account", SqlDbType.NVarChar, 50).Value = _account;
                command.Parameters.Add("@AdminAccount", SqlDbType.NVarChar, 50).Value = _manager;
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

