namespace XMLDB3
{
    using System;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class SqlAdapter
    {
        private XmlSerializer serializer;
        private string strConnection;

        private object _Read(SqlCommand _readCommand)
        {
            WorkSession.WriteStatus("SqlAdapter.Read() : 명령을 실행합니다");
            XmlReader xmlReader = _readCommand.ExecuteXmlReader();
            object obj2 = null;
            XmlSerializer serializer = this.GetSerializer();
            WorkSession.WriteStatus("SqlAdapter.Read() : XML 디시리얼라이즈 가능 여부를 체크합니다");
            if (serializer.CanDeserialize(xmlReader))
            {
                WorkSession.WriteStatus("SqlAdapter.Read() : XML 로부터 오브젝트를 생성합니다");
                obj2 = serializer.Deserialize(xmlReader);
            }
            else
            {
                WorkSession.WriteStatus("SqlAdapter.Read() : XML 디시리얼라이즈가 가능하지 않습니다");
            }
            xmlReader.Close();
            return obj2;
        }

        public bool Create(object _data)
        {
            WorkSession.WriteStatus("SqlAdapter.Create() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("SqlAdapter.Create() : 오브젝트를 XML 문자열로 변경합니다");
                MemoryStream stream = new MemoryStream();
                TextWriter textWriter = new StreamWriter(stream, Encoding.Unicode);
                this.GetSerializer().Serialize(textWriter, _data);
                stream.Position = 0L;
                TextReader reader = new StreamReader(stream, Encoding.Unicode);
                SqlConnection connection = new SqlConnection(this.strConnection);
                try
                {
                    WorkSession.WriteStatus("SqlAdapter.Create() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand createProcedure = this.GetCreateProcedure(reader.ReadToEnd(), connection);
                    WorkSession.WriteStatus("SqlAdapter.Create() : 명령을 실행합니다");
                    createProcedure.ExecuteNonQuery();
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, reader.ReadToEnd());
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("SqlAdapter.Create() : 연결을 종료합니다");
                    connection.Close();
                }
                return true;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                return false;
            }
        }

        public bool Delete(object _id)
        {
            bool flag;
            WorkSession.WriteStatus("SqlAdapter.Delete() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(this.strConnection);
            try
            {
                WorkSession.WriteStatus("SqlAdapter.Delete() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand deleteProcedure = this.GetDeleteProcedure(_id, connection);
                WorkSession.WriteStatus("SqlAdapter.Delete() : 명령을 실행합니다");
                deleteProcedure.ExecuteNonQuery();
                flag = true;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _id);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                flag = false;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                flag = false;
            }
            finally
            {
                WorkSession.WriteStatus("SqlAdapter.Delete() : 연결을 종료합니다");
                connection.Close();
            }
            return flag;
        }

        protected virtual SqlCommand GetCreateProcedure(object _Argument, SqlConnection _Con)
        {
            return null;
        }

        protected virtual SqlCommand GetDeleteProcedure(object _Argument, SqlConnection _Con)
        {
            return null;
        }

        protected virtual SqlCommand GetInsertProcedure(object _Argument, SqlConnection _Con)
        {
            return null;
        }

        public object GetMax()
        {
            object obj3;
            WorkSession.WriteStatus("SqlAdapter.GetMax() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(this.strConnection);
            try
            {
                WorkSession.WriteStatus("SqlAdapter.GetMax() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand maxProcedure = this.GetMaxProcedure(null, connection);
                WorkSession.WriteStatus("SqlAdapter.GetMax() : 명령을 실행합니다");
                XmlReader xmlReader = maxProcedure.ExecuteXmlReader();
                WorkSession.WriteStatus("SqlAdapter.GetMax() : XML 디시리얼라이즈 가능 여부를 체크합니다");
                object obj2 = null;
                XmlSerializer serializer = this.GetSerializer();
                if (serializer.CanDeserialize(xmlReader))
                {
                    WorkSession.WriteStatus("SqlAdapter.GetMax() : XML 로부터 오브젝트를 생성합니다");
                    obj2 = serializer.Deserialize(xmlReader);
                }
                else
                {
                    WorkSession.WriteStatus("SqlAdapter.Read() : XML 디시리얼라이즈가 가능하지 않습니다");
                }
                xmlReader.Close();
                obj3 = obj2;
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                obj3 = null;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                obj3 = null;
            }
            finally
            {
                WorkSession.WriteStatus("SqlAdapter.GetMax() : 연결을 종료합니다");
                connection.Close();
            }
            return obj3;
        }

        protected virtual SqlCommand GetMaxProcedure(object _Argument, SqlConnection _Con)
        {
            return null;
        }

        protected virtual SqlCommand GetSelectProcedure(object _Argument, SqlConnection _Con)
        {
            return null;
        }

        protected virtual XmlSerializer GetSerializer()
        {
            return this.serializer;
        }

        public virtual void Initialize(Type _type, string _strConnection)
        {
            this.serializer = new XmlSerializer(_type);
            this.strConnection = _strConnection;
        }

        public object Read(SqlCommand _readCommand)
        {
            object obj2;
            WorkSession.WriteStatus("SqlAdapter.Read() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(this.strConnection);
            try
            {
                WorkSession.WriteStatus("SqlAdapter.Read() : 데이터베이스와 연결합니다");
                connection.Open();
                _readCommand.Connection = connection;
                obj2 = this._Read(_readCommand);
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _readCommand.CommandText);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                obj2 = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _readCommand.CommandText);
                WorkSession.WriteStatus(exception2.Message);
                obj2 = null;
            }
            finally
            {
                WorkSession.WriteStatus("SqlAdapter.Read() : 연결을 종료합니다");
                connection.Close();
            }
            return obj2;
        }

        public object Read(object _id)
        {
            object obj2;
            WorkSession.WriteStatus("SqlAdapter.Read() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(this.strConnection);
            try
            {
                WorkSession.WriteStatus("SqlAdapter.Read() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand selectProcedure = this.GetSelectProcedure(_id, connection);
                obj2 = this._Read(selectProcedure);
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception, _id);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                obj2 = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2, _id);
                WorkSession.WriteStatus(exception2.Message);
                obj2 = null;
            }
            finally
            {
                WorkSession.WriteStatus("SqlAdapter.Read() : 연결을 종료합니다");
                connection.Close();
            }
            return obj2;
        }

        public bool Write(object _Data)
        {
            WorkSession.WriteStatus("SqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("SqlAdapter.Write() : 오브젝트를 XML 문자열로 변환합니다");
                MemoryStream stream = new MemoryStream();
                TextWriter textWriter = new StreamWriter(stream, Encoding.Unicode);
                this.GetSerializer().Serialize(textWriter, _Data);
                stream.Position = 0L;
                TextReader reader = new StreamReader(stream, Encoding.Unicode);
                string str = reader.ReadToEnd();
                reader.Close();
                textWriter.Close();
                SqlConnection connection = new SqlConnection(this.strConnection);
                try
                {
                    WorkSession.WriteStatus("SqlAdapter.Write() : 데이터베이스와 연결합니다");
                    connection.Open();
                    SqlCommand insertProcedure = this.GetInsertProcedure(str, connection);
                    WorkSession.WriteStatus("SqlAdapter.Write() : 명령을 실행합니다");
                    insertProcedure.ExecuteNonQuery();
                }
                catch (SqlException exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception, reader.ReadToEnd());
                    WorkSession.WriteStatus(exception.Message, exception.Number);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("SqlAdapter.Write() : 연결을 종료합니다");
                    connection.Close();
                }
                return true;
            }
            catch (Exception exception2)
            {
                WorkSession.WriteStatus(exception2.Message);
                ExceptionMonitor.ExceptionRaised(exception2);
                return false;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.strConnection;
            }
        }
    }
}

