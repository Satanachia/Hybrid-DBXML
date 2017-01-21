namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class FamilySqlAdapter : SqlAdapter, FamilyAdapter
    {
        private REPLY_RESULT _AddMember(SqlConnection _con, SqlTransaction _con_tran, long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            SqlCommand command = new SqlCommand("dbo.FamilyMemberAdd", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@charId", SqlDbType.BigInt).Value = _member.memberID;
            command.Parameters.Add("@familyId", SqlDbType.BigInt).Value = _familyID;
            command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = _member.memberName;
            command.Parameters.Add("@class", SqlDbType.SmallInt).Value = (short) _member.memberClass;
            command.Transaction = _con_tran;
            WorkSession.WriteStatus("FamilySqlAdapter._AddMember() : 명령을 실행합니다");
            if (command.ExecuteNonQuery() == 0)
            {
                return REPLY_RESULT.FAIL;
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT AddFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("FamilySqlAdapter.Add() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FAMILY_ADD_APP");
                SqlCommand command = new SqlCommand("dbo.FamilyAdd", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.BigInt).Value = _family.familyID;
                command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = _family.familyName;
                command.Parameters.Add("@headMemberId", SqlDbType.BigInt).Value = _family.headID;
                command.Parameters.Add("@state", SqlDbType.SmallInt).Value = (short) _family.state;
                command.Parameters.Add("@tradition", SqlDbType.Int).Value = (int) _family.tradition;
                command.Parameters.Add("@meta", SqlDbType.NVarChar, 200).Value = _family.meta;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 명령을 실행합니다");
                if (command.ExecuteNonQuery() == 0)
                {
                    Exception exception = new Exception("패밀리 추가에 실패하였습니다.", null);
                    ExceptionMonitor.ExceptionRaised(exception, _family.familyID);
                    WorkSession.WriteStatus(exception.Message, 0);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                        transaction.Rollback("FAMILY_ADD_APP");
                    }
                    return REPLY_RESULT.FAIL;
                }
                foreach (FamilyListFamilyMember member in _family.member)
                {
                    if (this._AddMember(connection, transaction, _family.familyID, member, ref _errorCode) != REPLY_RESULT.SUCCESS)
                    {
                        Exception exception2 = new Exception("패밀리 멤버 추가에 실패하였습니다.", null);
                        ExceptionMonitor.ExceptionRaised(exception2, _family.familyID);
                        WorkSession.WriteStatus(exception2.Message, 0);
                        if (transaction != null)
                        {
                            WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                            transaction.Rollback("FAMILY_ADD_APP");
                        }
                        return REPLY_RESULT.FAIL;
                    }
                }
                WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _family.familyID);
                WorkSession.WriteStatus(exception3.Message, exception3.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILY_ADD_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception4)
            {
                ExceptionMonitor.ExceptionRaised(exception4, _family.familyID);
                WorkSession.WriteStatus(exception4.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILY_ADD_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        public REPLY_RESULT AddMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FAMILYMEMBER_ADD_APP");
                this._AddMember(connection, transaction, _familyID, _member, ref _errorCode);
                WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _member.memberID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILYMEMBER_ADD_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _member.memberID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILYMEMBER_ADD_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        private FamilyList Build(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("패밀리 테이블을 얻어오지 못햇습니다.");
            }
            if ((_table.Rows == null) || (_table.Rows.Count <= 0))
            {
                return new FamilyList();
            }
            FamilyList list = new FamilyList();
            list.family = new FamilyListFamily[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                list.family[i] = new FamilyListFamily();
                list.family[i].familyID = (long) _table.Rows[i]["id"];
                list.family[i].familyName = (string) _table.Rows[i]["name"];
                list.family[i].headID = (long) _table.Rows[i]["headMemberId"];
                list.family[i].state = (ushort) ((short) _table.Rows[i]["state"]);
                list.family[i].tradition = (uint) ((int) _table.Rows[i]["tradition"]);
                list.family[i].meta = (string) _table.Rows[i]["meta"];
                list.family[i].member = this.ReadFamilyMember(list.family[i]);
            }
            return list;
        }

        private FamilyListFamilyMember[] BuildFamilyMember(DataTable _table)
        {
            if (_table == null)
            {
                throw new Exception("패밀리멤버 테이블을 얻어오지 못햇습니다.");
            }
            if ((_table.Rows == null) || (_table.Rows.Count <= 0))
            {
                return null;
            }
            FamilyListFamilyMember[] memberArray = new FamilyListFamilyMember[_table.Rows.Count];
            for (int i = 0; i < _table.Rows.Count; i++)
            {
                memberArray[i] = new FamilyListFamilyMember();
                memberArray[i].memberID = (long) _table.Rows[i]["charId"];
                memberArray[i].memberName = (string) _table.Rows[i]["name"];
                memberArray[i].memberClass = (ushort) ((short) _table.Rows[i]["class"]);
            }
            return memberArray;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(FamilyListFamily), _argument);
        }

        public FamilyListFamily Read(long _familyID)
        {
            FamilyListFamily family2;
            WorkSession.WriteStatus("FamilySqlAdapter.Read() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand command = new SqlCommand("dbo.FamilySelect", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.BigInt, 8).Value = _familyID;
                WorkSession.WriteStatus("FamilySqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                connection.Open();
                WorkSession.WriteStatus("FamilySqlAdapter.Read() : 데이터를 채웁니다.");
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        FamilyListFamily family = new FamilyListFamily();
                        family.familyID = _familyID;
                        family.familyName = (string) reader["name"];
                        family.headID = (long) reader["headMemberId"];
                        family.state = (ushort) ((short) reader["state"]);
                        family.tradition = (uint) ((int) reader["tradition"]);
                        family.meta = (string) reader["meta"];
                        family.member = this.ReadFamilyMember(family);
                        return family;
                    }
                    family2 = null;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _familyID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                family2 = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _familyID);
                WorkSession.WriteStatus(exception2.Message);
                family2 = null;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return family2;
        }

        public FamilyListFamilyMember[] ReadFamilyMember(FamilyListFamily _family)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand selectCommand = new SqlCommand("dbo.FamilyMemberSelect", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.Add("@familyId", SqlDbType.BigInt, 8).Value = _family.familyID;
                WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 데이터 베이스에 연결합니다.");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "FamilyMember");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                return this.BuildFamilyMember(dataSet.Tables["FamilyMember"]);
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
                WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return null;
        }

        public FamilyList ReadList()
        {
            WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 함수에 진입하였습니다.");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                SqlCommand selectCommand = new SqlCommand("dbo.FamilySelectAll", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "Family");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 데이터를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                return this.Build(dataSet.Tables["Family"]);
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
                WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
                connection.Close();
            }
            return null;
        }

        public REPLY_RESULT RemoveFamily(long _familyID, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FAMILY_REMOVE_APP");
                SqlCommand command = new SqlCommand("dbo.FamilyRemove", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@iD", SqlDbType.BigInt, 8).Value = _familyID;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILY_REMOVE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILY_REMOVE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        public REPLY_RESULT RemoveMember(long _familyID, long _memberID, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FAMILYMEMBER_REMOVE_APP");
                SqlCommand command = new SqlCommand("dbo.FamilyMemberRemove", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@charId", SqlDbType.BigInt, 8).Value = _memberID;
                command.Parameters.Add("@familyId", SqlDbType.BigInt, 8).Value = _familyID;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILYMEMBER_REMOVE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILYMEMBER_REMOVE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        public REPLY_RESULT UpdateFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FAMILY_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.FamilyUpdate", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", SqlDbType.BigInt).Value = _family.familyID;
                command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = _family.familyName;
                command.Parameters.Add("@headMemberId", SqlDbType.BigInt).Value = _family.headID;
                command.Parameters.Add("@state", SqlDbType.SmallInt).Value = (short) _family.state;
                command.Parameters.Add("@tradition", SqlDbType.Int).Value = (int) _family.tradition;
                command.Parameters.Add("@meta", SqlDbType.NVarChar, 200).Value = _family.meta;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _family.familyID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILY_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _family.familyID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILY_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }

        public REPLY_RESULT UpdateMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FAMILYMEMBER_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.FamilyMemberUpdate", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@charId", SqlDbType.BigInt).Value = _member.memberID;
                command.Parameters.Add("@familyId", SqlDbType.BigInt).Value = _familyID;
                command.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = _member.memberName;
                command.Parameters.Add("@class", SqlDbType.SmallInt).Value = (short) _member.memberClass;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _member.memberID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILYMEMBER_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _member.memberID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FAMILYMEMBER_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }
    }
}

