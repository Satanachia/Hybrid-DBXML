namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class PetSqlAdapter : SqlAdapter, PetAdapter
    {
        private PetInfo _Read(long _id)
        {
            PetInfo info2;
            WorkSession.WriteStatus("PetSqlAdapter._Read() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlCommand selectCommand = new SqlCommand("dbo.SelectPet", connection);
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.Add("@idPet", SqlDbType.BigInt, 8).Value = _id;
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter._Read() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "pet");
                adapter.TableMappings.Add("Table1", "pet_skill");
                adapter.TableMappings.Add("Table2", "CharItemLarge");
                adapter.TableMappings.Add("Table3", "CharItemSmall");
                adapter.TableMappings.Add("Table4", "CharItemHuge");
                adapter.TableMappings.Add("Table5", "CharItemQuest");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("PetSqlAdapter._Read() : DataSet 에 펫 정보를 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctPetRead, Stopwatch.GetElapsedMilliseconds(timestamp));
                WorkSession.WriteStatus("PetSqlAdapter._Read() : DataSet 으로부터 펫 정보를 생성합니다");
                info2 = PetObjectBuilder.Build(dataSet);
            }
            finally
            {
                WorkSession.WriteStatus("PetSqlAdapter._Read() : 연결을 종료합니다");
                connection.Close();
            }
            return info2;
        }

        private bool _Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref, bool _forceLinkUpdate)
        {
            bool flag;
            WorkSession.WriteStatus("PetSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.Write() : 비교본으로 사용할 수 있는 유효한 저장 데이터를 읽어옵니다");
                if (_cache == null)
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                    _cache = this._Read(_pet.id);
                }
                else if (!this.IsValidCache(_cache))
                {
                    _cache = this._Read(_pet.id);
                }
                WorkSession.WriteStatus("PetSqlAdapter.Write() : SQL 명령문을 생성합니다");
                string str = PetUpdateBuilder.Build(_pet, _cache) + InventoryUpdateBuilder.Build(_pet.id, _pet.inventory, _cache.inventory, _forceLinkUpdate);
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                SqlConnection connection2 = new SqlConnection(((SqlAdapter) _accountref).ConnectionString);
                SqlTransaction transaction2 = null;
                try
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : Accountref 데이터베이스와 연결합니다");
                    long timestamp = Stopwatch.GetTimestamp();
                    if (str != string.Empty)
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : Pet 데이터베이스와 연결합니다");
                        connection.Open();
                        transaction = connection.BeginTransaction("PET_UPDATE_APP");
                        _pet.updatetime = this.Update(_pet.id, str, connection, transaction);
                    }
                    else
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : 변경점이 없습니다. 쿼리를 생략합니다");
                    }
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : 소환시간을 기록합니다.");
                    connection2.Open();
                    transaction2 = connection2.BeginTransaction("PET_UPDATE_APP");
                    SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec dbo.UpdatePetSummonTime  ", UpdateUtility.BuildString(_account), ",", UpdateUtility.BuildString(_server), ",", _pet.id, ",", _channelgroupid, ",", _pet.summon.remaintime, ",", _pet.summon.lasttime }), connection2);
                    command.Transaction = transaction2;
                    command.ExecuteNonQuery();
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : 트랜잭션을 커밋합니다");
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                    if (transaction2 != null)
                    {
                        transaction2.Commit();
                    }
                    CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctPetWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _pet, str);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    if (transaction != null)
                    {
                        transaction.Rollback("PET_UPDATE_APP");
                    }
                    if (transaction2 != null)
                    {
                        transaction2.Rollback("PET_UPDATE_APP");
                    }
                    if (!_forceLinkUpdate && ItemSqlBuilder.ForceUpdateRetry(exception))
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : 아이템 오류로 재시도합니다.");
                        connection.Close();
                        connection2.Close();
                        return this._Write(_account, _server, _channelgroupid, _pet, null, _accountref, true);
                    }
                    flag = false;
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2, _pet, str);
                    WorkSession.WriteStatus(exception2.Message);
                    if (transaction != null)
                    {
                        transaction.Rollback("PET_UPDATE_APP");
                    }
                    if (transaction2 != null)
                    {
                        transaction2.Rollback("PET_UPDATE_APP");
                    }
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : 연결을 종료합니다");
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    if (connection2.State == ConnectionState.Open)
                    {
                        connection2.Close();
                    }
                }
            }
            catch (SqlException exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3);
                WorkSession.WriteStatus(exception3.Message, _pet);
                flag = false;
            }
            catch (Exception exception4)
            {
                ExceptionMonitor.ExceptionRaised(exception4);
                WorkSession.WriteStatus(exception4.Message, _pet);
                flag = false;
            }
            return flag;
        }

        public bool CreateEx(string _account, string _server, PetInfo _pet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            bool flag;
            WorkSession.WriteStatus("PetSqlAdapter.Create() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _accountref).ConnectionString);
            SqlTransaction transaction2 = null;
            SqlConnection connection3 = new SqlConnection(((SqlAdapter) _websynch).ConnectionString);
            SqlTransaction transaction3 = null;
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand("dbo.CheckUsableName", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@name", SqlDbType.VarChar, 0x40).Value = _pet.name;
                WorkSession.WriteStatus("PetSqlAdapter.CreateEx() : 펫 이름 중복 검사를 실행합니다");
                if (((int) command.ExecuteScalar()) == 0)
                {
                    ExceptionMonitor.ExceptionRaised(new Exception("펫 이름이 중복됩니다."), _account, _pet.name);
                    return false;
                }
                transaction = connection.BeginTransaction("PET_CREATE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("PET_CREATE_APP");
                connection3.Open();
                transaction3 = connection3.BeginTransaction("PET_CREATE_APP");
                SqlCommand command2 = new SqlCommand(PetCreateBuilder.Build(_pet), connection);
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 펫 생성 명령을 실행합니다");
                command2.Transaction = transaction;
                command2.ExecuteNonQuery();
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 계정-펫 링크 생성 명령을 실행합니다");
                SqlCommand command3 = new SqlCommand(string.Concat(new object[] { "exec dbo.AddAccountrefPet @strAccount=", UpdateUtility.BuildString(_account), ",@idPet=", _pet.id, ",@name=", UpdateUtility.BuildString(_pet.name), ",@server=", UpdateUtility.BuildString(_server), ",@remaintime=", _pet.summon.remaintime, ",@lasttime=", _pet.summon.lasttime, ",@expiretime=", _pet.summon.expiretime, "\n" }), connection2);
                command3.Transaction = transaction2;
                command3.ExecuteNonQuery();
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 웹싱크 명령을 실행합니다");
                SqlCommand command4 = new SqlCommand(string.Concat(new object[] { "exec WebSynch_AddCharacter @strAccount = ", UpdateUtility.BuildString(_account), " ,@idCharacter = ", _pet.id, " ,@strName = ", UpdateUtility.BuildString(_pet.name), " ,@strServer = ", UpdateUtility.BuildString(_server) }), connection3);
                command4.Transaction = transaction3;
                command4.ExecuteNonQuery();
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                transaction2.Commit();
                transaction3.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback("PET_CREATE_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("PET_CREATE_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("PET_CREATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    transaction.Rollback("PET_CREATE_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("PET_CREATE_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("PET_CREATE_APP");
                }
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("PetSqlAdapter.Create() : 연결을 종료합니다");
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
            }
            return flag;
        }

        public bool DeleteEx(string _account, string _server, long _idPet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            bool flag;
            WorkSession.WriteStatus("PetSqlAdapter.Delete() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _accountref).ConnectionString);
            SqlTransaction transaction2 = null;
            SqlConnection connection3 = new SqlConnection(((SqlAdapter) _websynch).ConnectionString);
            SqlTransaction transaction3 = null;
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("PET_DELETE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("PET_DELETE_APP");
                connection3.Open();
                transaction3 = connection3.BeginTransaction("PET_DELETE_APP");
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 계정-펫 링크를 제거명령을 실행합니다");
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec dbo. DeleteAccountrefPet @strAccount=", UpdateUtility.BuildString(_account), ",@idPet=", _idPet, ",@server=", _server, "\n" }), connection2);
                command.Transaction = transaction2;
                if (command.ExecuteNonQuery() != 1)
                {
                    throw new Exception(string.Concat(new object[] { "account_pet_ref 테이블에서 [", _idPet, "/", _account, "/", _server, "] 열을 찾을 수 없습니다." }));
                }
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 캐릭터 제거 명령을 실행합니다");
                SqlCommand deleteProcedure = this.GetDeleteProcedure(_idPet, connection);
                deleteProcedure.Transaction = transaction;
                deleteProcedure.ExecuteNonQuery();
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 웹 싱크 명령을 실행합니다");
                SqlCommand command3 = new SqlCommand(string.Concat(new object[] { "exec dbo.WebSynch_RemoveCharacter @strAccount = ", UpdateUtility.BuildString(_account), " ,@idCharacter = ", _idPet, " ,@strServer = ", UpdateUtility.BuildString(_server) }), connection3);
                command3.Transaction = transaction3;
                command3.ExecuteNonQuery();
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                transaction2.Commit();
                transaction3.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback("PET_DELETE_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("PET_DELETE_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("PET_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                ExceptionMonitor.ExceptionRaised(new Exception(string.Concat(new object[] { "[", exception.Number, "]", exception.Message })));
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    transaction.Rollback("PET_DELETE_APP");
                }
                if (transaction2 != null)
                {
                    transaction2.Rollback("PET_DELETE_APP");
                }
                if (transaction3 != null)
                {
                    transaction3.Rollback("PET_DELETE_APP");
                }
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("PetSqlAdapter.Delete() : 연결을 종료합니다");
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
            }
            return flag;
        }

        public bool DeleteItem(long _id, ItemList[] _list)
        {
            WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
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
                        WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();
                        connection.Open();
                        transaction = connection.BeginTransaction("PET_ITEM_DELETE_APP");
                        SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                        command.CommandType = System.Data.CommandType.Text;
                        WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 명령을 수행합니다.");
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
                            transaction.Rollback("PET_ITEM_DELETE_APP");
                        }
                        return false;
                    }
                    catch (Exception exception2)
                    {
                        ExceptionMonitor.ExceptionRaised(exception2, _list);
                        WorkSession.WriteStatus(exception2.Message);
                        if (transaction != null)
                        {
                            transaction.Rollback("PET_ITEM_DELETE_APP");
                        }
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 삭제할 아이템이 없습니다.");
                return true;
            }
            catch (Exception exception3)
            {
                WorkSession.WriteStatus(exception3.Message);
                ExceptionMonitor.ExceptionRaised(exception3);
                return false;
            }
        }

        protected override SqlCommand GetCreateProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("dbo.CreatePet", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlPet", SqlDbType.NText, 0xf4240).Value = _argument;
            return command;
        }

        protected override SqlCommand GetDeleteProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("dbo.DeletePet", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idPet", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            bool flag;
            WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlCommand command = new SqlCommand("GetPetWriteCounter", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@id", SqlDbType.BigInt, 8).Value = _id;
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 데이터베이스와 연결합니다");
                connection.Open();
                _counter = (byte) command.ExecuteScalar();
                WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 저장 카운터를 읽었습니다.");
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
                WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(PetInfo), _argument);
        }

        public bool IsValidCache(PetInfo _cache)
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
                SqlCommand command = new SqlCommand("dbo.GetPetUpdateTime", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idPet", SqlDbType.BigInt).Value = _cache.id;
                try
                {
                    WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                    connection.Open();
                    WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                    object obj2 = command.ExecuteScalar();
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        DateTime time = (DateTime) obj2;
                        if (time.Ticks <= _cache.updatetime.Ticks)
                        {
                            WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                            return true;
                        }
                        WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
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
                    WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
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

        public PetInfo Read(string _account, string _server, long _id, PetInfo _cache, AccountRefAdapter _accountref)
        {
            WorkSession.WriteStatus("PetSqlAdapter.Read() : 함수에 진입하였습니다");
            PetInfo info = null;
            try
            {
                if (_cache == null)
                {
                    info = this._Read(_id);
                }
                else if (!this.IsValidCache(_cache))
                {
                    info = this._Read(_id);
                }
                else
                {
                    info = _cache;
                }
                if ((_account != null) && (_server != null))
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Read() : 펫의 소환 시간을 확인합니다.");
                    SqlConnection connection = new SqlConnection(((SqlAdapter) _accountref).ConnectionString);
                    SqlCommand selectCommand = new SqlCommand("dbo.GetPetSummonTime", connection);
                    selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    selectCommand.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                    selectCommand.Parameters.Add("@strServer", SqlDbType.NVarChar, 0x80).Value = _server;
                    selectCommand.Parameters.Add("@idPet", SqlDbType.BigInt, 8).Value = _id;
                    try
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Read() : 데이터베이스와 연결합니다");
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                        adapter.TableMappings.Add("Table", "pet");
                        WorkSession.WriteStatus("PetSqlAdapter.Read() : DataSet 에 펫 정보를 채웁니다");
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        adapter.Dispose();
                        DataTable table = dataSet.Tables["pet"];
                        if (table.Rows.Count != 1)
                        {
                            throw new Exception("PetSqlAdapter.Read() : [" + _id.ToString() + "/" + _account + "/" + _server + "] 소환 정보를 가져오지 못하였습니다.");
                        }
                        info.summon.remaintime = (int) table.Rows[0]["remaintime"];
                        info.summon.lasttime = (long) table.Rows[0]["lasttime"];
                        info.summon.expiretime = (long) table.Rows[0]["expiretime"];
                        return info;
                    }
                    catch (SqlException exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception, _id);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        return null;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Read() : 데이터베이스에 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("PetSqlAdapter.Read() : 계정, 서버 정보가 없어 소환 시간을 가져 오지 않습니다.");
                return info;
            }
            catch (SqlException exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _id);
                WorkSession.WriteStatus(exception2.Message, exception2.Number);
                return null;
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _id);
                WorkSession.WriteStatus(exception3.Message);
                return null;
            }
        }

        private DateTime Update(long _id, string _sql)
        {
            DateTime minValue;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.Update() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("PET_UPDATE_APP");
                DateTime time = this.Update(_id, _sql, connection, transaction);
                transaction.Commit();
                minValue = time;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback("PET_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _id, _sql);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                minValue = DateTime.MinValue;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    transaction.Rollback("PET_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _id, _sql);
                WorkSession.WriteStatus(exception2.Message);
                minValue = DateTime.MinValue;
            }
            finally
            {
                WorkSession.WriteStatus("PetSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return minValue;
        }

        private DateTime Update(long _id, string _sql, SqlConnection _con, SqlTransaction _tran)
        {
            SqlCommand command = new SqlCommand(_sql, _con);
            command.Transaction = _tran;
            WorkSession.WriteStatus("PetSqlAdapter.Update() : 명령을 실행합니다.");
            command.ExecuteNonQuery();
            WorkSession.WriteStatus("PetSqlAdapter.Update() : 저장된 시간을 읽어서 리턴합니다.");
            SqlCommand command2 = new SqlCommand("dbo.GetPetUpdateTime", _con);
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.Parameters.Add("@idPet", SqlDbType.BigInt, 8).Value = _id;
            command2.Transaction = _tran;
            object obj2 = command2.ExecuteScalar();
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                throw new Exception(" 최근 업데이트 시간을 읽는데 실패했습니다.");
            }
            return (DateTime) obj2;
        }

        public bool Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref)
        {
            return this._Write(_account, _server, _channelgroupid, _pet, _cache, _accountref, false);
        }
    }
}

