namespace XMLDB3
{
    using System;

    public class ChronicleEmptyAdapter : ChronicleAdapter
    {
        public bool Create(string _characterName, Chronicle _chronicle)
        {
            return false;
        }

        public void Initialize(string _argument)
        {
        }

        public bool LoadChronicleKey(string _serverName, int[] _questIDs)
        {
            return false;
        }

        public bool UpdateChronicleInfoList(ChronicleInfoList _list)
        {
            return false;
        }
    }
}

