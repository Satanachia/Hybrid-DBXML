namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class CastleSqlAdapter : SqlAdapter, CastleAdapter
    {
        public bool CreateBid(CastleBid _bid)
        {
            bool flag;
            WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("CreateCastleBid", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter parameter = command.Parameters.Add("@idCastle", SqlDbType.BigInt, 0x40);
                    SqlParameter parameter2 = command.Parameters.Add("@bidEndTime", SqlDbType.DateTime, 0x40);
                    SqlParameter parameter3 = command.Parameters.Add("@minBidPrice", SqlDbType.Int, 0x20);
                    parameter.Value = _bid.castleID;
                    parameter2.Value = _bid.bidEndTime;
                    parameter3.Value = _bid.minBidPrice;
                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 명령을 실행합니다");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _bid);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _bid);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public REPLY_RESULT CreateBidder(CastleBidder _bidder, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            REPLY_RESULT eRROR;
            WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _guildAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            string cmdText = "";
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CASLTE_BIDDER_CREATE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CASLTE_BIDDER_CREATE_APP");
                cmdText = string.Concat(new object[] { "exec CreateCastleBidder @idCastle=", _bidder.castleID, ",@idGuild=", _bidder.bidGuildID, ",@bidPrice=", _bidder.bidPrice, ",@bidOrder=", _bidder.bidOrder, ",@bidGuildName=", UpdateUtility.BuildString(_bidder.bidGuildName), ",@bidCharacter=", _bidder.bidCharacter, ",@bidCharName=", UpdateUtility.BuildString(_bidder.bidCharName) });
                SqlCommand command = new SqlCommand(cmdText, connection);
                command.Transaction = transaction;
                SqlCommand command2 = new SqlCommand("WithdrawGuildMoney", connection2);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.Transaction = transaction2;
                SqlParameter parameter = command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                SqlParameter parameter2 = command2.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter.Value = _bidder.bidGuildID;
                parameter2.Value = _bidder.bidPrice;
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 입찰자를 생성합니다.");
                command.ExecuteNonQuery();
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 길드에서 돈을 인출합니다.");
                _remainMoney = (int) command2.ExecuteScalar();
                if (_remainMoney >= 0)
                {
                    transaction2.Commit();
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                _remainMoney += _bidder.bidPrice;
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 길드 머니가 부족합니다.");
                transaction2.Rollback("CASLTE_BIDDER_CREATE_APP");
                transaction.Rollback("CASLTE_BIDDER_CREATE_APP");
                eRROR = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BIDDER_CREATE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BIDDER_CREATE_APP");
                }
                if (exception.Number == 0xc350)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 길드 [" + _bidder.bidGuildID.ToString() + "] 가 이미 성을 소유하고 있거나, 입찰에 참여중입니다.");
                    return REPLY_RESULT.FAIL;
                }
                ExceptionMonitor.ExceptionRaised(exception, _bidder, cmdText);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                eRROR = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BIDDER_CREATE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BIDDER_CREATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _bidder);
                WorkSession.WriteStatus(exception2.Message);
                eRROR = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return eRROR;
        }

        public REPLY_RESULT DeleteBidder(long _castleID, long _guildID, int _repayMoney, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            REPLY_RESULT sUCCESS;
            WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _guildAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CASLTE_BIDDER_DELETE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CASLTE_BIDDER_DELETE_APP");
                SqlCommand command = new SqlCommand("DeleteCastleBidder", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction;
                SqlParameter parameter = command.Parameters.Add("@idCastle", SqlDbType.BigInt, 0x40);
                SqlParameter parameter2 = command.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                parameter.Value = _castleID;
                parameter2.Value = _guildID;
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 입찰자를 삭제합니다.");
                if (command.ExecuteNonQuery() < 1)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 길드가 입찰하지 않았습니다.");
                    ExceptionMonitor.ExceptionRaised(new Exception("길드 [" + _guildID.ToString() + "] 는 입찰하지 않았습니다."));
                    transaction2.Rollback("CASLTE_BIDDER_DELETE_APP");
                    transaction.Rollback("CASLTE_BIDDER_DELETE_APP");
                    return REPLY_RESULT.FAIL;
                }
                SqlCommand command2 = new SqlCommand("AddGuildMoney", connection2);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.Transaction = transaction2;
                SqlParameter parameter3 = command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                SqlParameter parameter4 = command2.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter3.Value = _guildID;
                parameter4.Value = _repayMoney;
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 길드 머니를 업데이트 합니다.");
                _remainMoney = (int) command2.ExecuteScalar();
                transaction2.Commit();
                transaction.Commit();
                sUCCESS = REPLY_RESULT.SUCCESS;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BIDDER_DELETE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BIDDER_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _castleID, _guildID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                sUCCESS = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BIDDER_DELETE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BIDDER_DELETE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _castleID, _guildID);
                WorkSession.WriteStatus(exception2.Message);
                sUCCESS = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return sUCCESS;
        }

        public bool EndBid(Castle _castle, GuildAdapter _guildAdapter)
        {
            bool flag;
            WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _guildAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CASLTE_BID_END_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CASLTE_BID_END_APP");
                SqlCommand selectCommand = new SqlCommand(string.Concat(new object[] { 
                    "exec EndCastleBid  @idCastle=", _castle.castleID, ",@idGuild=", _castle.guildID, ",@constructed=", _castle.constructed, ",@castleMoney=", _castle.castleMoney, ",@weeklyIncome=", _castle.weeklyIncome, ",@taxrate=", _castle.taxrate, ",@updateTime=", UpdateUtility.BuildDateTime(_castle.updateTime), ",@flag=", _castle.flag, 
                    ",@sellDungeonPass=", _castle.sellDungeonPass, ",@dungeonPassPrice=", _castle.dungeonPassPrice
                 }), connection);
                selectCommand.Transaction = transaction;
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 경매를 종료합니다.");
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : DataSet 에 낙찰 되지 않은 길드 정보를 채웁니다.");
                adapter.Fill(dataSet);
                adapter.Dispose();
                if (dataSet.Tables == null)
                {
                    throw new Exception("CastleSqlAdapter.EndBid() : 경매를 종료하는 중, 낙찰 받지 못한 길드를 얻어오는데 실패함");
                }
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : DataSet 에 낙찰 되지 않은 길드 머니를 지급합니다.");
                if ((dataSet.Tables[0].Rows != null) && (dataSet.Tables[0].Rows.Count > 0))
                {
                    SqlCommand command2 = new SqlCommand("AddGuildMoney", connection2);
                    command2.CommandType = System.Data.CommandType.StoredProcedure;
                    command2.Transaction = transaction2;
                    SqlParameter parameter = command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                    SqlParameter parameter2 = command2.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        parameter.Value = (long) row["bidGuildID"];
                        parameter2.Value = (int) row["bidPrice"];
                        command2.ExecuteNonQuery();
                    }
                }
                transaction2.Commit();
                transaction.Commit();
                flag = true;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BID_END_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BID_END_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _castle);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BID_END_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BID_END_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _castle);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return flag;
        }

        public REPLY_RESULT GiveGuildMoney(long _castleID, long _guildID, int _money, GuildAdapter _guildAdapter)
        {
            REPLY_RESULT fAIL;
            WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _guildAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CASLTE_GUILD_MONEY_GIVE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CASLTE_GUILD_MONEY_GIVE_APP");
                SqlCommand command = new SqlCommand("WithdrawCastleMoney", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction;
                SqlParameter parameter = command.Parameters.Add("@idCastle", SqlDbType.BigInt, 0x40);
                SqlParameter parameter2 = command.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter.Value = _castleID;
                parameter2.Value = _money;
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 성의 돈을 업데이트 합니다.");
                if (((int) command.ExecuteScalar()) >= 0)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 길드 머니를 업데이트 합니다.");
                    SqlCommand command2 = new SqlCommand("AddGuildMoney", connection2);
                    command2.CommandType = System.Data.CommandType.StoredProcedure;
                    command2.Transaction = transaction2;
                    SqlParameter parameter3 = command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                    SqlParameter parameter4 = command2.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                    parameter3.Value = _guildID;
                    parameter4.Value = _money;
                    command2.ExecuteNonQuery();
                    transaction2.Commit();
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 성의 돈이 부족합니다.");
                transaction2.Rollback("CASLTE_GUILD_MONEY_GIVE_APP");
                transaction.Rollback("CASLTE_GUILD_MONEY_GIVE_APP");
                fAIL = REPLY_RESULT.FAIL;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_GUILD_MONEY_GIVE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_GUILD_MONEY_GIVE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _castleID, _guildID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                fAIL = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_GUILD_MONEY_GIVE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_GUILD_MONEY_GIVE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                fAIL = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return fAIL;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(CastleList), _argument);
        }

        public CastleList ReadList()
        {
            CastleList list2;
            WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand selectCommand = new SqlCommand("SelectCastleList", connection);
                    selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 명령을 실행합니다");
                    SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                    adapter.TableMappings.Add("Table", "castleBid");
                    adapter.TableMappings.Add("Table1", "castleBidder");
                    adapter.TableMappings.Add("Table2", "castle");
                    adapter.TableMappings.Add("Table3", "castleBuildResource");
                    adapter.TableMappings.Add("Table4", "castleBlock");
                    DataSet dataSet = new DataSet();
                    WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : DataSet 에 성 정보를 채웁니다");
                    adapter.Fill(dataSet);
                    adapter.Dispose();
                    list2 = CastleListObjectBuilder.Build(dataSet);
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    list2 = null;
                }
                finally
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                list2 = null;
            }
            return list2;
        }

        public REPLY_RESULT TakeGuildMoney(long _castleID, long _guildID, int _money, ref int _remainMoney, GuildAdapter _guildAdapter)
        {
            REPLY_RESULT eRROR;
            WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _guildAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CASLTE_GUILD_MONEY_TAKE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CASLTE_GUILD_MONEY_TAKE_APP");
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 성의 돈을 업데이트 합니다.");
                SqlCommand command = new SqlCommand("AddCastleMoney", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Transaction = transaction;
                SqlParameter parameter = command.Parameters.Add("@idCastle", SqlDbType.BigInt, 0x40);
                SqlParameter parameter2 = command.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter.Value = _castleID;
                parameter2.Value = _money;
                command.ExecuteNonQuery();
                SqlCommand command2 = new SqlCommand("WithdrawGuildMoney", connection2);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.Transaction = transaction2;
                SqlParameter parameter3 = command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                SqlParameter parameter4 = command2.Parameters.Add("@intMoney", SqlDbType.Int, 0x20);
                parameter3.Value = _guildID;
                parameter4.Value = _money;
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 길드 머니를 업데이트 합니다.");
                int num = (int) command2.ExecuteScalar();
                if (num >= 0)
                {
                    transaction2.Commit();
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 길드 머니가 부족합니다.");
                transaction2.Rollback("CASLTE_GUILD_MONEY_TAKE_APP");
                transaction.Rollback("CASLTE_GUILD_MONEY_TAKE_APP");
                _remainMoney = num + _money;
                eRROR = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_GUILD_MONEY_TAKE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_GUILD_MONEY_TAKE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _castleID, _guildID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                eRROR = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_GUILD_MONEY_TAKE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_GUILD_MONEY_TAKE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _castleID, _guildID);
                WorkSession.WriteStatus(exception2.Message);
                eRROR = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return eRROR;
        }

        public REPLY_RESULT UpdateBidder(long _castleID, long _guildID, int _bidPrice, int _bidDiffPrice, int _bidOrder, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            REPLY_RESULT eRROR;
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            SqlConnection connection2 = new SqlConnection(((SqlAdapter) _guildAdapter).ConnectionString);
            SqlTransaction transaction2 = null;
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("CASLTE_BIDDER_UPDATE_APP");
                connection2.Open();
                transaction2 = connection2.BeginTransaction("CASLTE_BIDDER_UPDATE_APP");
                SqlCommand command = new SqlCommand(string.Concat(new object[] { "exec UpdateCastleBidder @idCastle=", _castleID, ",@idGuild=", _guildID, ",@bidPrice=", _bidPrice, ",@bidOrder=", _bidOrder }), connection);
                command.Transaction = transaction;
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 입찰자를 업데이트합니다.");
                if (command.ExecuteNonQuery() < 1)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드가 이미 성을 소유하고 있거나, 입찰하지 않았습니다.");
                    ExceptionMonitor.ExceptionRaised(new Exception("길드 [" + _guildID.ToString() + "] 가 이미 성을 소유하고 있거나, 입찰하지 않았습니다."));
                    transaction2.Rollback("CASLTE_BIDDER_UPDATE_APP");
                    transaction.Rollback("CASLTE_BIDDER_UPDATE_APP");
                    return REPLY_RESULT.FAIL;
                }
                string cmdText = (_bidDiffPrice > 0) ? "WithdrawGuildMoney" : "AddGuildMoney";
                SqlCommand command2 = new SqlCommand(cmdText, connection2);
                command2.CommandType = System.Data.CommandType.StoredProcedure;
                command2.Transaction = transaction2;
                SqlParameter parameter = command2.Parameters.Add("@idGuild", SqlDbType.BigInt, 0x40);
                SqlParameter parameter2 = command2.Parameters.Add("@money", SqlDbType.Int, 0x20);
                parameter.Value = _guildID;
                parameter2.Value = _bidDiffPrice;
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드 머니를 업데이트 합니다.");
                _remainMoney = (int) command2.ExecuteScalar();
                if (_remainMoney >= 0)
                {
                    transaction2.Commit();
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드 머니가 부족합니다.");
                transaction2.Rollback("CASLTE_BIDDER_UPDATE_APP");
                transaction.Rollback("CASLTE_BIDDER_UPDATE_APP");
                eRROR = REPLY_RESULT.FAIL_EX;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BIDDER_UPDATE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BIDDER_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception, _castleID, _guildID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                eRROR = REPLY_RESULT.ERROR;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 트랜잭션을 롤백합니다");
                if (transaction2 != null)
                {
                    transaction2.Rollback("CASLTE_BIDDER_UPDATE_APP");
                }
                if (transaction != null)
                {
                    transaction.Rollback("CASLTE_BIDDER_UPDATE_APP");
                }
                ExceptionMonitor.ExceptionRaised(exception2, _castleID, _guildID);
                WorkSession.WriteStatus(exception2.Message);
                eRROR = REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 연결을 종료합니다");
                connection.Close();
                connection2.Close();
            }
            return eRROR;
        }

        public bool UpdateBlock(long _castleID, CastleBlock[] _added, CastleBlock[] _deleted)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                StringBuilder builder = new StringBuilder(null, 100);
                if (_added != null)
                {
                    foreach (CastleBlock block in _added)
                    {
                        builder.AppendFormat("insert into castle_block (castleID, gameName, flag, entry) values({0}, {1}, {2}, {3})\n", new object[] { _castleID, UpdateUtility.BuildString(block.gameName), block.flag, block.entry });
                    }
                }
                if (_deleted != null)
                {
                    foreach (CastleBlock block2 in _deleted)
                    {
                        builder.AppendFormat("delete castle_block where castleID={0} and gameName={1} and flag={2} and entry={3}\n", new object[] { _castleID, UpdateUtility.BuildString(block2.gameName), block2.flag, block2.entry });
                    }
                }
                if (builder.ToString() != string.Empty)
                {
                    try
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 데이터베이스와 연결합니다");
                        connection.Open();
                        SqlCommand command = new SqlCommand(builder.ToString(), connection);
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 명령을 실행합니다");
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException exception)
                    {
                        ExceptionMonitor.ExceptionRaised(exception, _castleID, _added, _deleted);
                        WorkSession.WriteStatus(exception.Message, exception.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 연결을 종료합니다");
                        connection.Close();
                    }
                }
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 생성된 명령이 없어 종료합니다.");
                return true;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _castleID, _added, _deleted);
                WorkSession.WriteStatus(exception2.Message);
                return false;
            }
        }

        public bool UpdateBuild(long _castleID, CastleBuild _build)
        {
            bool flag;
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                SqlTransaction transaction = null;
                try
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 데이터베이스와 연결합니다");
                    connection.Open();
                    transaction = connection.BeginTransaction("CASTLE_BUILD_UPDATE_APP");
                    object obj2 = string.Concat(new object[] { "exec UpdateCastleBuild  @idCastle=", _castleID, ",@durability=", _build.durability, ",@maxDurability=", _build.maxDurability, ",@buildState=", _build.buildState, ",@buildNextTime=", UpdateUtility.BuildDateTime(_build.buildNextTime), ",@buildStep=", _build.buildStep });
                    string cmdText = string.Concat(new object[] { obj2, "delete castle_buildresource where castleID=", _castleID, "\n" });
                    if (_build.resource != null)
                    {
                        foreach (CastleBuildResource resource in _build.resource)
                        {
                            obj2 = cmdText;
                            cmdText = string.Concat(new object[] { obj2, "insert into castle_buildresource(castleID, classID, curAmount, maxAmount) values(", _castleID, ",", resource.classID, ",", resource.curAmount, ",", resource.maxAmount, ")\n" });
                        }
                    }
                    SqlCommand command = new SqlCommand(cmdText, connection);
                    command.Transaction = transaction;
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 성 건설 상태를 업데이트 합니다.");
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("CASTLE_BUILD_UPDATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception, _castleID, _build);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                catch (Exception exception2)
                {
                    if (transaction != null)
                    {
                        transaction.Rollback("CASTLE_BUILD_UPDATE_APP");
                    }
                    ExceptionMonitor.ExceptionRaised(exception2, _castleID, _build);
                    WorkSession.WriteStatus(exception2.Message);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception3)
            {
                ExceptionMonitor.ExceptionRaised(exception3, _castleID, _build);
                WorkSession.WriteStatus(exception3.Message);
                flag = false;
            }
            return flag;
        }

        public bool UpdateBuildResource(long _castleID, CastleBuildResource _resource)
        {
            bool flag;
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand("UpdateCastleBuildResource", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter parameter = command.Parameters.Add("@idCastle", SqlDbType.BigInt, 8);
                    SqlParameter parameter2 = command.Parameters.Add("@classID", SqlDbType.Int, 4);
                    SqlParameter parameter3 = command.Parameters.Add("@curAmount", SqlDbType.Int, 4);
                    SqlParameter parameter4 = command.Parameters.Add("@maxAmount", SqlDbType.Int, 4);
                    parameter.Value = _castleID;
                    parameter2.Value = _resource.classID;
                    parameter3.Value = _resource.curAmount;
                    parameter4.Value = _resource.maxAmount;
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 리소스를 업데이트 합니다.");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _castleID, _resource);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _castleID, _resource);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }

        public bool Write(Castle _castle)
        {
            bool flag;
            WorkSession.WriteStatus("CastleSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                SqlConnection connection = new SqlConnection(base.ConnectionString);
                try
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.Write() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand command = new SqlCommand(string.Concat(new object[] { 
                        "exec UpdateCastle  @idCastle=", _castle.castleID, ",@idGuild=", _castle.guildID, ",@constructed=", _castle.constructed, ",@castleMoney=", _castle.castleMoney, ",@weeklyIncome=", _castle.weeklyIncome, ",@taxrate=", _castle.taxrate, ",@updateTime=", UpdateUtility.BuildDateTime(_castle.updateTime), ",@flag=", _castle.flag, 
                        ",@sellDungeonPass=", _castle.sellDungeonPass, ",@dungeonPassPrice=", _castle.dungeonPassPrice
                     }), connection);
                    WorkSession.WriteStatus("CastleSqlAdapter.Write() : 성 정보를 업데이트 합니다.");
                    command.ExecuteNonQuery();
                    flag = true;
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, _castle);
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    flag = false;
                }
                finally
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.Write() : 연결을 종료합니다");
                    connection.Close();
                }
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _castle);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            return flag;
        }
    }
}

