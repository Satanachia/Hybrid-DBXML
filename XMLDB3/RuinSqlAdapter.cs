namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class RuinSqlAdapter : SqlAdapter, RuinAdapter
    {
        private RuinList Build(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("유적 테이블을 얻어오지 못햇습니다.");
            }
            if ((_table.Rows == null) || (_table.Rows.Count <= 0))
            {
                return new RuinList();
            }
            RuinList list = new RuinList();
            list.ruins = new Ruin[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                list.ruins[i] = new Ruin();
                list.ruins[i].ruinID = (int) _table.Rows[i]["ruinID"];
                list.ruins[i].state = (int) _table.Rows[i]["state"];
                list.ruins[i].position = (int) _table.Rows[i]["position"];
                list.ruins[i].lastTime = (int) _table.Rows[i]["lastTime"];
                list.ruins[i].exploCharID = (long) _table.Rows[i]["exploCharID"];
                list.ruins[i].exploCharName = (string) _table.Rows[i]["exploCharName"];
                if (_table.Rows[i].IsNull("exploTime"))
                {
                    list.ruins[i].exploTime = DateTime.MinValue;
                }
                else
                {
                    list.ruins[i].exploTime = (DateTime) _table.Rows[i]["exploTime"];
                }
            }
            return list;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(RuinList), _argument);
        }

        public RuinList Read(RuinType _type)
        {
            WorkSession.WriteStatus("RuinSqlAdapter.Read() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                string cmdText = string.Empty;
                switch (_type)
                {
                    case RuinType.rtRuin:
                        cmdText = "dbo.SelectRuin";
                        break;

                    case RuinType.rtRelic:
                        cmdText = "dbo.SelectRelic";
                        break;

                    default:
                        throw new Exception("잘못된 유적 타입입니다");
                }
                SqlCommand selectCommand = new SqlCommand(cmdText, connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                WorkSession.WriteStatus("RuinSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "ruin");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("RuinSqlAdapter.Read() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                return this.Build(dataSet.Tables["ruin"]);
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _type);
                WorkSession.WriteStatus(exception.Message, exception.Number);
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
            }
            finally
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return null;
        }

        public bool Write(Ruin _ruin, RuinType _type)
        {
            bool flag;
            WorkSession.WriteStatus("RuinSqlAdapter.Write() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 데이터베이스와 연결합니다");
                connection.Open();
                string str = string.Empty;
                string str2 = string.Empty;
                switch (_type)
                {
                    case RuinType.rtRuin:
                        str2 = "dbo.UpdateRuin";
                        break;

                    case RuinType.rtRelic:
                        str2 = "dbo.UpdateRelic";
                        break;

                    default:
                        throw new Exception("잘못된 유적 타입입니다");
                }
                object obj2 = str;
                SqlCommand command = new SqlCommand(string.Concat(new object[] { 
                    obj2, "exec ", str2, " @ruinID=", _ruin.ruinID, ",@state=", _ruin.state, ",@position=", _ruin.position, ",@lastTime=", _ruin.lastTime, ",@exploCharID=", _ruin.exploCharID, ",@exploCharName=", UpdateUtility.BuildString(_ruin.exploCharName), ",@exploTime=", 
                    (_ruin.exploTime == DateTime.MinValue) ? "NULL" : UpdateUtility.BuildDateTime(_ruin.exploTime), "\n"
                 }), connection);
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool Write(RuinList _ruinList, RuinType _type)
        {
            bool flag;
            WorkSession.WriteStatus("RuinSqlAdapter.Write() : 함수에 진입하였습니다");
            if ((_ruinList.ruins == null) || (_ruinList.ruins.Length == 0))
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 업데이트할 유적이 없습니다.");
                return true;
            }
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 데이터베이스와 연결합니다");
                connection.Open();
                string cmdText = string.Empty;
                string str2 = string.Empty;
                switch (_type)
                {
                    case RuinType.rtRuin:
                        str2 = "dbo.UpdateRuin";
                        break;

                    case RuinType.rtRelic:
                        str2 = "dbo.UpdateRelic";
                        break;

                    default:
                        throw new Exception("잘못된 유적 타입입니다");
                }
                foreach (Ruin ruin in _ruinList.ruins)
                {
                    object obj2 = cmdText;
                    cmdText = string.Concat(new object[] { 
                        obj2, "exec ", str2, " @ruinID=", ruin.ruinID, ",@state=", ruin.state, ",@position=", ruin.position, ",@lastTime=", ruin.lastTime, ",@exploCharID=", ruin.exploCharID, ",@exploCharName=", UpdateUtility.BuildString(ruin.exploCharName), ",@exploTime=", 
                        (ruin.exploTime == DateTime.MinValue) ? "NULL" : UpdateUtility.BuildDateTime(ruin.exploTime), "\n"
                     });
                }
                if (cmdText != string.Empty)
                {
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    WorkSession.WriteStatus("RuinSqlAdapter.Write() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                }
                else
                {
                    WorkSession.WriteStatus("RuinSqlAdapter.Write() : 업데이트할 유적이 없습니다.");
                }
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

