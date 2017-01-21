namespace XMLDB3
{
    using System;

    public class ChronicleFileAdapter : FileAdapter, ChronicleAdapter
    {
        private void AddChronicle(ChronicleList _list, Chronicle _added)
        {
            Chronicle[] chronicleArray;
            if ((_list.chronicles == null) || (_list.chronicles.Length == 0))
            {
                chronicleArray = new Chronicle[1];
            }
            else
            {
                chronicleArray = new Chronicle[_list.chronicles.Length + 1];
                _list.chronicles.CopyTo(chronicleArray, 0);
            }
            chronicleArray[_list.chronicles.Length] = _added;
            _list.chronicles = chronicleArray;
        }

        public bool Create(string _characterName, Chronicle _chronicle)
        {
            string chronicleID = this.GetChronicleID(_chronicle.serverName, _chronicle.charID);
            if (!base.IsExistData(chronicleID))
            {
                ChronicleList list = new ChronicleList();
                list.chronicles = new Chronicle[] { _chronicle };
                base.WriteToDB(list, chronicleID);
                return true;
            }
            ChronicleList list2 = (ChronicleList) base.ReadFromDB(chronicleID);
            if (list2 == null)
            {
                return false;
            }
            this.AddChronicle(list2, _chronicle);
            base.WriteToDB(list2, chronicleID);
            return true;
        }

        private string GetChronicleID(string _serverName, long _charID)
        {
            return (_serverName + "@" + _charID.ToString());
        }

        public void Initialize(string _Argument)
        {
            base.Initialize(typeof(ChronicleList), ConfigManager.GetFileDBPath("chronicle"), ".xml");
        }

        public bool UpdateChronicleInfoList(ChronicleInfoList _list)
        {
            return true;
        }
    }
}

