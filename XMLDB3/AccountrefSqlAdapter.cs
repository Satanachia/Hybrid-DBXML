namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;

    public class AccountrefSqlAdapter : SqlAdapter, AccountRefAdapter
    {
        public bool CacheKeyCheck(string _account, int _cacheKey, out int _oldCacheKey)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec UpdateCacheKey @account = \"", _account, "\", @key = ", _cacheKey }), connection);
                WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 명령을 실행합니다");
                _oldCacheKey = (int) command.ExecuteScalar();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _oldCacheKey = 0;
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message, _account);
                ExceptionMonitor.ExceptionRaised(exception2);
                _oldCacheKey = 0;
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool Create(Accountref _data)
        {
            return base.Create(_data);
        }

        protected override SqlCommand GetCreateProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("CreateAccountref2", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlAccountref", SqlDbType.NText, 0xfa0).Value = _argument;
            return command;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("SelectAccountref2", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idAccountref", SqlDbType.VarChar, 0xff).Value = _argument;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Accountref), _argument);
        }

        public bool PlayIn(string _account, int _remainTime)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "update Accountref set [in] = getdate(), playabletime=", _remainTime, " where id = '", _account, "'" }), connection);
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 명령을 실행합니다");
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
                ExceptionMonitor.ExceptionRaised(exception2, _account);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool PlayOut(string _account, int _remainTime, string _server, GroupIDList _charGroupID, GroupIDList _petGroupID, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, byte _macroCheckFailure, byte _macroCheckSuccess, bool _beginnerFlag)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                object obj2;
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("PLAYOUT_APP");
                string cmdText = string.Concat(new object[] { 
                    "exec dbo.PlayOut  @strAccount=", UpdateUtility.BuildString(_account), ",@playableTime=", _remainTime, ",@supportRace=", _supportRace, ",@supportRewardState=", _supportRewardState, ",@supportLastChangeTime=", _supportLastChangeTime, ",@macroCheckFailure=", _macroCheckFailure, ",@macroCheckSuccess=", _macroCheckSuccess, ",@beginnerFlag=", _beginnerFlag ? "1" : "0", 
                    "\n"
                 });
                if (_charGroupID.group != null)
                {
                    foreach (GroupID pid in _charGroupID.group)
                    {
                        obj2 = cmdText;
                        cmdText = string.Concat(new object[] { obj2, "exec dbo.WriteCharacterGroup  @strAccount=", UpdateUtility.BuildString(_account), ",@idCharacter=", pid.charID, ",@strServer=", UpdateUtility.BuildString(_server), ",@groupID=", pid.groupID, "\n" });
                    }
                }
                if (_petGroupID.group != null)
                {
                    foreach (GroupID pid2 in _petGroupID.group)
                    {
                        obj2 = cmdText;
                        cmdText = string.Concat(new object[] { obj2, "exec dbo.WritePetGroup  @strAccount=", UpdateUtility.BuildString(_account), ",@idPet=", pid2.charID, ",@strServer=", UpdateUtility.BuildString(_server), ",@groupID=", pid2.groupID, "\n" });
                    }
                }
                SqlCommand command = new SqlCommand(cmdText, connection);
                command.Transaction = transaction;
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("PLAYOUT_APP");
                    }
                }
                finally
                {
                    ExceptionMonitor.ExceptionRaised(exception, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("PLAYOUT_APP");
                    }
                }
                finally
                {
                    ExceptionMonitor.ExceptionRaised(exception2, _account);
                    WorkSession.WriteStatus(exception2.Message);
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public Accountref Read(string _id)
        {
            return (Accountref) base.Read(_id);
        }

        public bool SetFlag(string _account, int _flag)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("UpdateAccountRefFlag", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter parameter = command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50);
                    SqlParameter parameter2 = command.Parameters.Add("@flag", SqlDbType.Int, 4);
                    parameter.Value = _account;
                    parameter2.Value = _flag;
                    WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public bool SetLobbyOption(string _account, int _lobbyOption, LobbyTabList _charLobbyTabList, LobbyTabList _petLobbyTabList)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 데이터베이스와 연결합니다");
                connection.Open();
                StringBuilder builder = new StringBuilder();
                builder.AppendFormat("exec dbo.WriteLobbyOption @strAccount={0}, @lobbyOption={1}\n", UpdateUtility.BuildString(_account), _lobbyOption);
                if ((_charLobbyTabList.tabInfo != null) && (_charLobbyTabList.tabInfo.Length <= 80))
                {
                    foreach (LobbyTab tab in _charLobbyTabList.tabInfo)
                    {
                        builder.AppendFormat("exec dbo.UpdateCharacterLobbytab @strAccount={0}, @idCharacter={1}, @server={2}, @tab={3}\n", new object[] { UpdateUtility.BuildString(_account), tab.charID, UpdateUtility.BuildString(tab.server), tab.tab ? "1" : "0" });
                    }
                }
                if ((_petLobbyTabList.tabInfo != null) && (_petLobbyTabList.tabInfo.Length <= 80))
                {
                    foreach (LobbyTab tab2 in _petLobbyTabList.tabInfo)
                    {
                        builder.AppendFormat("exec dbo.UpdatePetLobbytab @strAccount={0}, @idPet={1}, @server={2}, @tab={3}\n", new object[] { UpdateUtility.BuildString(_account), tab2.charID, UpdateUtility.BuildString(tab2.server), tab2.tab ? "1" : "0" });
                    }
                }
                SqlCommand command = new SqlCommand(builder.ToString(), connection);
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 명령을 실행합니다");
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
                ExceptionMonitor.ExceptionRaised(exception2, _account);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool SetPetSlotFlag(string _account, long _id, string _server, long _flag)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetPetSlotFlag() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetPetSlotFlag() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "update account_pet_ref set deleted = ", _flag, " where id = '", _account, "' and petid = ", _id, " and server = '", _server, "'" }), connection);
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetPetSlotFlag() : 명령을 실행합니다");
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
                ExceptionMonitor.ExceptionRaised(exception2, _account);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetPetSlotFlag() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool SetSlotFlag(string _account, long _id, string _server, long _flag)
        {
            bool flag;
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetSlotFlag() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetSlotFlag() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "update account_character_ref set deleted = ", _flag, " where id = '", _account, "' and characterid = ", _id, " and server = '", _server, "'" }), connection);
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetSlotFlag() : 명령을 실행합니다");
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
                ExceptionMonitor.ExceptionRaised(exception2, _account);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetSlotFlag() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

