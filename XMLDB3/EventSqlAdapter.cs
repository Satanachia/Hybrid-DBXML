namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class EventSqlAdapter : SqlAdapter, EventAdapter
    {
        private const byte NEWYEAR_NEXON_EVENT_TYPE = 1;

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Event), _argument);
        }

        public REPLY_RESULT Update(Event _event, ref byte _errorCode)
        {
            REPLY_RESULT sUCCESS;
            if (_event.eventType != 1)
            {
                return REPLY_RESULT.FAIL;
            }
            WorkSession.WriteStatus("EventSqlAdapter.Update() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("EventSqlAdapter.Update() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("EVENT_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.newYearNexonEvent", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@account", SqlDbType.NVarChar, 0x24)).Value = _event.account;
                command.Parameters.Add(new SqlParameter("@characterName", SqlDbType.NVarChar, 50)).Value = _event.charName;
                command.Parameters.Add(new SqlParameter("@server", SqlDbType.NVarChar, 20)).Value = _event.serverName;
                WorkSession.WriteStatus("EventSqlAdapter.Update() : 명령을 실행합니다");
                command.Transaction = transaction;
                command.ExecuteNonQuery();
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _event.account, _event.charName, _event.serverName);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("EVENT_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _event.account, _event.charName, _event.serverName);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("EVENT_UPDATE_APP");
                }
                sUCCESS = REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("EventSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return sUCCESS;
        }
    }
}

