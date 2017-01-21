namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    public class ChronicleSqlAdapter : SqlAdapter, ChronicleAdapter
    {
        public bool Create(string _characterName, Chronicle _chronicle)
        {
            bool flag2;
            WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                DateTime nextTime;
                ChronicleCache chronicle = ObjectCache.Chronicle;
                bool flag = false;
                int chronicleCount = 0;
                if (chronicle.Exists(_chronicle.questID))
                {
                    flag = true;
                    chronicleCount = this.GetChronicleCount(_chronicle);
                }
                string cmdText = string.Empty;
                WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 명령을 생성합니다.");
                int num2 = 0;
                if (flag && (chronicleCount == 0))
                {
                    num2 = chronicle.GetNextCount(_chronicle.serverName, _chronicle.questID, out nextTime);
                }
                else
                {
                    nextTime = chronicle.GetNextTime();
                }
                object obj2 = cmdText;
                cmdText = string.Concat(new object[] { obj2, "exec dbo.CreateChronicle  @serverName=", UpdateUtility.BuildString(_chronicle.serverName), ",@charID=", _chronicle.charID, ",@questID=", _chronicle.questID, ",@createTime=", UpdateUtility.BuildDateTime(nextTime), ",@meta=", UpdateUtility.BuildString(_chronicle.meta), "\n" });
                if (flag)
                {
                    if (chronicleCount > 0)
                    {
                        obj2 = cmdText;
                        cmdText = string.Concat(new object[] { obj2, "exec UpdateChronicleEventCount @serverName=", UpdateUtility.BuildString(_chronicle.serverName), ",@charID=", _chronicle.charID, ",@questID=", _chronicle.questID, ",@updateTime=", UpdateUtility.BuildDateTime(nextTime), "\n" });
                    }
                    else
                    {
                        obj2 = cmdText;
                        obj2 = string.Concat(new object[] { obj2, "exec CreateChronicleEventRank @serverName=", UpdateUtility.BuildString(_chronicle.serverName), ",@charID=", _chronicle.charID, ",@charName=", UpdateUtility.BuildString(_characterName), ",@questID=", _chronicle.questID, ",@rank=", num2, ",@updateTime=", UpdateUtility.BuildDateTime(nextTime), "\n" });
                        cmdText = string.Concat(new object[] { obj2, "exec UpdateChronicleLatestRank @serverName=", UpdateUtility.BuildString(_chronicle.serverName), ",@charID=", _chronicle.charID, ",@charName=", UpdateUtility.BuildString(_characterName), ",@questID=", _chronicle.questID, ",@rankTime=", UpdateUtility.BuildDateTime(nextTime), ",@rank=", num2, "\n" });
                        if (num2 <= ConfigManager.MaxChronicleFirst)
                        {
                            obj2 = cmdText;
                            cmdText = string.Concat(new object[] { obj2, "exec CreateChronicleFirstRank @serverName=", UpdateUtility.BuildString(_chronicle.serverName), ",@charID=", _chronicle.charID, ",@charName=", UpdateUtility.BuildString(_characterName), ",@questID=", _chronicle.questID, ",@rankTime=", UpdateUtility.BuildDateTime(nextTime), "\n" });
                        }
                    }
                }
                WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 데이터 베이스에 연결합니다.");
                connection.Open();
                transaction = connection.BeginTransaction("CHRONICLE_CREATE_APP");
                SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                WorkSession.WriteStatus("ChronicleSqlAdapter.Write() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag2 = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _chronicle.serverName, _chronicle.charID, _chronicle.questID);
                WorkSession.WriteStatus(exception.Message);
                if (transaction != null)
                {
                    transaction.Rollback("CHRONICLE_CREATE_APP");
                }
                flag2 = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _chronicle.serverName, _chronicle.charID, _chronicle.questID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("CHRONICLE_CREATE_APP");
                }
                flag2 = false;
            }
            finally
            {
                WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 연결을 종료합니다");
                connection.Close();
            }
            return flag2;
        }

        private int GetChronicleCount(Chronicle _chronicle)
        {
            int num;
            WorkSession.WriteStatus("ChronicleSqlAdapter.IsChronicleExist() : 탐사연표 존재를 검사합니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("ChronicleSqlAdapter.IsChronicleExist() : DB에 연결합니다.");
                connection.Open();
                SqlCommand command = new SqlCommand("CheckChronicleEventCount", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@charID", SqlDbType.BigInt).Value = _chronicle.charID;
                command.Parameters.Add("@serverName", SqlDbType.NVarChar, 0x80).Value = _chronicle.serverName;
                command.Parameters.Add("@questID", SqlDbType.Int).Value = _chronicle.questID;
                num = (int) command.ExecuteScalar();
            }
            finally
            {
                connection.Close();
            }
            return num;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Chronicle), _argument);
        }

        private string UpdateChronicleCache(DataTable _table, ChronicleInfoList _list)
        {
            Hashtable hashtable = ChronicleCacheBuilder.Build(_table, _list.infos);
            ObjectCache.InitChronicleCache(_list.serverName, hashtable);
            string str = string.Empty;
            foreach (int num in hashtable.Keys)
            {
                object obj2 = str;
                str = string.Concat(new object[] { obj2, "exec InitChronicleRank @questID=", num, ",@serverName=", UpdateUtility.BuildString(_list.serverName), ",@maxRecentRank=", ConfigManager.MaxChronicleLatest, "\n" });
            }
            return str;
        }

        private string UpdateChronicleInfo(DataTable _table, ChronicleInfo[] _infos)
        {
            Hashtable hashtable = ChronicleInfoBuilder.Build(_table);
            string str = string.Empty;
            foreach (ChronicleInfo info in _infos)
            {
                object obj2;
                if (hashtable.ContainsKey(info.questID))
                {
                    ChronicleInfo info2 = (ChronicleInfo) hashtable[info.questID];
                    if (!info.ContentEquals(info2))
                    {
                        obj2 = str;
                        str = string.Concat(new object[] { 
                            obj2, "exec UpdateChronicleInfo @questID=", info.questID, ",@questName=", UpdateUtility.BuildString(info.questName), ",@keyword=", UpdateUtility.BuildString(info.keyword), ",@localtext=", UpdateUtility.BuildString(info.localtext), ",@sort=", UpdateUtility.BuildString(info.sort), ",@group=", UpdateUtility.BuildString(info.group), ",@source=", UpdateUtility.BuildString(info.source), ",@width=", 
                            info.width, ",@height=", info.height, "\n"
                         });
                    }
                }
                else
                {
                    obj2 = str;
                    str = string.Concat(new object[] { 
                        obj2, "exec UpdateChronicleInfo @questID=", info.questID, ",@questName=", UpdateUtility.BuildString(info.questName), ",@keyword=", UpdateUtility.BuildString(info.keyword), ",@localtext=", UpdateUtility.BuildString(info.localtext), ",@sort=", UpdateUtility.BuildString(info.sort), ",@group=", UpdateUtility.BuildString(info.group), ",@source=", UpdateUtility.BuildString(info.source), ",@width=", 
                        info.width, ",@height=", info.height, "\n"
                     });
                }
            }
            return str;
        }

        public bool UpdateChronicleInfoList(ChronicleInfoList _list)
        {
            WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 함수에 진입하였습니다");
            if (((_list != null) && (_list.infos != null)) && (_list.infos.Length > 0))
            {
                SqlConnection connection = null;
                SqlTransaction transaction = null;
                try
                {
                    connection = new SqlConnection(base.ConnectionString);
                    SqlCommand selectCommand = new SqlCommand("SelectChronicleInfoList", connection);
                    selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    selectCommand.Parameters.Add("@serverName", SqlDbType.NVarChar, 0x80).Value = _list.serverName;
                    WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 데이터베이스에 연결합니다");
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    DataSet dataSet = new DataSet();
                    WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 데이터베이스로부터 이미지를 읽어옵니다.");
                    adapter.TableMappings.Add("Table", "chronicle_AllEventCount");
                    adapter.TableMappings.Add("Table1", "chronicle_Info");
                    adapter.Fill(dataSet);
                    adapter.Dispose();
                    string cmdText = string.Empty + this.UpdateChronicleCache(dataSet.Tables["chronicle_AllEventCount"], _list) + this.UpdateChronicleInfo(dataSet.Tables["chronicle_Info"], _list.infos);
                    if (cmdText != string.Empty)
                    {
                        WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 명령을 실행합니다.");
                        transaction = connection.BeginTransaction("CHRONICLE_INFO_UPDATE_APP");
                        SqlCommand command2 = new SqlCommand(cmdText, connection);
                        command2.Transaction = transaction;
                        command2.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    else
                    {
                        WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 실행할 명령이 없습니다.");
                    }
                    return true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    if (transaction != null)
                    {
                        transaction.Rollback("CHRONICLE_INFO_UPDATE_APP");
                    }
                    ObjectCache.DeleteChronicleCache();
                    return false;
                }
                catch (Exception exception2)
                {
                    ExceptionMonitor.ExceptionRaised(exception2);
                    WorkSession.WriteStatus(exception2.Message);
                    if (transaction != null)
                    {
                        transaction.Rollback("CHRONICLE_INFO_UPDATE_APP");
                    }
                    ObjectCache.DeleteChronicleCache();
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 이미지가 없습니다.");
            return true;
        }
    }
}

