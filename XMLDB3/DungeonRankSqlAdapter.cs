namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    public class DungeonRankSqlAdapter : SqlAdapter, DungeonRankAdapter
    {
        public void Initialize(string _argument)
        {
            this.Initialize(typeof(DungeonRank), _argument);
        }

        public bool Update(DungeonRank _dungeonRank)
        {
            bool flag;
            WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 데이터 베이스에 연결합니다.");
                connection.Open();
                transaction = connection.BeginTransaction("DUNGEONRANK_UPDATE_APP");
                SqlCommand command = new SqlCommand("dbo.UpdateDungeonRank", connection, transaction);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@server", SqlDbType.NVarChar, 20).Value = _dungeonRank.server;
                command.Parameters.Add("@dungeonName", SqlDbType.NVarChar, 80).Value = _dungeonRank.dungeonName;
                command.Parameters.Add("@race", SqlDbType.TinyInt).Value = _dungeonRank.race;
                command.Parameters.Add("@score", SqlDbType.Int).Value = _dungeonRank.score;
                command.Parameters.Add("@lapTime", SqlDbType.Int).Value = _dungeonRank.laptime;
                command.Parameters.Add("@characterID", SqlDbType.BigInt).Value = _dungeonRank.characterID;
                command.Parameters.Add("@charactername", SqlDbType.NVarChar, 50).Value = _dungeonRank.characterName;
                WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _dungeonRank.server, _dungeonRank.characterID, _dungeonRank.dungeonName);
                WorkSession.WriteStatus(exception.Message);
                if (transaction != null)
                {
                    transaction.Rollback("DUNGEONRANK_UPDATE_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _dungeonRank.server, _dungeonRank.characterID, _dungeonRank.dungeonName);
                WorkSession.WriteStatus(exception2.Message);
                if (transaction != null)
                {
                    transaction.Rollback("DUNGEONRANK_UPDATE_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

