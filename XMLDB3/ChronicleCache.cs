namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class ChronicleCache
    {
        private string serverName;
        private Hashtable table;

        public ChronicleCache(string _serverName)
        {
            this.serverName = _serverName;
            this.table = new Hashtable();
        }

        public ChronicleCache(string _serverName, IDictionary _dic)
        {
            this.serverName = _serverName;
            this.table = new Hashtable(_dic);
        }

        public bool Exists(int _queryID)
        {
            return this.table.Contains(_queryID);
        }

        public int GetNextCount(string _serverName, int _queryID, out DateTime _createTime)
        {
            if (_serverName != this.serverName)
            {
                throw new Exception("서버이름 불일치:" + this.serverName + ":" + _serverName);
            }
            if (!this.table.Contains(_queryID))
            {
                throw new Exception("없는 키:" + _queryID.ToString());
            }
            lock (this)
            {
                int num = (int) this.table[_queryID];
                this.table[_queryID] = ++num;
                _createTime = DateTime.Now;
                return num;
            }
        }

        public DateTime GetNextTime()
        {
            lock (this)
            {
                return DateTime.Now;
            }
        }

        public void InsertKey(int _questID, int _count)
        {
            this.table.Add(_questID, _count);
        }
    }
}

