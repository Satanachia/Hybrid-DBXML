namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class BankSqlAdapter : SqlAdapter, BankAdapter
    {
        private static string[] wealthColum = new string[] { "humanWealth", "elfWealth", "giantWealth" };

        private Bank _Read(string _account, string _charName, bool bRaceSpecified, BankRace _race)
        {
            Bank bank2;
            WorkSession.WriteStatus("BankSqlAdapter._Read() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlCommand selectCommand = new SqlCommand();
            selectCommand.Connection = connection;
            selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
            selectCommand.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20).Value = _account;
            selectCommand.Parameters.Add("@strCharacter", SqlDbType.NVarChar, 50).Value = _charName;
            if (bRaceSpecified)
            {
                if (_race != BankRace.None)
                {
                    selectCommand.CommandText = "dbo.SelectBank_Race";
                    selectCommand.Parameters.Add("@race", SqlDbType.TinyInt).Value = (byte) _race;
                }
                else
                {
                    selectCommand.CommandText = "dbo.SelectBank_Only";
                }
            }
            else
            {
                selectCommand.CommandText = "dbo.SelectBank2";
            }
            try
            {
                WorkSession.WriteStatus("BankSqlAdapter._Read() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "bank");
                adapter.TableMappings.Add("Table1", "slot");
                adapter.TableMappings.Add("Table2", "itemLarge");
                adapter.TableMappings.Add("Table3", "itemSmall");
                adapter.TableMappings.Add("Table4", "itemHuge");
                adapter.TableMappings.Add("Table5", "itemQuest");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("BankSqlAdapter._Read() : DataSet 에 뱅크 정보를 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctBankRead, Stopwatch.GetElapsedMilliseconds(timestamp));
                WorkSession.WriteStatus("BankSqlAdapter._Read() : DataSet 으로부터 뱅크 정보를 생성합니다");
                Bank bank = BankObjectBuilder.Build(dataSet);
                if (!bRaceSpecified)
                {
                    SlotObjectBuilder.Build(bank, dataSet);
                    bank.SetBankLoadStateAll(true);
                }
                else if (bRaceSpecified && (_race != BankRace.None))
                {
                    SlotObjectBuilder.Build(bank, dataSet);
                    bank.SetBankLoadState(_race);
                }
                bank2 = bank;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                bank2 = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _account);
                WorkSession.WriteStatus(exception2.Message);
                bank2 = null;
            }
            finally
            {
                WorkSession.WriteStatus("BankSqlAdapter._Read() : 연결을 종료합니다");
                connection.Close();
            }
            return bank2;
        }

        private bool _WriteEx(Bank _bank, CharacterInfo _character, BankCache _bankCache, CharacterInfo _charCache, CharacterAdapter _charAdapter, bool _forceUpdate)
        {
            WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 함수에 진입하였습니다");
            CharacterSqlAdapter adapter = (CharacterSqlAdapter) _charAdapter;
            try
            {
                WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 저장되어 있는 데이터를 읽어옵니다");
                if (_charCache == null)
                {
                    WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 저장되어 있는 캐릭터 데이터가 없어 새로 만듭니다.");
                    _charCache = adapter.Read(_character.id, null);
                }
                else if (!adapter.IsValidCache(_charCache))
                {
                    _charCache = adapter.Read(_character.id, null);
                }
                _bankCache = this.ValidateBankCache(_character.name, _bank, _bankCache);
                string str = string.Empty;
                string str2 = string.Empty;
                string str3 = string.Empty;
                string str4 = string.Empty;
                SlotUpdateBuilder.Build(_bank, _bankCache, out str, out str3);
                InventoryUpdateBuilder.Build(_character.id, _character.inventory, _charCache.inventory, _forceUpdate, out str2, out str4);
                string str5 = str + str2 + BankUpdateBuilder.Build(_bank, _bankCache) + str3;
                string str6 = CharacterUpdateBuilder.Build(_character, _charCache) + str4;
                if ((str5 != string.Empty) || (str6 != string.Empty))
                {
                    SqlConnection connection = new SqlConnection(base.ConnectionString);
                    SqlTransaction transaction = null;
                    try
                    {
                        WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();
                        connection.Open();
                        transaction = connection.BeginTransaction("BANK_UPDATE_EX_APP");
                        _bankCache.bank.updatetime = this.Update(_bank.account, str5, connection, transaction);
                        _character.updatetime = adapter.Update(_character.id, str6, connection, transaction);
                        transaction.Commit();
                        CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctBankWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                        return true;
                    }
                    catch (SqlException exception)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback("BANK_UPDATE_EX_APP");
                        }
                        if (!_forceUpdate && ItemSqlBuilder.ForceUpdateRetry(exception))
                        {
                            WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 아이템 오류로 재시도합니다.");
                            ExceptionMonitor.ExceptionRaised(exception, str5, str6);
                            WorkSession.WriteStatus(exception.Message, exception.Number);
                            connection.Close();
                            _bankCache.Invalidate();
                            return this._WriteEx(_bank, _character, _bankCache, null, _charAdapter, true);
                        }
                        ExceptionMonitor.ExceptionRaised(exception, str5, str6);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 수행할 명령이 없습니다.");
                return true;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _bank.account);
                WorkSession.WriteStatus(exception2.Message);
                return false;
            }
        }

        public static string GetWealthColumn(BankRace _race)
        {
            return wealthColum[(int) _race];
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Bank), _argument);
        }

        public bool IsValidCache(BankCache _cache)
        {
            bool flag;
            WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if ((_cache == null) || !_cache.IsValid())
            {
                return false;
            }
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlCommand command = new SqlCommand("dbo.GetBankUpdateTime", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _cache.Account;
                try
                {
                    WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                    connection.Open();
                    WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                    object obj2 = command.ExecuteScalar();
                    if ((obj2 != null) && (obj2 != DBNull.Value))
                    {
                        DateTime time = (DateTime) obj2;
                        if (time.Ticks <= _cache.bank.updatetime.Ticks)
                        {
                            WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                            return true;
                        }
                        WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
                        return false;
                    }
                    flag = false;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _cache.Account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _cache.Account);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public Bank Read(string _account, string _charName, BankRace _race, BankCache _cache)
        {
            WorkSession.WriteStatus("BankSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    return this._Read(_account, _charName, true, _race);
                }
                Bank bank = null;
                if (!this.IsValidCache(_cache))
                {
                    _cache.Invalidate();
                }
                if (_cache.IsRaceLoaded(_race))
                {
                    bank = _cache.ToBank(_race);
                }
                else
                {
                    bank = this._Read(_account, _charName, true, _race);
                    _cache.Update(bank);
                }
                return bank;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message);
                return null;
            }
        }

        private DateTime Update(string _account, string _sql)
        {
            DateTime minValue;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("BankSqlAdapter.Update() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                connection.Open();
                transaction = connection.BeginTransaction("BANK_UPDATE_APP");
                DateTime time = this.Update(_account, _sql, connection, transaction);
                transaction.Commit();
                CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctBankWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                minValue = time;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback("BANK_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _account, _sql);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                minValue = DateTime.MinValue;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    transaction.Rollback("BANK_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _account, _sql);
                WorkSession.WriteStatus(exception2.Message);
                minValue = DateTime.MinValue;
            }
            finally
            {
                WorkSession.WriteStatus("BankSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return minValue;
        }

        private DateTime Update(string _account, string _sql, SqlConnection _con, SqlTransaction _tran)
        {
            SqlCommand command = new SqlCommand(_sql, _con);
            command.Transaction = _tran;
            WorkSession.WriteStatus("BankSqlAdapter.Update() : 명령을 실행합니다.");
            command.ExecuteNonQuery();
            WorkSession.WriteStatus("BankSqlAdapter.Update() : 은행이 저장된 시간을 읽어서 리턴합니다.");
            SqlCommand command2 = new SqlCommand("dbo.GetBankUpdateTime", _con);
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
            command2.Transaction = _tran;
            object obj2 = command2.ExecuteScalar();
            if ((obj2 == null) || (obj2 == DBNull.Value))
            {
                throw new Exception("은행 최근 업데이트 시간을 읽는데 실패했습니다.");
            }
            return (DateTime) obj2;
        }

        private BankCache ValidateBankCache(string _charName, Bank _bank, BankCache _bankCache)
        {
            if (_bankCache == null)
            {
                WorkSession.WriteStatus("BankSqlAdapter.ValidateBankCache() : 저장되어 있는 은행 데이터가 없어 새로 만듭니다.");
                return new BankCache(this._Read(_bank.account, _charName, false, BankRace.None));
            }
            bool flag = false;
            if (!this.IsValidCache(_bankCache))
            {
                _bankCache.Invalidate();
                flag = true;
            }
            BankRace none = BankRace.None;
            bool bRaceSpecified = true;
            for (int i = 0; i < 3; i++)
            {
                BankRace race2 = (BankRace) ((byte) i);
                if (_bank.IsBankLoaded(race2) && !_bankCache.IsRaceLoaded(race2))
                {
                    flag = true;
                    if (none != BankRace.None)
                    {
                        none = BankRace.None;
                        bRaceSpecified = false;
                        break;
                    }
                    none = race2;
                }
            }
            if (flag)
            {
                _bankCache.Update(this._Read(_bank.account, _charName, bRaceSpecified, none));
            }
            return _bankCache;
        }

        public bool Write(string _charName, Bank _data, BankCache _cache)
        {
            WorkSession.WriteStatus("BankSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("BankSqlAdapter.Write() : 저장되어 있는 데이터를 읽어옵니다");
                _cache = this.ValidateBankCache(_charName, _data, _cache);
                string str = BankUpdateBuilder.Build(_data, _cache) + SlotUpdateBuilder.Build(_data, _cache);
                if (str != string.Empty)
                {
                    DateTime time = this.Update(_data.account, str);
                    if (time != DateTime.MinValue)
                    {
                        _cache.bank.updatetime = time;
                        return true;
                    }
                    return false;
                }
                WorkSession.WriteStatus("BankSqlAdapter.Write() : 변경점이 없습니다. 쿼리를 생략합니다");
                return true;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _data.account);
                WorkSession.WriteStatus(exception.Message);
                return false;
            }
        }

        public bool WriteEx(Bank _bank, CharacterInfo _character, BankCache _bankCache, CharacterInfo _charCache, CharacterAdapter _charAdapter)
        {
            return this._WriteEx(_bank, _character, _bankCache, _charCache, _charAdapter, false);
        }
    }
}

