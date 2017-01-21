namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class CharacterFileAdapter : FileAdapter, CharacterAdapter
    {
       
        private bool Create(CharacterInfo _character)
        {
            if (!base.IsExistData(_character.id))
            {
                base.WriteToDB(_character, _character.id);
                return true;
            }
            return false;
        }

        //Start modifications of CharacterFile adapter
        public bool CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo _character, AccountRefAdapter _accountref, BankAdapter _bank, WebSynchAdapter _websynch)
        {
            //Check to see if we are using the fix. If we are account_character_ref is instead changed to use the SQL adapter code instead of the file adapter code.
            if (ConfigManager.characterFileAdapterFix)
            {
                bool flag;
               // WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 함수에 진입하였습니다");
               //Work sessions commented out for debugging on my end. Makes the code long to traverse. 
                SqlConnection connection2 = new SqlConnection(((SqlAdapter)_accountref).ConnectionString);
                SqlTransaction transaction2 = null;
                try
                {
                 //   WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 데이터베이스와 연결합니다");
                    connection2.Open();
                    //Run the duplicate name check stored procedure. If a chaarcter with the same name and server exist already in account_character_ref throw error
                    //This errror is ugly to the users. Be aware. 
                    SqlCommand command = new SqlCommand("dbo.CheckUsableName", connection2)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    command.Parameters.Add("@name", SqlDbType.VarChar, 0x40).Value = _character.name;
                  //  WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 캐릭터 이름 중복 검사를 실행합니다");
                    if (((int)command.ExecuteScalar()) == 0)
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("캐릭터 이름이 중복됩니다."), _account, _character.name);
                        return false;
                    }
                 
                    transaction2 = connection2.BeginTransaction("CHARACTER_CREATE_EX_APP");

                   // WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 뱅크 생성 명령을 실행합니다");
                    
                    object obj2 = string.Concat(new object[] { "exec dbo.AddAccountrefCharacter  @strAccount=", UpdateUtility.BuildString(_account), ",@idCharacter=", _character.id, ",@name=", UpdateUtility.BuildString(_character.name), ",@server=", UpdateUtility.BuildString(_server), ",@race=", _race, ",@supportCharacter=", _supportCharacter ? 1 : 0, "\n" });
                    new SqlCommand(string.Concat(new object[] { obj2, "exec dbo.WriteAccountSupportRewardState  @strAccount=", UpdateUtility.BuildString(_account), ",@supportRewardState=", _supportRewardState, "\n" }), connection2) { Transaction = transaction2 }.ExecuteNonQuery();
                   // WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 웹싱크 명령을 실행합니다");

                    transaction2.Commit();
                    //Might want to add error checking here....but yolo
                    if (this.Create(_character))
                    {
                        ((BankFileAdapter)_bank).AddSlot(_account, _character.name, (BankRace)_race);
                        return true;
                    }
                    flag = true;
                }
                catch (SqlException exception)
                {
                    if (transaction2 != null)
                    {
                        transaction2.Rollback("CHARACTER_CREATE_EX_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _account);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                    flag = false;
                }
                catch (Exception exception2)
                {
                    if (transaction2 != null)
                    {
                        transaction2.Rollback("CHARACTER_CREATE_EX_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _account);
                    WorkSession.WriteStatus(exception2.Message, _account);
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 연결을 종료합니다");
                    if (connection2.State == ConnectionState.Open)
                    {
                        connection2.Close();
                    }
                }
                return flag;

            }
            //Not using the fix? This is the original code using only file adapters. Preserved for test mode. 
            else
            
            {
                if (this.Create(_character))
                {
                    AccountrefFileAdapter adapter = _accountref as AccountrefFileAdapter;
                    adapter.AddCharacterSlot(_account, _supportRewardState, _character.id, _character.name, _server, _race, _supportCharacter);
                    adapter.SetSupportRewardState(_account, _supportRewardState);
                    ((BankFileAdapter)_bank).AddSlot(_account, _character.name, (BankRace)_race);
                    return true;
                }
                return false;
            }
        }

        public bool Delete(long _id)
        {
            return true;
        }

        public bool Delete(string _name)
        {
            return base.DeleteDB(_name);
        }

        public bool DeleteEx(string _account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string _server, long _idcharacter, AccountRefAdapter _accountref, BankAdapter _bank, GuildAdapter _guild, WebSynchAdapter _websynch)
        {
            if (this.Delete(_idcharacter))
            {
                ((BankFileAdapter) _bank).RemoveSlot(_account, _idcharacter);
                AccountrefFileAdapter adapter = _accountref as AccountrefFileAdapter;
                adapter.RemoveSlot(_account, _idcharacter, _server);
                adapter.SetSupportState(_account, _supportRace, _supportRewardState, _supportLastChangeTime);
                return true;
            }
            return false;
        }

        public bool DeleteItem(long _id, ItemList[] _list)
        {
            CharacterInfo info = this.Read(_id, null);
            if (info == null)
            {
                return false;
            }
            if ((info.inventory != null) && (info.inventory.Count > 0))
            {
                foreach (ItemList list in _list)
                {
                    info.inventory.Remove(list.itemID);
                }
                base.WriteToDB(info, _id);
            }
            return true;
        }

        public uint GetAccumLevel(string _account, string _name)
        {
            return 0;
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            if (base.IsExistData(_id))
            {
                CharacterInfo info = (CharacterInfo) base.ReadFromDB(_id);
                _counter = (info.data != null) ? info.data.writeCounter : ((byte) 0);
                return true;
            }
            _counter = 0;
            return false;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(CharacterInfo), ConfigManager.GetFileDBPath("character"), ".xml");
        }

        //Start name check mess
        public bool IsUsableName(string _name, string _account)
        {
            if (ConfigManager.characterFileAdapterFix)
            {
                SqlCommand command;
                bool flag;
                WorkSession.WriteStatus("CharacterFileAdapter.IsUsableName() : 함수에 진입하였습니다");
                //Can't seem to figure out a dynamic way from reading from config without using the base sql interface
                SqlConnection connection = new SqlConnection("data source=127.0.0.1;initial catalog=mabinogi;integrated security=False;User ID=mabi_admin;Password=eM6LhjFW6BnT!h0!vo2s");
                if (_account.Length > 0)
                {
                    command = new SqlCommand("dbo.CheckGameReservedName", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    SqlParameter parameter = command.Parameters.Add("@account", SqlDbType.VarChar, 0x40);
                    SqlParameter parameter2 = command.Parameters.Add("@characterName", SqlDbType.VarChar, 0x40);
                    parameter.Value = _account;
                    parameter2.Value = _name;
                }
                else
                {
                    command = new SqlCommand("dbo.CheckUsableName", connection)
                    {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    command.Parameters.Add("@name", SqlDbType.VarChar, 0x40).Value = _name;
                }
                try
                {
                    WorkSession.WriteStatus("CharacterFileAdapter.IsUsableName() : 데이터베이스와 연결합니다");
                    connection.Open();
                    flag = ((int)command.ExecuteScalar()) != 0;
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
                    WorkSession.WriteStatus("CharacterFileAdapter.IsUsableName() : 연결을 종료합니다");
                    connection.Close();
                }
                return flag;
            }
            else
            //orig test mode code
            {
                return true;
            }
        }


        public CharacterInfo Read(long _id, CharacterInfo _cache)
        {
            if (base.IsExistData(_id))
            {
                return (CharacterInfo) base.ReadFromDB(_id);
            }
            return null;
        }

        public bool RemoveReservedCharName(string _name, string _account)
        {
            return true;
        }

        public bool Write(CharacterInfo _character, CharacterInfo _cache)
        {
            if (base.IsExistData(_character.id))
            {
                base.WriteToDB(_character, _character.id);
                return true;
            }
            return false;
        }
    }
}

