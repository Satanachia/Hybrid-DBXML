namespace XMLDB3
{
    using System;

    public class WorldMetaEmptyAdapter : WorldMetaAdapter
    {
        public void Initialize(string _argument)
        {
        }

        public WorldMetaList Read()
        {
            return null;
        }

        public REPLY_RESULT UpdateList(WorldMetaList _worldmeteUpdateList, string[] _removeKeys, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }
    }
}

