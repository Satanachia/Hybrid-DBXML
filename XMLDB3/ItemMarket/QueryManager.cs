namespace XMLDB3.ItemMarket
{
    using System;
    using System.Collections;

    public class QueryManager
    {
        private int idx = 0;
        private Hashtable queryTable = new Hashtable();

        public Query PopQuery(int _packetNo)
        {
            Query query = null;
            if (this.queryTable.ContainsKey(_packetNo))
            {
                query = (Query) this.queryTable[_packetNo];
                this.queryTable.Remove(_packetNo);
            }
            return query;
        }

        public int PushQuery(uint _ID, uint _queryID, uint _targetID, int _clientID)
        {
            int key = this.idx++;
            while (this.queryTable.ContainsKey(key))
            {
                key = this.idx++;
            }
            Query query = new Query();
            query.packetNo = key;
            query.ID = _ID;
            query.queryID = _queryID;
            query.targetID = _targetID;
            query.clientID = _clientID;
            this.queryTable[key] = query;
            return key;
        }
    }
}

