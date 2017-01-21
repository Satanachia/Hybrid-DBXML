namespace XMLDB3
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Xml;
    using System.Xml.Serialization;

    public class PropSqlAdapter : SqlAdapter, PropAdapter
    {
        private static XmlSerializer propIDListSerializer = new XmlSerializer(typeof(PropIDList));

        public bool Create(Prop _data)
        {
            return base.Create(_data);
        }

        public bool Delete(long _id)
        {
            return base.Delete(_id);
        }

        protected override SqlCommand GetCreateProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("CreateProp", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlProp", SqlDbType.NText, 0xf4240).Value = _argument;
            return command;
        }

        protected override SqlCommand GetDeleteProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("DeleteProp", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idProp", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        protected override SqlCommand GetInsertProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("InsertProp", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@xmlProp", SqlDbType.NText, 0xf4240).Value = _argument;
            return command;
        }

        protected override SqlCommand GetSelectProcedure(object _argument, SqlConnection _con)
        {
            SqlCommand command = new SqlCommand("SelectProp", _con);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@idProp", SqlDbType.BigInt, 8).Value = _argument;
            return command;
        }

        public void Initialize(string _argument)
        {
            this.Initialize(typeof(Prop), _argument);
        }

        public PropIDList LoadPropList()
        {
            PropIDList list;
            WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 함수에 진입하였습니다");
            SqlConnection connection = new SqlConnection(base.ConnectionString);
            try
            {
                WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 데이터베이스와 연결합니다");
                connection.Open();
                SqlCommand command = new SqlCommand("SelectPropList", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 명령을 실행합니다");
                XmlReader xmlReader = command.ExecuteXmlReader();
                if (xmlReader == null)
                {
                    throw new Exception("쿼리된 데이터가 없습니다");
                }
                try
                {
                    WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 쿼리 결과에서 프랍 리스트를 얻어옵니다");
                    if (propIDListSerializer.CanDeserialize(xmlReader))
                    {
                        return (PropIDList) propIDListSerializer.Deserialize(xmlReader);
                    }
                    WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 쿼리 결과를 프랍 리스트로 변환할 수 없습니다");
                    list = null;
                }
                finally
                {
                    xmlReader.Close();
                }
            }
            catch (SqlException exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                WorkSession.WriteStatus(exception.Message, exception.Number);
                list = null;
            }
            catch (Exception exception2)
            {
                ExceptionMonitor.ExceptionRaised(exception2);
                WorkSession.WriteStatus(exception2.Message);
                list = null;
            }
            finally
            {
                WorkSession.WriteStatus("PropSqlAdapter.LoadPropList() : 연결을 종료합니다");
                connection.Close();
            }
            return list;
        }

        public Prop Read(long _id)
        {
            return (Prop) base.Read(_id);
        }

        public bool Write(Prop _data)
        {
            return base.Write(_data);
        }
    }
}

