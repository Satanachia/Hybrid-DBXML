namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class GuildSqlAdapter : SqlAdapter, GuildAdapter
    {
        private static XmlSerializer guildIDListSerializer = new XmlSerializer(typeof(GuildIDList));

        private GuildIDList _loadGuildList(string _server)
        {
            GuildIDList list;
            WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand("SelectGuildList3", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@server", SqlDbType.NVarChar, 0x80).Value = _server;
                WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 명령을 실행합니다");
                XmlReader xmlReader = command.ExecuteXmlReader();
                if (xmlReader == null)
                {
                    throw new Exception("쿼리된 데이터가 없습니다");
                }
                try
                {
                    WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 쿼리 결과를 길드리스트로 변환합니다");
                    if (guildIDListSerializer.CanDeserialize(xmlReader))
                    {
                        return (GuildIDList) guildIDListSerializer.Deserialize(xmlReader);
                    }
                    WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 쿼리 결과를 길드리스트로 변환할 수 없습니다");
                    list = null;
                }
                finally
                {
                    xmlReader.Close();
                }
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                list = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                list = null;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 연결을 종료합니다");
                connection.Close();
            }
            return list;
        }

        public bool AddMember(long _id, GuildMember _member, string _joinmsg)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("GUILD_ADDMEMBER_APP");
                SqlCommand command = new SqlCommand("AddGuildMember", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _id;
                command.Parameters.Add("@memberid", SqlDbType.BigInt, 8).Value = _member.memberid;
                command.Parameters.Add("@name", SqlDbType.NVarChar, 0x40).Value = _member.name;
                command.Parameters.Add("@account", SqlDbType.NVarChar, 0x20).Value = _member.account;
                command.Parameters.Add("@class", SqlDbType.Int, 4).Value = _member.@class;
                command.Parameters.Add("@point", SqlDbType.Int, 4).Value = _member.point;
                command.Parameters.Add("@joinmsg", SqlDbType.NVarChar, 100).Value = _joinmsg;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_ADDMEMBER_APP");
                }
                if (exception.Number == 0xc350)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 멤버수 초과로 인해서 실패하였습니다");
                    return false;
                }
                ExceptionMonitor.ExceptionRaised(exception, _id, _member);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_ADDMEMBER_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _id, _member);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool AddMoney(long _id, int _iAddedMoney)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_ADDMONEY_APP");
            try
            {
                SqlCommand command = new SqlCommand("AddGuildMoney", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _id;
                command.Parameters.Add("@intMoney", SqlDbType.BigInt, 4).Value = _iAddedMoney;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_ADDMONEY_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _id, _iAddedMoney);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_ADDMONEY_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _id, _iAddedMoney);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool AddPoint(long _id, int _iAddedPoint)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_ADDPOINT_APP");
            try
            {
                SqlCommand command = new SqlCommand("AddGuildPoint", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _id;
                command.Parameters.Add("@iPoint", SqlDbType.BigInt, 4).Value = _iAddedPoint;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_ADDPOINT_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _id, _iAddedPoint);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_ADDPOINT_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _id, _iAddedPoint);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public int ChangeGuildStone(long _idGuild, int _iType, int _iGold, int _iGP)
        {
            int num2;
            WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_CHANGE_GUILDSTONE");
            try
            {
                SqlCommand command = new SqlCommand("ChangeGuildStone", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@intType", SqlDbType.Int, 4).Value = _iType;
                command.Parameters.Add("@intMoney", SqlDbType.Int, 4).Value = _iGold;
                command.Parameters.Add("@intGP", SqlDbType.Int, 4).Value = _iGP;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback("GUILD_CHANGE_GUILDSTONE");
                }
                num2 = num;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_CHANGE_GUILDSTONE");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                num2 = -1;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_CHANGE_GUILDSTONE");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                num2 = -1;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 연결을 종료합니다");
                connection.Close();
            }
            return num2;
        }

        public bool CheckMemberJointime(long _idMember, string _server)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 데이터베이스와 연결합니다");
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand("CheckGuildMemberJointime", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@memberid", SqlDbType.BigInt, 8).Value = _idMember;
                command.Parameters.Add("@server", SqlDbType.NVarChar, 0x20).Value = _server;
                WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 가입 가능합니다.");
                flag = true;
            }
            catch (SqlException exception)
            {
                if (exception.Number >= 0xc350)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 가입 불가능합니다.");
                    return false;
                }
                ExceptionMonitor.ExceptionRaised(exception, _idMember, _server);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _idMember, _server);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool ClearBattleGroundType(string _server, ArrayList _GuildList)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 함수에 진입하였습니다");
            if (_GuildList.Count <= 0)
            {
                WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 업데이트 내역이 없습니다.");
                return true;
            }
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_CLEAR_BGTYPE_APP");
            try
            {
                StringBuilder builder = new StringBuilder("update guild set battlegroundtype = 0 where id in (");
                for (int i = 0; i < _GuildList.Count; i++)
                {
                    if (i == 0)
                    {
                        builder.Append(_GuildList[i].ToString());
                    }
                    else
                    {
                        builder.AppendFormat(", {0}", _GuildList[i].ToString());
                    }
                }
                builder.Append(")");
                SqlCommand command = new SqlCommand(builder.ToString(), connection, transaction);
                command.CommandType = System.Data.CommandType.Text;
                WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 명령을 실행합니다.");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_CLEAR_BGTYPE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_CLEAR_BGTYPE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool Create(Guild _data)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.Create() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter.Write() : SQL 명령문을 생성합니다");
                string cmdText = GuildCreateBuilder.Build(_data);
                if (!(cmdText != string.Empty))
                {
                    throw new Exception("길드 생성 쿼리를 만드는데 실패하였습니다.");
                }
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.Create() : 데이터 베이스에 연결합니다.");
                    connection.Open();
                    transaction = connection.BeginTransaction("GUILD_CREATE_APP");
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("GuildSqlAdapter.Write() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    WorkSession.WriteStatus("GuildSqlAdapter.Write() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                }
                catch (SqlException exception)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                        transaction.Rollback("GUILD_CREATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _data);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    return false;
                }
                catch (Exception exception2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                        transaction.Rollback("GUILD_CREATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _data);
                    WorkSession.WriteStatus(exception2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.Create() : 연결을 종료합니다");
                    connection.Close();
                }
                return true;
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _data);
                WorkSession.WriteStatus(exception3.Message);
                return false;
            }
        }

        public bool Delete(long _id)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("GUILD_DELETE_APP");
                SqlCommand command = new SqlCommand("DeleteGuild", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _id;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _id);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _id);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool DeleteGuildRobe(long _idGuild)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_DELETE_GUILDROBE_APP");
            try
            {
                SqlCommand command = new SqlCommand("dbo.DeleteGuildRobe", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 명령을 실행합니다.");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 트랜잭션을 커밋합니다.");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_GUILDROBE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_GUILDROBE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool DeleteGuildStone(long _idGuild)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_DELETE_GUILDSTONE_APP");
            try
            {
                SqlCommand command = new SqlCommand("DeleteGuildStone", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_DELETE_GUILDSTONE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_DELETE_GUILDSTONE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public DateTime GetDBCurrentTime()
        {
            DateTime minValue;
            WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 함수에 진입하였습니다");
            SqlConnection selectConnection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 데이터베이스와 연결합니다");
            selectConnection.Open();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select getdate() as [time]", selectConnection);
                adapter.TableMappings.Add("Table", "CurrentTime");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : DataSet 을 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 데이터베이스의 현재 시간을 DataSet 에서 읽습니다");
                DateTime time = (DateTime) dataSet.Tables["CurrentTime"].Rows[0]["time"];
                dataSet.Dispose();
                minValue = time;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                minValue = DateTime.MinValue;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                minValue = DateTime.MinValue;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 연결을 종료합니다");
                selectConnection.Close();
            }
            return minValue;
        }

        protected override SqlCommand GetDeleteProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("DeleteGuild", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        protected override SqlCommand GetInsertProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("InsertGuild", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlGuild", SqlDbType.NText, 0xf4240).Value = _argument;
            return command;
        }

        public bool GetJoinedMemberCount(long _idGuild, DateTime _startTime, DateTime _endTime, out int _count)
        {
            bool flag;
            WorkSession.WriteStatus("GetJoinedMemberCount.UpdateTitle() : 함수에 진입하였습니다");
            _count = 0;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GetJoinedMemberCount.UpdateTitle() : 데이터베이스와 연결합니다");
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand("dbo.GetGuildJoinedMemberByPeriod", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@startTime", SqlDbType.DateTime).Value = _startTime;
                command.Parameters.Add("@endTime", SqlDbType.DateTime).Value = _endTime;
                WorkSession.WriteStatus("GetJoinedMemberCount.UpdateTitle() : 명령을 실행합니다.");
                _count = (int) command.ExecuteScalar();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GetJoinedMemberCount.UpdateTitle() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("SelectGuild4", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Guild), _argument);
        }

        public bool IsUsableName(string _name)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : 함수에 진입하였습니다");
            SqlConnection selectConnection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : 데이터베이스와 연결합니다");
            selectConnection.Open();
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select id from [guild] where [guild].[name] = '" + _name.Replace("'", "''") + "'", selectConnection);
                adapter.TableMappings.Add("Table", "GuildName");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : DataSet 을 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                int count = dataSet.Tables["GuildName"].Rows.Count;
                dataSet.Dispose();
                flag = count == 0;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _name);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _name);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : 연결을 종료합니다");
                selectConnection.Close();
            }
            return flag;
        }

        public GuildIDList LoadDeletedGuildList(string _server, DateTime _overTime)
        {
            GuildIDList list;
            WorkSession.WriteStatus("GuildSqlAdapter.LoadDeletedGuildList() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SelectDeletedGuildListDiffer3", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@server", SqlDbType.NVarChar, 0x80).Value = _server;
                command.Parameters.Add("@timeline", SqlDbType.DateTime, 0).Value = _overTime;
                XmlReader xmlReader = command.ExecuteXmlReader();
                if (xmlReader == null)
                {
                    throw new Exception("쿼리된 데이터가 없습니다");
                }
                try
                {
                    if (guildIDListSerializer.CanDeserialize(xmlReader))
                    {
                        return (GuildIDList) guildIDListSerializer.Deserialize(xmlReader);
                    }
                    list = null;
                }
                finally
                {
                    xmlReader.Close();
                }
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                list = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                list = null;
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

        public GuildIDList LoadGuildList(string _server, DateTime _overTime)
        {
            GuildIDList list;
            WorkSession.WriteStatus("GuildSqlAdapter.LoadGuildList() : 함수에 진입하였습니다");
            try
            {
                if (_overTime == DateTime.MinValue)
                {
                    list = this._loadGuildList(_server);
                }
                else
                {
                    SqlConnection connection = new SqlConnection(base.ConnectionString);
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SelectGuildListDiffer3", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("@server", SqlDbType.NVarChar, 0x80).Value = _server;
                        command.Parameters.Add("@timeline", SqlDbType.DateTime, 0).Value = _overTime;
                        XmlReader xmlReader = command.ExecuteXmlReader();
                        if (xmlReader == null)
                        {
                            throw new Exception("쿼리된 데이터가 없습니다");
                        }
                        try
                        {
                            if (guildIDListSerializer.CanDeserialize(xmlReader))
                            {
                                return (GuildIDList) guildIDListSerializer.Deserialize(xmlReader);
                            }
                            list = null;
                        }
                        finally
                        {
                            xmlReader.Close();
                        }
                    }
                    catch (SqlException exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        list = null;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                list = null;
            }
            return list;
        }

        public Guild Read(long _id)
        {
            return (Guild) base.Read(_id);
        }

        public bool SetGuildStone(long _idGuild, GuildStone _guildStone)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_SET_GUILDSTONE_APP");
            try
            {
                SqlCommand command = new SqlCommand("SetGuildStone2", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@server", SqlDbType.NVarChar, 0x20).Value = _guildStone.server;
                command.Parameters.Add("@position_id", SqlDbType.BigInt, 8).Value = _guildStone.position_id;
                command.Parameters.Add("@type", SqlDbType.Int, 4).Value = _guildStone.type;
                command.Parameters.Add("@region", SqlDbType.SmallInt, 2).Value = _guildStone.region;
                command.Parameters.Add("@x", SqlDbType.Int, 4).Value = _guildStone.x;
                command.Parameters.Add("@y", SqlDbType.Int, 4).Value = _guildStone.y;
                command.Parameters.Add("@direction", SqlDbType.Real, 4).Value = _guildStone.direction;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_SET_GUILDSTONE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild, _guildStone);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_SET_GUILDSTONE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild, _guildStone);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool TransferGuildMaster(long _idGuild, long _idOldMaster, long _idNewMaster)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_TRANSFER_MASTER_APP");
            try
            {
                SqlCommand command = new SqlCommand("TransferGuildMaster", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@idOldMaster", SqlDbType.BigInt, 8).Value = _idOldMaster;
                command.Parameters.Add("@idNewMaster", SqlDbType.BigInt, 8).Value = _idNewMaster;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_TRANSFER_MASTER_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild, _idOldMaster);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_TRANSFER_MASTER_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild, _idOldMaster);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public int UpdateBattleGroundType(long _idGuild, int _guildPoint, int _guildMoney, byte _battleGroundType)
        {
            int num2;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_UPDATE_BGTYPE_APP");
            try
            {
                SqlCommand command = new SqlCommand("dbo.UpdateGuildBattleGroundType", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@guildPoint", SqlDbType.Int, 4).Value = _guildPoint;
                command.Parameters.Add("@guildMoney", SqlDbType.Int, 4).Value = _guildMoney;
                command.Parameters.Add("@battlegroundtype", SqlDbType.TinyInt).Value = _battleGroundType;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 명령을 실행합니다.");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                }
                else
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 롤백합니다.");
                    transaction.Rollback("GUILD_UPDATE_BGTYPE_APP");
                }
                num2 = num;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_BGTYPE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                num2 = -1;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_BGTYPE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                num2 = -1;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 연결을 종료합니다");
                connection.Close();
            }
            return num2;
        }

        public bool UpdateBattleGroundWinnerType(long _idGuild, byte _BattleGroundWinnerType)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_UPDATE_BGGRADE_APP");
            try
            {
                SqlCommand command = new SqlCommand("dbo.UpdateGuildBattleGroundWinnerType", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@BattleGroundWinnerType", SqlDbType.TinyInt).Value = _BattleGroundWinnerType;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 명령을 실행합니다.");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 트랜잭션을 커밋합니다.");
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_BGGRADE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_BGGRADE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool UpdateGuildProperties(long _idGuild, int _iGP, int _iMoney, int _iLevel)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_UPDATE_PROPERTIES");
            try
            {
                SqlCommand command = new SqlCommand("UpdateGuild", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@intGuildpoint", SqlDbType.Int, 4).Value = _iGP;
                command.Parameters.Add("@intGuildmoney", SqlDbType.Int, 4).Value = _iMoney;
                command.Parameters.Add("@flag", SqlDbType.Int, 4).Value = _iLevel;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback("GUILD_UPDATE_PROPERTIES");
                }
                flag = num == 0;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_PROPERTIES");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_PROPERTIES");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public REPLY_RESULT UpdateGuildRobe(long _idGuild, int _guildPoint, int _guildMoney, GuildRobe _guildRobe, out byte _errorCode)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 함수에 진입하였습니다");
            _errorCode = 0;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_UPDATE_GUILDROBE_APP");
            try
            {
                SqlCommand command = new SqlCommand("dbo.UpdateGuildRobe", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@guildPoint", SqlDbType.Int, 4).Value = _guildPoint;
                command.Parameters.Add("@guildMoney", SqlDbType.Int, 4).Value = _guildMoney;
                command.Parameters.Add("@robeEmblemChestIcon", SqlDbType.TinyInt, 1).Value = _guildRobe.emblemChestIcon;
                command.Parameters.Add("@robeEmblemChestDeco", SqlDbType.TinyInt, 1).Value = _guildRobe.emblemChestDeco;
                command.Parameters.Add("@robeEmblemBeltDeco", SqlDbType.TinyInt, 1).Value = _guildRobe.emblemBeltDeco;
                command.Parameters.Add("@robeColor1", SqlDbType.Int, 4).Value = _guildRobe.color1;
                command.Parameters.Add("@robeColor2Index", SqlDbType.TinyInt, 1).Value = _guildRobe.color2Index;
                command.Parameters.Add("@robeColor3Index", SqlDbType.TinyInt, 1).Value = _guildRobe.color3Index;
                command.Parameters.Add("@robeColor4Index", SqlDbType.TinyInt, 1).Value = _guildRobe.color4Index;
                command.Parameters.Add("@robeColor5Index", SqlDbType.TinyInt, 1).Value = _guildRobe.color5Index;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 명령을 실행합니다.");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                _errorCode = (byte) num;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 롤백합니다.");
                transaction.Rollback("GUILD_UPDATE_GUILDROBE_APP");
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_GUILDROBE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild, _guildRobe);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                fAIL = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_GUILDROBE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild, _guildRobe);
                WorkSession.WriteStatus(exception2.Message);
                fAIL = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }

        public bool UpdateGuildStatus(long _idGuild, byte _statusFlag, bool _set, int _guildPointRequired)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_UPDATE_STATUS");
            try
            {
                SqlCommand command = new SqlCommand("dbo.UpdateGuildStatus", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@statusFlag", SqlDbType.TinyInt).Value = _statusFlag;
                command.Parameters.Add("@set", SqlDbType.Bit).Value = _set;
                command.Parameters.Add("@guildPointRequired", SqlDbType.Int).Value = _guildPointRequired;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 명령을 실행합니다.");
                if (command.ExecuteNonQuery() > 0)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return true;
                }
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_STATUS");
                }
                flag = false;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_STATUS");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_STATUS");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool UpdateTitle(long _idGuild, string _strGuildTitle, bool _bUsable)
        {
            bool flag;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_UPDATE_TITLE");
            try
            {
                SqlCommand command = new SqlCommand("dbo.UpdateGuildTitle", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@title", SqlDbType.NVarChar, 20).Value = _strGuildTitle;
                command.Parameters.Add("@usable", SqlDbType.Bit).Value = _bUsable;
                command.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 명령을 실행합니다.");
                if (command.ExecuteNonQuery() > 0)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return true;
                }
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_TITLE");
                }
                flag = false;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_TITLE");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_UPDATE_TITLE");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public REPLY_RESULT WithdrawDrawableMoney(long _idGuild, int _money, out int _remainMoney, out int _remainDrawableMoney)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 데이터베이스와 연결합니다");
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction("GUILD_DRAWMONEY_APP");
            try
            {
                SqlCommand command = new SqlCommand("WithdrawDrawableMoney", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command.Parameters.Add("@intMoney", SqlDbType.Int, 4).Value = _money;
                command.Transaction = transaction;
                SqlCommand command2 = new SqlCommand("WithdrawGuildMoney", connection);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 8).Value = _idGuild;
                command2.Parameters.Add("@intMoney", SqlDbType.Int, 4).Value = _money;
                command2.Transaction = transaction;
                WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 명령을 실행합니다");
                _remainDrawableMoney = (int) command.ExecuteScalar();
                _remainMoney = (int) command2.ExecuteScalar();
                if ((_remainDrawableMoney >= 0) && (_remainMoney >= 0))
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                _remainDrawableMoney += _money;
                _remainMoney += _money;
                transaction.Rollback();
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_DRAWMONEY_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _idGuild, _money);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _remainDrawableMoney = 0;
                _remainMoney = 0;
                fAIL = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("GUILD_DRAWMONEY_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _idGuild, _money);
                WorkSession.WriteStatus(exception2.Message);
                _remainDrawableMoney = 0;
                _remainMoney = 0;
                fAIL = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }

        public bool Write(Guild _data)
        {
            return base.Write(_data);
        }
    }
}

