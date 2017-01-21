namespace XMLDB3
{
    using System;

    public interface WorldMetaAdapter
    {
        void Initialize(string _argument);
        WorldMetaList Read();
        REPLY_RESULT UpdateList(WorldMetaList _worldmeteUpdateList, string[] _removeKeys, ref byte _errorCode);
    }
}

