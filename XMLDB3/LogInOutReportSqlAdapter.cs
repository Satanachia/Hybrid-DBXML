namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class LogInOutReportSqlAdapter : SqlAdapter, LogInOutReportAdapter
    {
        public void Initialize(string _argument)
        {
            this.Initialize(typeof(LogInOutReport), _argument);
        }

        public bool ReportLogInOut(LogInOutReport _report)
        {
            bool flag;
            if (_report == null)
            {
                return false;
            }
            WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 함수에 진입하였습니다");
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction("LOGINOUT_REPORT");
                SqlCommand command = new SqlCommand("uspInsertMabiIPlog", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@loginID", SqlDbType.VarChar, 50).Value = _report.account;
                command.Parameters.Add("@IP", SqlDbType.VarChar, 50).Value = _report.ip;
                command.Parameters.Add("@CountryCode", SqlDbType.VarChar, 5).Value = _report.countrycode;
                command.Parameters.Add("@logType", SqlDbType.VarChar, 2).Value = _report.inout;
                command.Transaction = transaction;
                WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 명령을 실행합니다");
                int num = command.ExecuteNonQuery();
                WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 트랜잭션을 커밋합니다");
                transaction.Commit();
                if (num > 0)
                {
                    return true;
                }
                flag = false;
            }
            catch (SqlException exception)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("LOGINOUT_REPORT");
                }
                ExceptionMonitor.ExceptionRaised(exception, _report);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("LOGINOUT_REPORT");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _report);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

