namespace XMLDB3
{
    using System;
    using System.Data.SqlClient;

    public class PromotionSqlAdapter : SqlAdapter, PromotionAdapter
    {
        public bool BeginPromotion(string serverName, string channelName, ushort skillid)
        {
            bool flag;
            WorkSession.WriteStatus("PromotionSqlAdapter.BeginPromotion() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                string cmdText = string.Empty;
                WorkSession.WriteStatus("PromotionSqlAdapter.BeginPromotion() : 명령을 생성합니다.");
                object obj2 = cmdText;
                cmdText = string.Concat(new object[] { obj2, "exec dbo.beginPromotionTest @server=", UpdateUtility.BuildString(serverName), ",@channel=", UpdateUtility.BuildString(channelName), ",@skillID=", skillid, "\n" });
                WorkSession.WriteStatus("PromotionSqlAdapter.BeginPromotion() : 데이터 베이스에 연결합니다.");
                connection.Open();
                transaction = connection.BeginTransaction("PROMOTION_BEGIN_APP");
                SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                WorkSession.WriteStatus("PromotionSqlAdapter.BeginPromotion() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, serverName, skillid);
                WorkSession.WriteStatus(exception.Message);
                if (transaction != null)
                {
                    transaction.Rollback("PROMOTION_BEGIN_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, serverName, skillid);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("PROMOTION_BEGIN_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("PromotionSqlAdapter.BeginPromotion() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool EndPromotion(string serverName, ushort skillid)
        {
            bool flag;
            WorkSession.WriteStatus("PromotionSqlAdapter.EndPromotion() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                string cmdText = string.Empty;
                WorkSession.WriteStatus("PromotionSqlAdapter.EndPromotion() : 명령을 생성합니다.");
                object obj2 = cmdText;
                cmdText = string.Concat(new object[] { obj2, "exec dbo.updatePromotionRank @server=", UpdateUtility.BuildString(serverName), ",@skillID=", skillid, "\n" });
                WorkSession.WriteStatus("PromotionSqlAdapter.EndPromotion() : 데이터 베이스에 연결합니다.");
                connection.Open();
                transaction = connection.BeginTransaction("PROMOTION_END_APP");
                SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                WorkSession.WriteStatus("PromotionSqlAdapter.EndPromotion() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, serverName, skillid);
                WorkSession.WriteStatus(exception.Message);
                if (transaction != null)
                {
                    transaction.Rollback("PROMOTION_END_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, serverName, skillid);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("PROMOTION_END_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("PromotionSqlAdapter.EndPromotion() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public void Initialize(string _Argument)
        {
            this.Initialize(typeof(PromotionRecord), _Argument);
        }

        public bool RecordScore(string serverName, string channelName, ushort skillid, string skillCategory, string skillName, ulong characterID, string characterName, byte race, ushort level, uint point)
        {
            bool flag;
            WorkSession.WriteStatus("PromotionSqlAdapter.RecordScore() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                string cmdText = string.Empty;
                WorkSession.WriteStatus("PromotionSqlAdapter.RecordScore() : 명령을 생성합니다.");
                object obj2 = cmdText;
                cmdText = string.Concat(new object[] { 
                    obj2, "exec dbo.recordPromotionPoint @server=", UpdateUtility.BuildString(serverName), ",@channel=", UpdateUtility.BuildString(channelName), ",@skillID=", skillid, ",@skillCategory=", UpdateUtility.BuildString(skillCategory), ",@skillName=", UpdateUtility.BuildString(skillName), ",@race=", race, ",@characterName=", UpdateUtility.BuildString(characterName), ",@characterID=", 
                    characterID, ",@level=", level, ",@point=", point, "\n"
                 });
                WorkSession.WriteStatus("PromotionSqlAdapter.RecordScore() : 데이터 베이스에 연결합니다.");
                connection.Open();
                transaction = connection.BeginTransaction("PROMOTION_RECORD_APP");
                SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                WorkSession.WriteStatus("PromotionSqlAdapter.RecordScore() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, serverName, skillid);
                WorkSession.WriteStatus(exception.Message);
                if (transaction != null)
                {
                    transaction.Rollback("PROMOTION_RECORD_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, serverName, skillid);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("PROMOTION_RECORD_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("PromotionSqlAdapter.RecordScore() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

