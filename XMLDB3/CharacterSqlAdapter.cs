namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class CharacterSqlAdapter : SqlAdapter, CharacterAdapter
    {
        private CharacterInfo _Read(long _id)
        {
            CharacterInfo info2;
            WorkSession.WriteStatus("CharacterSqlAdapter._Read() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            string cmdText = "exec dbo.SelectCharacter4 @idCharacter=" + _id;
            if (ConfigManager.IsPVPable)
            {
                cmdText = cmdText + "\nexec dbo.SelectCharacterPVP @idCharacter=" + _id;
            }
            SqlCommand selectCommand = new SqlCommand(cmdText, connection);
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter._Read() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "character");
                adapter.TableMappings.Add("Table1", "CharacterQuest");
                adapter.TableMappings.Add("Table2", "character_skill2");
                adapter.TableMappings.Add("Table3", "CharItemLarge");
                adapter.TableMappings.Add("Table4", "CharItemSmall");
                adapter.TableMappings.Add("Table5", "CharItemHuge");
                adapter.TableMappings.Add("Table6", "CharItemQuest");
                adapter.TableMappings.Add("Table7", "CharItemEgo");
                adapter.TableMappings.Add("Table8", "CharacterAchievement");
                adapter.TableMappings.Add("Table9", "character_pvp");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("CharacterSqlAdapter._Read() : DataSet 에 캐릭터 정보를 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharacterRead, Stopwatch.GetElapsedMilliseconds(timestamp));
                WorkSession.WriteStatus("CharacterSqlAdapter._Read() : DataSet 으로부터 캐릭터 정보를 생성합니다");
                info2 = CharacterObjectBuilder.Build(dataSet);
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter._Read() : 연결을 종료합니다");
                connection.Close();
            }
            return info2;
        }

        private bool _Write(CharacterInfo _character, CharacterInfo _cache, bool _forceLinkUpdate)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 저장되어 있는 데이터를 읽어옵니다");
                if (_cache == null)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                    _cache = this._Read(_character.id);
                }
                else if (!this.IsValidCache(_cache))
                {
                    _cache = this._Read(_character.id);
                }
                string str = CharacterUpdateBuilder.Build(_character, _cache) + InventoryUpdateBuilder.Build(_character.id, _character.inventory, _cache.inventory, _forceLinkUpdate);
                if (str != string.Empty)
                {
                    SqlConnection connection = new SqlConnection(base.ConnectionString);
                    SqlTransaction transaction = null;
                    try
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();
                        connection.Open();
                        transaction = connection.BeginTransaction("CHARACTER_UPDATE_APP");
                        _character.updatetime = this.Update(_character.id, str, connection, transaction);
                        transaction.Commit();
                        CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharacterWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                        return true;
                    }
                    catch (SqlException exception)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback("CHARACTER_UPDATE_APP");
                        }
                        if (!_forceLinkUpdate && ItemSqlBuilder.ForceUpdateRetry(exception))
                        {
                            WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 아이템 오류로 재시도합니다.");
                            ExceptionMonitor.ExceptionRaised(exception, _character, str);
                            WorkSession.WriteStatus(exception.Message, exception.Number);
                            connection.Close();
                            return this._Write(_character, null, true);
                        }
                        ExceptionMonitor.ExceptionRaised(exception, _character, str);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        return false;
                    }
                    catch (Exception exception2)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback("CHARACTER_UPDATE_APP");
                        }
                        ExceptionMonitor.ExceptionRaised(exception2, _character, str);
                        WorkSession.WriteStatus(exception2.Message);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 변경점이 없습니다. 쿼리를 생략합니다");
                return true;
            }
            catch (SqlException exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _character);
                WorkSession.WriteStatus(exception3.Message, exception3.Number);
                return false;
            }
            catch (Exception exception4)
            {
                ExceptionMonitor.ExceptionRaised(exception4, _character);
                WorkSession.WriteStatus(exception4.Message);
                return false;
            }
        }

        private uint Build(DataTable _table)
        {
            if ((_table != null) && ((_table.Rows != null) && (_table.Rows.Count > 0)))
            {
                return (uint) ((int) _table.Rows[0]["totalLevel"]);
            }
            return 0;
        }

        public bool CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo _data, AccountRefAdapter _accountref, BankAdapter _bank, WebSynchAdapter _websynch)
        {
            bool flag;
            WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _accountref).ConnectionString);
            SqlTransaction transaction2 = null;
            SqlConnection connection3 = new SqlConnection(((SqlAdapter) _bank).ConnectionString);
            SqlTransaction transaction3 = null;
            SqlConnection connection4 = new SqlConnection(((SqlAdapter) _websynch).ConnectionString);
            SqlTransaction transaction4 = null;
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand("dbo.CheckUsableName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@name", SqlDbType.VarChar, 0x40).Value = _data.name;
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 캐릭터 이름 중복 검사를 실행합니다");
                if (((int) command.ExecuteScalar()) == 0)
                {
                    ExceptionMonitor.ExceptionRaised(new Exception("캐릭터 이름이 중복됩니다."), _account, _data.name);
                    return false;
                }
                transaction = connection.BeginTransaction("CHARACTER_CREATE_EX_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CHARACTER_CREATE_EX_APP");
                connection3.Open();
                transaction3 = connection3.BeginTransaction("CHARACTER_CREATE_EX_APP");
                connection4.Open();
                transaction4 = connection4.BeginTransaction("CHARACTER_CREATE_EX_APP");
                SqlCommand command2 = new SqlCommand(CharacterCreateBuilder.Build(_data), connection);
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 캐릭터 생성 명령을 실행합니다");
                command2.Transaction = transaction;
                command2.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 뱅크 생성 명령을 실행합니다");
                SqlCommand command3 = new SqlCommand(string.Concat(new object[] { "exec dbo.AddBankSlot  @strAccount=", UpdateUtility.BuildString(_account), ",@slotname=", UpdateUtility.BuildString(_data.name), ",@race=", _race, "\n" }), connection3);
                command3.Transaction = transaction3;
                command3.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 계정-캐릭터 링크 생성 명령을 실행합니다");
                object obj2 = string.Concat(new object[] { "exec dbo.AddAccountrefCharacter  @strAccount=", UpdateUtility.BuildString(_account), ",@idCharacter=", _data.id, ",@name=", UpdateUtility.BuildString(_data.name), ",@server=", UpdateUtility.BuildString(_server), ",@race=", _race, ",@supportCharacter=", _supportCharacter ? 1 : 0, "\n" });
                SqlCommand command4 = new SqlCommand(string.Concat(new object[] { obj2, "exec dbo.WriteAccountSupportRewardState  @strAccount=", UpdateUtility.BuildString(_account), ",@supportRewardState=", _supportRewardState, "\n" }), connection2);
                command4.Transaction = transaction2;
                command4.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 웹싱크 명령을 실행합니다");
                SqlCommand command5 = new SqlCommand(string.Concat(new object[] { "exec WebSynch_AddCharacter @strAccount = ", UpdateUtility.BuildString(_account), " ,@idCharacter = ", _data.id, " ,@strName = ", UpdateUtility.BuildString(_data.name), " ,@strServer = ", UpdateUtility.BuildString(_server) }), connection4);
                command5.Transaction = transaction4;
                command5.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                transaction2.Commit();
                transaction3.Commit();
                transaction4.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback("CHARACTER_CREATE_EX_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("CHARACTER_CREATE_EX_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("CHARACTER_CREATE_EX_APP");
                }
                if (transaction4 != null)
                {
                    transaction4.Rollback("CHARACTER_CREATE_EX_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _account, _data);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    transaction.Rollback("CHARACTER_CREATE_EX_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("CHARACTER_CREATE_EX_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("CHARACTER_CREATE_EX_APP");
                }
                if (transaction4 != null)
                {
                    transaction4.Rollback("CHARACTER_CREATE_EX_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _account, _data);
                WorkSession.WriteStatus(exception2.Message, _account);
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 연결을 종료합니다");
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection2.State == ConnectionState.Open)
                {
                    connection2.Close();
                }
                if (connection3.State == ConnectionState.Open)
                {
                    connection3.Close();
                }
                if (connection4.State == ConnectionState.Open)
                {
                    connection4.Close();
                }
            }
            return flag;
        }

        public bool DeleteEx(string _account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string _server, long _idcharacter, AccountRefAdapter _accountref, BankAdapter _bank, GuildAdapter _guild, WebSynchAdapter _websynch)
        {
            bool flag;
            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _accountref).ConnectionString);
            SqlTransaction transaction2 = null;
            SqlConnection connection3 = new SqlConnection(((SqlAdapter) _bank).ConnectionString);
            SqlTransaction transaction3 = null;
            SqlConnection connection4 = new SqlConnection(((SqlAdapter) _guild).ConnectionString);
            SqlTransaction transaction4 = null;
            SqlConnection connection5 = new SqlConnection(((SqlAdapter) _websynch).ConnectionString);
            SqlTransaction transaction5 = null;
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CHARACTER_DELETE_EX_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CHARACTER_DELETE_EX_APP");
                connection3.Open();
                transaction3 = connection3.BeginTransaction("CHARACTER_DELETE_EX_APP");
                connection4.Open();
                transaction4 = connection4.BeginTransaction("CHARACTER_DELETE_EX_APP");
                connection5.Open();
                transaction5 = connection5.BeginTransaction("CHARACTER_DELETE_EX_APP");
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 뱅크 삭제 명령을 실행합니다");
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec dbo.RemoveBankSlot ", UpdateUtility.BuildString(_account), ",", _idcharacter }), connection3);
                command.Transaction = transaction3;
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 계정-캐릭터 링크를 제거명령을 실행합니다");
                object obj2 = string.Concat(new object[] { "exec dbo.DeleteAccountrefCharacter  @strAccount=", UpdateUtility.BuildString(_account), ",@idCharacter=", _idcharacter, ",@server=", UpdateUtility.BuildString(_server), "\n" });
                SqlCommand command2 = new SqlCommand(string.Concat(new object[] { obj2, "exec dbo.WriteAccountSupportState  @strAccount=", UpdateUtility.BuildString(_account), ",@supportRace=", _supportRace, ",@supportRewardState=", _supportRewardState, ",@supportLastChangeTime=", _supportLastChangeTime, "\n" }), connection2);
                command2.Transaction = transaction2;
                command2.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 길드 멤버 제거명령을 실행합니다");
                SqlCommand command3 = new SqlCommand(string.Concat(new object[] { "exec dbo.RemoveGuildMemberEx @memberid = ", _idcharacter, " ,@server = ", UpdateUtility.BuildString(_server) }), connection4);
                command3.Transaction = transaction4;
                command3.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 웹 싱크 명령을 실행합니다");
                SqlCommand command4 = new SqlCommand(string.Concat(new object[] { "exec dbo.WebSynch_RemoveCharacter @strAccount = ", UpdateUtility.BuildString(_account), " ,@idCharacter = ", _idcharacter, " ,@strServer = ", UpdateUtility.BuildString(_server) }), connection5);
                command4.Transaction = transaction5;
                command4.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 캐릭터 제거 명령을 실행합니다");
                SqlCommand deleteProcedure = this.GetDeleteProcedure(_idcharacter, connection);
                deleteProcedure.Transaction = transaction;
                deleteProcedure.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                transaction2.Commit();
                transaction3.Commit();
                transaction4.Commit();
                transaction5.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 롤백합니다");
                if (transaction != null)
                {
                    transaction.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction4 != null)
                {
                    transaction4.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction5 != null)
                {
                    transaction5.Rollback("CHARACTER_DELETE_EX_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _account, _idcharacter);
                WorkSession.WriteStatus(exception.Message, _account);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 롤백합니다");
                if (transaction != null)
                {
                    transaction.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction4 != null)
                {
                    transaction4.Rollback("CHARACTER_DELETE_EX_APP");
                }
                if (transaction5 != null)
                {
                    transaction5.Rollback("CHARACTER_DELETE_EX_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _account, _idcharacter);
                WorkSession.WriteStatus(exception2.Message, _account);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 연결을 종료합니다");
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                if (connection2.State == ConnectionState.Open)
                {
                    connection2.Close();
                }
                if (connection3.State == ConnectionState.Open)
                {
                    connection3.Close();
                }
                if (connection4.State == ConnectionState.Open)
                {
                    connection4.Close();
                }
                if (connection5.State == ConnectionState.Open)
                {
                    connection5.Close();
                }
            }
            return flag;
        }

        public bool DeleteItem(long _id, ItemList[] _list)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {
                string cmdText = string.Empty;
                if ((_list != null) && (_list.Length > 0))
                {
                    foreach (ItemList list in _list)
                    {
                        cmdText = cmdText + ItemSqlBuilder.DeleteItem(_id, list.itemID, list.storedtype);
                    }
                }
                if (cmdText != string.Empty)
                {
                    SqlConnection connection = new SqlConnection(base.ConnectionString);
                    SqlTransaction transaction = null;
                    try
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();
                        connection.Open();
                        transaction = connection.BeginTransaction("CHARACTER_ITEM_DELETE_APP");
                        SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                        WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 명령을 수행합니다.");
                        command.ExecuteNonQuery();
                        transaction.Commit();
                        CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharItemDelete, Stopwatch.GetElapsedMilliseconds(timestamp));
                        return true;
                    }
                    catch (SqlException exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception, _list);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        if (transaction != null)
                        {
                            transaction.Rollback("CHARACTER_ITEM_DELETE_APP");
                        }
                        return false;
                    }
                    catch (Exception exception2)
                    {
                        ExceptionMonitor.ExceptionRaised(exception2, _list);
                        WorkSession.WriteStatus(exception2.Message);
                        if (transaction != null)
                        {
                            transaction.Rollback("CHARACTER_ITEM_DELETE_APP");
                        }
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 삭제할 아이템이 없습니다.");
                return true;
            }
            catch (Exception exception3)
            {
                WorkSession.WriteStatus(exception3.Message);
                ExceptionMonitor.ExceptionRaised(exception3);
                return false;
            }
        }

        public uint GetAccumLevel(string _account, string _name)
        {
            SqlCommand command;
            uint num;
            WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            if ((_account.Length > 0) && (_name.Length > 0))
            {
                command = new SqlCommand("dbo.GetCharacterCumulatedLevel", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@characterName", SqlDbType.VarChar, 50).Value = _name;
            }
            else
            {
                return 0;
            }
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.TableMappings.Add("Table", "Character");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                num = this.Build(dataSet.Tables["Character"]);
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account + " / " + _name);
                WorkSession.WriteStatus(exception.Message, _account + " / " + _name);
                num = 0;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account + " / " + _name);
                WorkSession.WriteStatus(exception2.Message, _account + " / " + _name);
                num = 0;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 연결을 종료합니다");
                connection.Close();
            }
            return num;
        }

        protected override SqlCommand GetCreateProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("dbo.CreateCharacter2", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlCharacter", SqlDbType.NText, 0xf4240).Value = _argument;
            return command;
        }

        protected override SqlCommand GetDeleteProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("dbo.DeleteCharacter2", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idCharacter", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            bool flag;
            WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlCommand command = new SqlCommand("dbo.GetCharacterWriteCounter", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@id", SqlDbType.BigInt, 8).Value = _id;
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 데이터베이스와 연결합니다");
                connection.Open();
                _counter = (byte) command.ExecuteScalar();
                WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 저장 카운터를 읽었습니다.");
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _id);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _counter = 0;
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _id);
                WorkSession.WriteStatus(exception2.Message, _id);
                _counter = 0;
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(CharacterInfo), _argument);
        }

        public bool IsUsableName(string _name, string _account)
        {
            SqlCommand command;
            bool flag;
            WorkSession.WriteStatus("CharacterSqlAdapter.IsUsableName() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            if (_account.Length > 0)
            {
                command = new SqlCommand("dbo.CheckGameReservedName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter parameter = command.Parameters.Add("@account", SqlDbType.VarChar, 0x40);
                SqlParameter parameter2 = command.Parameters.Add("@characterName", SqlDbType.VarChar, 0x40);
                parameter.Value = _account;
                parameter2.Value = _name;
            }
            else
            {
                command = new SqlCommand("dbo.CheckUsableName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@name", SqlDbType.VarChar, 0x40).Value = _name;
            }
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.IsUsableName() : 데이터베이스와 연결합니다");
                connection.Open();
                flag = ((int) command.ExecuteScalar()) != 0;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _name);
                WorkSession.WriteStatus(exception.Message, _name);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _name);
                WorkSession.WriteStatus(exception2.Message, _name);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.IsUsableName() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool IsValidCache(CharacterInfo _cache)
        {
            bool flag;
            WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if (_cache == null)
            {
                return false;
            }
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlCommand command = new SqlCommand("dbo.GetCharacterUpdateTime", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idCharacter", SqlDbType.BigInt).Value = _cache.id;
                try
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                    connection.Open();
                    WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                    object obj2 = command.ExecuteScalar();
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        DateTime time = (DateTime) obj2;
                        if (time.Ticks <= _cache.updatetime.Ticks)
                        {
                            WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                            return true;
                        }
                        WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
                        return false;
                    }
                    flag = false;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _cache);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _cache);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public CharacterInfo Read(long _id, CharacterInfo _cache)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    return this._Read(_id);
                }
                if (!this.IsValidCache(_cache))
                {
                    return this._Read(_id);
                }
                return _cache;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _id);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                return null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _id);
                WorkSession.WriteStatus(exception2.Message);
                return null;
            }
        }

        public bool RemoveReservedCharName(string _name, string _account)
        {
            bool flag;
            WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("REMOVE_RESERVED_CHAR_NAME");
                SqlCommand command = new SqlCommand("dbo.UpdateGameReservedNameUsed", connection, transaction);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter parameter = command.Parameters.Add("@account", SqlDbType.VarChar, 0x40);
                SqlParameter parameter2 = command.Parameters.Add("@characterName", SqlDbType.VarChar, 0x40);
                parameter.Value = _account;
                parameter2.Value = _name;
                WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 명령을 실행합니다.");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 트랜잭션을 커밋합니다.");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("REMOVE_RESERVED_CHAR_NAME");
                }
                ExceptionMonitor.ExceptionRaised(exception, _name);
                WorkSession.WriteStatus(exception.Message, _name);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("REMOVE_RESERVED_CHAR_NAME");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _name);
                WorkSession.WriteStatus(exception2.Message, _name);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public DateTime Update(long _id, string _sql, SqlConnection _con, SqlTransaction _tran)
        {
            SqlCommand command = new SqlCommand(_sql, _con);
            command.Transaction = _tran;
            WorkSession.WriteStatus("CharacterSqlAdapter.Update() : 명령을 실행합니다.");
            command.ExecuteNonQuery();
            WorkSession.WriteStatus("CharacterSqlAdapter.Update() : 저장된 시간을 읽어서 리턴합니다.");
            SqlCommand command2 = new SqlCommand("dbo.GetCharacterUpdateTime", _con);
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.Parameters.Add("@idCharacter", SqlDbType.BigInt, 8).Value = _id;
            command2.Transaction = _tran;
            object obj2 = command2.ExecuteScalar();
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                throw new Exception(" 최근 업데이트 시간을 읽는데 실패했습니다.");
            }
            return (DateTime) obj2;
        }

        public bool Write(CharacterInfo _character, CharacterInfo _cache)
        {
            return this._Write(_character, _cache, false);
        }
    }
}

