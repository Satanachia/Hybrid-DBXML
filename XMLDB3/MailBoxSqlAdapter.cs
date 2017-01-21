namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.InteropServices;

    public class MailBoxSqlAdapter : SqlAdapter, MailBoxAdapter
    {
        private static readonly string[] mailDeleteProc = new string[] { null, "MailDeleteLarge", "MailDeleteSmall", "MailDeleteHuge", "MailDeleteQuest", null };
        private static readonly string[] mailSendProc = new string[] { null, "MailSendLarge", "MailSendSmall", "MailSendHuge", "MailSendQuest", null };

        private MailBox BuildMailBox(DataTable _boxTable, DataTable _itemTable)
        {
            ArrayList list = new ArrayList();
            if ((_boxTable == null) || (_itemTable == null))
            {
                return null;
            }
            Hashtable hashtable = new Hashtable();
            if ((_itemTable.Rows != null) && (_itemTable.Rows.Count > 0))
            {
                foreach (DataRow row in _itemTable.Rows)
                {
                    Item charItem = ItemSqlBuilder.GetCharItem((Item.StoredType) row["storedtype"], row);
                    if (charItem != null)
                    {
                        hashtable.Add(charItem.id, charItem);
                    }
                    else
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("아이템을 빌드하는데 실패했습니다"), (long) row["itemid"]);
                    }
                }
            }
            if ((_boxTable.Rows != null) && (_boxTable.Rows.Count > 0))
            {
                foreach (DataRow row2 in _boxTable.Rows)
                {
                    MailItem item2 = new MailItem();
                    item2.postID = (long) row2["postID"];
                    item2.receiverCharID = (long) row2["receiverCharID"];
                    item2.receiverCharName = (string) row2["receiverCharName"];
                    item2.senderCharID = (long) row2["senderCharID"];
                    item2.senderCharName = (string) row2["senderCharName"];
                    item2.itemCharge = (int) row2["itemCharge"];
                    item2.senderMsg = (string) row2["senderMsg"];
                    item2.sendDate = (DateTime) row2["sendDate"];
                    item2.postType = (byte) row2["postType"];
                    item2.location = (string) row2["location"];
                    item2.status = (byte) row2["status"];
                    long key = (long) row2["itemID"];
                    if (key != 0L)
                    {
                        if (!hashtable.Contains(key))
                        {
                            throw new Exception("메일에 아이템이 존재하지 않습니다. PostID:" + item2.postID);
                        }
                        item2.item = (Item) hashtable[key];
                    }
                    else
                    {
                        item2.item = null;
                    }
                    list.Add(item2);
                }
            }
            MailBox box = new MailBox();
            box.mailItem = (MailItem[]) list.ToArray(typeof(MailItem));
            return box;
        }

        public long CheckCharacterName(string _name, ref string _outname, ref byte _errorCode)
        {
            long num2;
            WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                long num;
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 프로시져 명령 객체를 작성합니다");
                SqlCommand selectCommand = new SqlCommand("CheckCharacterName", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.Add("@strCharacterName", SqlDbType.VarChar, 50).Value = _name;
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : DataSet 을 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                if (((dataSet.Tables != null) && (dataSet.Tables["Table"] != null)) && ((dataSet.Tables["Table"].Rows != null) && (dataSet.Tables["Table"].Rows.Count > 0)))
                {
                    if (((byte) dataSet.Tables["Table"].Rows[0]["flag"]) == 1)
                    {
                        num = (long) dataSet.Tables["Table"].Rows[0]["id"];
                        _outname = (string) dataSet.Tables["Table"].Rows[0]["name"];
                    }
                    else
                    {
                        num = 0L;
                        _errorCode = 5;
                    }
                }
                else
                {
                    num = 0L;
                    _errorCode = 4;
                }
                dataSet.Dispose();
                num2 = num;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _name);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _errorCode = 0;
                num2 = 0L;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                _errorCode = 0;
                num2 = 0L;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 연결을 종료합니다");
                connection.Close();
            }
            return num2;
        }

        private bool CheckMailBox(long _charID, ref byte _errorCode)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("MailBoxCheckQuota", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idCharacter", SqlDbType.BigInt).Value = _charID;
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                _errorCode = (num == 0) ? ((byte) 0) : ((byte) 1);
                return (num == 0);
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                _errorCode = 0;
                return false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _charID);
                WorkSession.WriteStatus(exception2.Message, _charID);
                _errorCode = 0;
                return false;
            }
        }

        public bool DeleteMail(long _postID, long _itemID, byte _storedType, long _receiverCharID, long _senderCharID, ref byte _errorCode)
        {
            bool flag;
            WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 함수에 진입하였습니다");
            _errorCode = 0;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                string str;
                WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("MAIL_DELETE_APP");
                WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 프로시져 명령 객체를 작성합니다");
                if (_itemID == 0L)
                {
                    str = "dbo.MailDelete";
                }
                else
                {
                    str = mailDeleteProc[_storedType];
                }
                SqlCommand command = new SqlCommand(str, connection, transaction);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idPost", SqlDbType.BigInt).Value = _postID;
                command.Parameters.Add("@itemID", SqlDbType.BigInt).Value = _itemID;
                WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 명령을 실행합니다");
                int num = (int) command.ExecuteScalar();
                transaction.Commit();
                if (num < 1)
                {
                    _errorCode = 3;
                }
                flag = num > 0;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _postID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("MAIL_DELETE_APP");
                }
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                if (transaction != null)
                {
                    transaction.Rollback("MAIL_DELETE_APP");
                }
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public bool GetUnreadCount(long _receiverID, out int _unreadCount)
        {
            bool flag;
            WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 함수에 진입하였습니다");
            _unreadCount = 0;
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("dbo.MailGetUnreadCount", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@receiverID", SqlDbType.BigInt).Value = _receiverID;
                WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 명령을 실행합니다");
                _unreadCount = (int) command.ExecuteScalar();
                flag = true;
            }
            catch (SqlException exception)
            {
                WorkSession.WriteStatus(exception.Message);
                ExceptionMonitor.ExceptionRaised(exception);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(MailBox), _argument);
        }

        public bool ReadMail(long _charID, MailBox _recvBox, MailBox _sendBox)
        {
            bool flag;
            WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 프로시져 명령 객체를 작성합니다");
                SqlCommand selectCommand = new SqlCommand("dbo.MailBoxSelect", connection);
                selectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                selectCommand.Parameters.Add("@idCharacter", SqlDbType.BigInt).Value = _charID;
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand);
                adapter.TableMappings.Add("Table", "recvBox");
                adapter.TableMappings.Add("Table1", "recvItem");
                adapter.TableMappings.Add("Table2", "sendBox");
                adapter.TableMappings.Add("Table3", "sendItem");
                DataSet dataSet = new DataSet();
                WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : DataSet 에 우편함 정보를 채웁니다");
                adapter.Fill(dataSet);
                adapter.Dispose();
                MailBox box = this.BuildMailBox(dataSet.Tables["recvBox"], dataSet.Tables["recvItem"]);
                if (box == null)
                {
                    return false;
                }
                _recvBox.mailItem = box.mailItem;
                box = this.BuildMailBox(dataSet.Tables["sendBox"], dataSet.Tables["sendItem"]);
                if (box == null)
                {
                    return false;
                }
                _sendBox.mailItem = box.mailItem;
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _charID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        public long SendMail(MailItem _mail, ref byte _errorCode)
        {
            long num2;
            WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 함수에 진입하였습니다");
            if (((_mail.postType == 1) || (_mail.postType == 2)) && !this.CheckMailBox(_mail.receiverCharID, ref _errorCode))
            {
                return 0L;
            }
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            SqlTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 데이터베이스와 연결합니다");
                connection.Open();
                transaction = connection.BeginTransaction("MAIL_SEND_APP");
                WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 프로시져 명령 객체를 작성합니다");
                string cmdText = string.Empty;
                if (_mail.item == null)
                {
                    cmdText = string.Concat(new object[] { 
                        "exec dbo.MailSend  @receiverCharID=", _mail.receiverCharID, ",@receiverCharName=", UpdateUtility.BuildString(_mail.receiverCharName), ",@senderCharID=", _mail.senderCharID, ",@senderCharName=", UpdateUtility.BuildString(_mail.senderCharName), ",@itemID=0,@itemCharge=", _mail.itemCharge, ",@senderMsg=", UpdateUtility.BuildString(_mail.senderMsg), ",@sendDate=", UpdateUtility.BuildDateTime(_mail.sendDate), ",@postType=", _mail.postType, 
                        ",@location=", UpdateUtility.BuildString(_mail.location), ",@status=", _mail.status
                     });
                }
                else
                {
                    cmdText = string.Concat(new object[] { 
                        "exec dbo.", mailSendProc[_mail.item.storedtype], " @receiverCharID=", _mail.receiverCharID, ",@receiverCharName=", UpdateUtility.BuildString(_mail.receiverCharName), ",@senderCharID=", _mail.senderCharID, ",@senderCharName=", UpdateUtility.BuildString(_mail.senderCharName), ",@itemID=", _mail.item.id, ",@itemCharge=", _mail.itemCharge, ",@senderMsg=", UpdateUtility.BuildString(_mail.senderMsg), 
                        ",@sendDate=", UpdateUtility.BuildDateTime(_mail.sendDate), ",@postType=", _mail.postType, ",@location=", UpdateUtility.BuildString(_mail.location), ",@status=", _mail.status, ",@storedType=", _mail.item.storedtype, ",@class=", _mail.item.@class, ",@color_01=", _mail.item.color_01, ",@color_02=", _mail.item.color_02, 
                        ",@color_03=", _mail.item.color_03, ",@price=", _mail.item.price, ",@bundle=", _mail.item.bundle, ",@linked_pocket=", _mail.item.linked_pocket, ",@figure=", _mail.item.figure, ",@flag=", _mail.item.flag, ",@durability=", _mail.item.durability, ",@durability_max=", _mail.item.durability_max, 
                        ",@origin_durability_max=", _mail.item.origin_durability_max, ",@attack_min=", _mail.item.attack_min, ",@attack_max=", _mail.item.attack_max, ",@wattack_min=", _mail.item.wattack_min, ",@wattack_max=", _mail.item.wattack_max, ",@balance=", _mail.item.balance, ",@critical=", _mail.item.critical, ",@defence=", _mail.item.defence, 
                        ",@protect=", _mail.item.protect, ",@effective_range=", _mail.item.effective_range, ",@attack_speed=", _mail.item.attack_speed, ",@down_hit_count=", _mail.item.down_hit_count, ",@experience=", _mail.item.experience, ",@exp_point=", _mail.item.exp_point, ",@upgraded=", _mail.item.upgraded, ",@upgrade_max=", _mail.item.upgrade_max, 
                        ",@grade=", _mail.item.grade, ",@prefix=", _mail.item.prefix, ",@suffix=", _mail.item.suffix, ",@data=", UpdateUtility.BuildString(_mail.item.data), ",@option=", UpdateUtility.BuildString(ItemXmlFieldHelper.BuildItemOptionXml(_mail.item.options)), ",@sellingprice=", _mail.item.sellingprice, ",@expiration=", _mail.item.expiration
                     });
                }
                SqlCommand command = new SqlCommand(cmdText, connection, transaction);
                command.CommandType = System.Data.CommandType.Text;
                WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 명령을 실행합니다");
                long num = (long) command.ExecuteScalar();
                transaction.Commit();
                num2 = num;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _mail.senderCharName);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                if (transaction != null)
                {
                    transaction.Rollback("MAIL_SEND_APP");
                }
                num2 = 0L;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                if (transaction != null)
                {
                    transaction.Rollback("MAIL_SEND_APP");
                }
                num2 = 0L;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 연결을 종료합니다");
                connection.Close();
            }
            return num2;
        }

        public bool UpdateMail(long _postID, byte _status, long _receiverCharID, long _senderCharID)
        {
            bool flag;
            WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 데이터베이스와 연결합니다");
                connection.Open();
                WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 프로시져 명령 객체를 작성합니다");
                SqlCommand command = new SqlCommand("MailUpdate", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@idPost", SqlDbType.BigInt).Value = _postID;
                command.Parameters.Add("@status", SqlDbType.TinyInt).Value = _status;
                WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 명령을 실행합니다");
                command.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _postID);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message, _postID);
                ExceptionMonitor.ExceptionRaised(exception2);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }
    }
}

