namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class WorldMetaSqlAdapter : SqlAdapter, WorldMetaAdapter
    {
        private WorldMetaList Build(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("월드메타 테이블을 얻어오지 못햇습니다.");
            }
            if ((_table.Rows == null) || (_table.Rows.Count <= 0))
            {
                return new WorldMetaList();
            }
            WorldMetaList list = new WorldMetaList();
            list.metas = new WorldMeta[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                list.metas[i] = new WorldMeta();
                list.metas[i].key = (string) _table.Rows[i]["key"];
                list.metas[i].type = (byte) _table.Rows[i]["type"];
                list.metas[i].value = (string) _table.Rows[i]["value"];
            }
            return list;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(WorldMeta), _argument);
        }

        public WorldMetaList Read()
        {
            WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand selectCommand = new SqlCommand("dbo.WorldMetaSelect", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "WorldMeta");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                return this.Build(dataSet.Tables["WorldMeta"]);
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
                WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return null;
        }

        private bool Remove(StringBuilder _sb, string _key)
        {
            if (_sb == null)
            {
                return false;
            }
            _sb.AppendFormat("exec dbo.worldmetaRemove @key='{0}'\n", _key);
            return true;
        }

        private bool Update(StringBuilder _sb, WorldMeta _worldmeta)
        {
            if (_sb == null)
            {
                return false;
            }
            _sb.AppendFormat("exec dbo.worldmetaUpdate @key='{0}',@type={1},@value='{2}'\n", _worldmeta.key, _worldmeta.type.ToString(), _worldmeta.value);
            return true;
        }

        public REPLY_RESULT UpdateList(WorldMetaList _worldmetaUpdateList, string[] _removeKeys, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                StringBuilder builder = new StringBuilder();
                if ((_worldmetaUpdateList != null) && (_worldmetaUpdateList.metas != null))
                {
                    foreach (WorldMeta meta in _worldmetaUpdateList.metas)
                    {
                        if (!this.Update(builder, meta))
                        {
                            if (transaction != null)
                            {
                                transaction.Rollback("WORLDMETA_UPDATELIST_APP");
                            }
                            return REPLY_RESULT.FAIL;
                        }
                    }
                }
                if (_removeKeys != null)
                {
                    foreach (string str in _removeKeys)
                    {
                        if (!this.Remove(builder, str))
                        {
                            if (transaction != null)
                            {
                                transaction.Rollback("WORLDMETA_UPDATELIST_APP");
                            }
                            return REPLY_RESULT.FAIL;
                        }
                    }
                }
                string cmdText = builder.ToString();
                if (cmdText.Length > 0)
                {
                    WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("WORLDMETA_UPDATELIST_APP");
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 명령을 실행합니다");
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("WORLDMETA_UPDATELIST_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("WORLDMETA_UPDATELIST_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }
    }
}

