namespace XMLDB3
{
    using System;
    using System.Data.SqlClient;
    using System.Text;

    public class MemoSqlAdapter : SqlAdapter, MemoAdapter
    {
        public void Initialize(string _Argument)
        {
            this.Initialize(typeof(Memo), _Argument);
        }

        public bool SendMemo(Memo _memo)
        {
            bool flag;
            WorkSession.WriteStatus("MemoSqlAdapter.SendMemo() : 함수에 진입하였습니다.");
            if (_memo.receipants == null)
            {
                WorkSession.WriteStatus("MemoSqlAdapter.SendMemo() : 받는 사람이 없어 바로 리턴합니다.");
                return true;
            }
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("SendMemo.SendMemo() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("MEMO_SEND_APP");
                    StringBuilder builder = new StringBuilder();
                    foreach (MemoCharacter character in _memo.receipants)
                    {
                        string str = "exec dbo.usp_Memo_Insert  @M_fromname=" + UpdateUtility.BuildString(_memo.sender.name) + ",@M_fromid=" + UpdateUtility.BuildString(_memo.sender.account) + ",@M_toname=" + UpdateUtility.BuildString(character.name) + ",@M_toid=" + UpdateUtility.BuildString(character.account) + ",@M_content=" + UpdateUtility.BuildString(_memo.content) + ",@Tuser_Server=" + UpdateUtility.BuildString(_memo.sender.server) + ",@Tuser_Level=3,@Fuser_Server=" + UpdateUtility.BuildString(_memo.sender.server) + ",@Fuser_Level=0,@OutFlag=1\n";
                        builder.Append(str);
                    }
                    SqlCommand command = new SqlCommand(builder.ToString(), connection);
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("SendMemo.SendMemo() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("MEMO_SEND_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("MEMO_SEND_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2);
                    WorkSession.WriteStatus(exception2.Message);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.Read() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                WorkSession.WriteStatus(exception3.Message);
                ExceptionMonitor.ExceptionRaised(exception3);
                flag = false;
            }
            return flag;
        }
    }
}

