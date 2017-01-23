namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class PetFileAdapter : FileAdapter, PetAdapter
    {
        public bool CreateEx(string _account, string _server, PetInfo _pet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            if (ConfigManager.characterFileAdapterFix)
            {
                bool flag;
                // WorkSession.WriteStatus("PetSqlAdapter.Create() : 함수에 진입하였습니다");
                SqlConnection connection2 = new SqlConnection(((SqlAdapter)_accountref).ConnectionString);
                SqlTransaction transaction2 = null;
                try
                {
                    //  WorkSession.WriteStatus("PetSqlAdapter.Create() : 데이터베이스와 연결합니다");
                    connection2.Open();
                    SqlCommand command = new SqlCommand("dbo.CheckUsableName", connection2);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add("@name", SqlDbType.VarChar, 0x40).Value = _pet.name;
                    //   WorkSession.WriteStatus("PetSqlAdapter.CreateEx() : 펫 이름 중복 검사를 실행합니다");
                    if (((int)command.ExecuteScalar()) == 0)
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("펫 이름이 중복됩니다."), _account, _pet.name);
                        return false;
                    }
                    transaction2 = connection2.BeginTransaction("PET_CREATE_APP");
                    //   WorkSession.WriteStatus("PetSqlAdapter.Create() : 계정-펫 링크 생성 명령을 실행합니다");
                    SqlCommand command3 = new SqlCommand(string.Concat(new object[] { "exec dbo.AddAccountrefPet @strAccount=", UpdateUtility.BuildString(_account), ",@idPet=", _pet.id, ",@name=", UpdateUtility.BuildString(_pet.name), ",@server=", UpdateUtility.BuildString(_server), ",@remaintime=", _pet.summon.remaintime, ",@lasttime=", _pet.summon.lasttime, ",@expiretime=", _pet.summon.expiretime, "\n" }), connection2);
                    command3.Transaction = transaction2;
                    command3.ExecuteNonQuery();
                    // WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 커밋합니다");
                    transaction2.Commit();

                    if (!base.IsExistData(_pet.id))
                    {
                        base.WriteToDB(_pet, _pet.id);
                        return true;
                    }
                    flag = true;


                }
                catch (SqlException exception)
                {
                    if (transaction2 != null)
                    {
                        transaction2.Rollback("PET_CREATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception);
                    //    WorkSession.WriteStatus(exception.Message, exception.Number);
                    //    WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                    flag = false;
                }
                catch (Exception exception2)
                {

                    if (transaction2 != null)
                    {
                        transaction2.Rollback("PET_CREATE_APP");
                    }

                    //    WorkSession.WriteStatus(exception2.Message);
                    ExceptionMonitor.ExceptionRaised(exception2);
                    //    WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                    flag = false;
                }
                finally
                {
                    //  WorkSession.WriteStatus("PetSqlAdapter.Create() : 연결을 종료합니다");

                    if (connection2.State == ConnectionState.Open)
                    {
                        connection2.Close();
                    }

                }
                return flag;
            }





            else
                //We are in test mode - use original file adapter code. 
                if (!base.IsExistData(_pet.id))
                {
                    base.WriteToDB(_pet, _pet.id);
                    ((AccountrefFileAdapter)_accountref).AddPetSlot(_account, _pet.id, _pet.name, _server, _pet.summon.remaintime, _pet.summon.lasttime, _pet.summon.expiretime);
                    return true;
                }
            return false;
        }

        public bool Delete(string _name)
        {
            return base.DeleteDB(_name);
        }

        public bool DeleteEx(string _account, string _server, long _idpet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            ((AccountrefFileAdapter)_accountref).RemovePetSlot(_account, _idpet, _server);
            return true;
        }

        public bool DeleteItem(long _id, ItemList[] _list)
        {
            if (!base.IsExistData(_id))
            {
                return false;
            }
            PetInfo info = (PetInfo)base.ReadFromDB(_id);
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
                this.Write(info);
            }
            return true;
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            if (base.IsExistData(_id))
            {
                PetInfo info = (PetInfo)base.ReadFromDB(_id);
                _counter = (info.data != null) ? info.data.writeCounter : ((byte)0);
                return true;
            }
            _counter = 0;
            return false;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(PetInfo), ConfigManager.GetFileDBPath("pet"), ".xml");
        }

        public bool IsUsableName(string _name, string _account)
        {
            if (ConfigManager.characterFileAdapterFix)
            {
                SqlCommand command;
                bool flag;
                WorkSession.WriteStatus("CharacterFileAdapter.IsUsableName() : 함수에 진입하였습니다");
                //Can't seem to figure out a dynamic way from reading from config without using the base sql interface
                SqlConnection connection = new SqlConnection(ConfigManager.nameCheckConnection);
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

        public PetInfo Read(string _account, string _server, long _id, PetInfo _cache, AccountRefAdapter _accountref)
        {
               //Gotta use that pesky sql account ref adapter again. Should probably do more error checking but its w/e quick n dirty for temp fix. 
            if (ConfigManager.characterFileAdapterFix)
            {


              //  WorkSession.WriteStatus("PetSqlAdapter.Read() : 함수에 진입하였습니다");
                try
                {
                    if ((_account != null) && (_server != null))
                    {
                        SqlConnection connection = null;
                        SqlDataReader reader = null;
                      //  WorkSession.WriteStatus("PetSqlAdapter.Read() : 펫의 소환 시간을 확인합니다.");
                        connection = new SqlConnection(((SqlAdapter)_accountref).ConnectionString);
                        connection.Open();
                        SqlCommand selectCommand = new SqlCommand("dbo.GetPetSummonTime", connection);
                        selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        selectCommand.Parameters.Add("@strAccount", SqlDbType.NVarChar, 50).Value = _account;
                        selectCommand.Parameters.Add("@strServer", SqlDbType.NVarChar, 0x80).Value = _server;
                        selectCommand.Parameters.Add("@idPet", SqlDbType.BigInt, 8).Value = _id;
                        //create SQL reader

                        reader = selectCommand.ExecuteReader();
               
                      /*  try
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
                            info.summon.remaintime = (int)table.Rows[0]["remaintime"];
                            info.summon.lasttime = (long)table.Rows[0]["lasttime"];
                            info.summon.expiretime = (long)table.Rows[0]["expiretime"];
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
                       * 
                       */
                        //Get the info from the reader. 
                        PetInfo info = (PetInfo)base.ReadFromDB(_id);
                        reader.Read();
                        info.summon.remaintime = reader.GetInt32(0);
                        info.summon.lasttime = reader.GetInt64(1);
                        info.summon.expiretime = reader.GetInt32(2);
                        if (reader != null)
                        {
                            try
                            {
                                reader.Close();
                            }
                            catch
                            {
                            }
                        }
                        try
                        {
                            connection.Close();
                        }
                        catch
                        {
                        }
                        return info;
                    }
                    WorkSession.WriteStatus("PetSqlAdapter.Read() : 계정, 서버 정보가 없어 소환 시간을 가져 오지 않습니다.");
                    return null;
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
            //If we are running in normal test mode use original code    
            else
            {
                AccountrefPet pet = null;
                Accountref accountref = _accountref.Read(_account);
                if (accountref != null)
                {
                    foreach (AccountrefPet pet2 in accountref.pet)
                    {
                        if ((pet2.id == _id) && (pet2.server == _server))
                        {
                            pet = pet2;
                            break;
                        }
                    }
                    if (pet == null)
                    {
                        return null;
                    }
                    if (base.IsExistData(_id))
                    {
                        PetInfo info = (PetInfo)base.ReadFromDB(_id);
                        info.summon.remaintime = pet.remaintime;
                        info.summon.lasttime = pet.lasttime;
                        return info;
                    }
                }
                return null;
            }
        }

        public bool Write(PetInfo _data)
        {
            if (base.IsExistData(_data.id))
            {
                base.WriteToDB(_data, _data.id);
                return true;
            }
            return false;
        }

        public bool Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref)
        {
               // If we are using character file adapter fix use my mess of shit. 
            if (ConfigManager.characterFileAdapterFix)
            {
                AccountrefPet pet = null;
                Accountref accountref = _accountref.Read(_account);
                if (accountref == null)
                {
                    return false;
                }
                foreach (AccountrefPet pet2 in accountref.pet)
                {
                    if ((pet2.id == _pet.id) && (pet2.server == _server))
                    {
                        pet = pet2;
                        break;
                    }
                }
                if (pet == null)
                {
                    return false;
                }
                pet.remaintime = _pet.summon.remaintime;
                pet.lasttime = _pet.summon.lasttime;
                pet.groupID = _channelgroupid;

                bool flag;
                WorkSession.WriteStatus("PetSqlAdapter.Write() : 함수에 진입하였습니다");
                try
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : SQL 명령문을 생성합니다");
                    string str = PetUpdateBuilder.Build(_pet, _cache) + InventoryUpdateBuilder.Build(_pet.id, _pet.inventory, _cache.inventory, false);
                    SqlConnection connection2 = new SqlConnection(((SqlAdapter)_accountref).ConnectionString);
                    SqlTransaction transaction2 = null;
                    try
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : Accountref 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();
                        connection2.Open();
                        transaction2 = connection2.BeginTransaction("PET_UPDATE_APP");
                        SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec dbo.UpdatePetSummonTime  ", UpdateUtility.BuildString(_account), ",", UpdateUtility.BuildString(_server), ",", _pet.id, ",", _channelgroupid, ",", _pet.summon.remaintime, ",", _pet.summon.lasttime }), connection2);
                        command.Transaction = transaction2;
                        command.ExecuteNonQuery();
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : 트랜잭션을 커밋합니다");
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
                        if (transaction2 != null)
                        {
                            transaction2.Rollback("PET_UPDATE_APP");
                        }
                        flag = false;
                    }
                    catch (Exception exception2)
                    {
                        ExceptionMonitor.ExceptionRaised(exception2, _pet, str);
                        WorkSession.WriteStatus(exception2.Message);
                        if (transaction2 != null)
                        {
                            transaction2.Rollback("PET_UPDATE_APP");
                        }
                        flag = false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : 연결을 종료합니다");
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
                this.Write(_pet);
                return flag;
            }


            // original Test mode code
            else
            {
                AccountrefPet pet = null;
                Accountref accountref = _accountref.Read(_account);
                if (accountref == null)
                {
                    return false;
                }
                foreach (AccountrefPet pet2 in accountref.pet)
                {
                    if ((pet2.id == _pet.id) && (pet2.server == _server))
                    {
                        pet = pet2;
                        break;
                    }
                }
                if (pet == null)
                {
                    return false;
                }
                pet.remaintime = _pet.summon.remaintime;
                pet.lasttime = _pet.summon.lasttime;
                pet.groupID = _channelgroupid;
                ((AccountrefFileAdapter)_accountref).WriteToDB(accountref, _account);
                return this.Write(_pet);
            }
        }
    }
}

