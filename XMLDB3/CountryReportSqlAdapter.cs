namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class CountryReportSqlAdapter : SqlAdapter, CountryReportAdapter
    {
        public void Initialize(string _argument)
        {
            this.Initialize(typeof(CountryReport), _argument);
        }

        public bool ReportCCU(CountryReport _report)
        {
            bool flag;
            if (_report == null)
            {
                return false;
            }
            WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 함수에 진입하였습니다");
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction("COUNTRY_CCU_REPORT");
                SqlCommand command = new SqlCommand("uspInsertMabiRecord", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@userCountData", SqlDbType.VarChar, 0x1770).Value = _report.reportstring;
                command.Transaction = transaction;
                WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 명령을 실행합니다");
                int num = command.ExecuteNonQuery();
                WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 트랜잭션을 커밋합니다");
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
                    WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("COUNTRY_CCU_REPORT");
                }
                ExceptionMonitor.ExceptionRaised(exception, _report);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                if (transaction != null)
                {
                    WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    transaction.Rollback("COUNTRY_CCU_REPORT");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _report);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

