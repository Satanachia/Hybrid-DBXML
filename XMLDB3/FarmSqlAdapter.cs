namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class FarmSqlAdapter : SqlAdapter, FarmAdapter
    {
        public REPLY_RESULT Expire(long _farmID, ref byte _errorCode)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FARM_EXPIRE_APP");
                SqlCommand command = new SqlCommand("dbo.FarmExpire", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idFarm", SqlDbType.BigInt, 8).Value = _farmID;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                transaction.Rollback("FARM_EXPIRE_APP");
                _errorCode = (byte) num;
                fAIL = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _farmID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FARM_EXPIRE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _farmID);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FARM_EXPIRE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }

        public bool GetOwnerInfo(string _account, ref long _farmID, ref long _ownerCharID, ref string _ownerCharName)
        {
            bool flag;
            WorkSession.WriteStatus("FarmSqlAdapter.GetFarmOwnInfo() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("FarmSqlAdapter.GetFarmOwnInfo() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand("dbo.FarmGetOwnerInfo", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20).Value = _account;
                SqlDataReader reader = null;
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        _farmID = (long) reader["farmID"];
                        _ownerCharID = (long) reader["ownerCharID"];
                        _ownerCharName = (string) reader["ownerCharName"];
                        return true;
                    }
                    flag = false;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _account);
                WorkSession.WriteStatus(exception.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("FarmSqlAdapter.GetFarmOwnInfo() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("FarmSelect", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idFarm", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Farm), _argument);
        }

        public REPLY_RESULT Lease(long _farmID, string _account, long _charID, string _charName, long _expireTime, ref byte _errorCode)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FARM_LEASE_APP");
                SqlCommand command = new SqlCommand("dbo.FarmLease", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idFarm", SqlDbType.BigInt, 8).Value = _farmID;
                command.Parameters.Add("@strAccount", SqlDbType.NVarChar, 0x20).Value = _account;
                command.Parameters.Add("@ownerCharID", SqlDbType.BigInt, 8).Value = _charID;
                command.Parameters.Add("@ownerCharName", SqlDbType.NVarChar, 0x40).Value = _charName;
                command.Parameters.Add("@expireTime", SqlDbType.BigInt, 8).Value = _expireTime;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                transaction.Rollback("FARM_LEASE_APP");
                _errorCode = (byte) num;
                fAIL = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _farmID, _account);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FARM_LEASE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _farmID, _account);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FARM_LEASE_APP");
                }
                fAIL = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }

        public Farm Read(long _farmID)
        {
            return (Farm) base.Read(_farmID);
        }

        public REPLY_RESULT Update(Farm _farm, ref byte _errorCode)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("FarmSqlAdapter.Update() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Update() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("FARM_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.FarmUpdate", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idFarm", SqlDbType.BigInt).Value = _farm.farmID;
                command.Parameters.Add("@crop", SqlDbType.TinyInt).Value = _farm.crop;
                command.Parameters.Add("@plantTime", SqlDbType.BigInt).Value = _farm.plantTime;
                command.Parameters.Add("@waterWork", SqlDbType.SmallInt).Value = _farm.waterWork;
                command.Parameters.Add("@nutrientWork", SqlDbType.SmallInt).Value = _farm.nutrientWork;
                command.Parameters.Add("@insectWork", SqlDbType.SmallInt).Value = _farm.insectWork;
                command.Parameters.Add("@water", SqlDbType.SmallInt).Value = _farm.water;
                command.Parameters.Add("@nutrient", SqlDbType.SmallInt).Value = _farm.nutrient;
                command.Parameters.Add("@insect", SqlDbType.SmallInt).Value = _farm.insect;
                command.Parameters.Add("@growth", SqlDbType.SmallInt).Value = _farm.growth;
                command.Parameters.Add("@currentWork", SqlDbType.TinyInt).Value = _farm.currentWork;
                command.Parameters.Add("@workCompleteTime", SqlDbType.BigInt).Value = _farm.workCompleteTime;
                command.Parameters.Add("@todayWorkCount", SqlDbType.TinyInt).Value = _farm.todayWorkCount;
                command.Parameters.Add("@lastWorkTime", SqlDbType.BigInt).Value = _farm.lastWorkTime;
                command.Transaction = transaction;
                WorkSession.WriteStatus("FarmSqlAdapter.Update() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                if (num == 0)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Update() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                transaction.Rollback("FARM_UPDATE_APP");
                _errorCode = (byte) num;
                fAIL = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FARM_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _farm.farmID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("FARM_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _farm.farmID);
                WorkSession.WriteStatus(exception2.Message);
                fAIL = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return fAIL;
        }
    }
}

