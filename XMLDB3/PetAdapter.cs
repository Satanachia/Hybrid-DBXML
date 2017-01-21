namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public interface PetAdapter
    {
        bool CreateEx(string _account, string _server, PetInfo _data, AccountRefAdapter _accountref, WebSynchAdapter _websynch);
        bool DeleteEx(string _account, string _server, long _idcharacter, AccountRefAdapter _accountref, WebSynchAdapter _websynch);
        bool DeleteItem(long _id, ItemList[] _list);
        bool GetWriteCounter(long _id, out byte _counter);
        void Initialize(string _argument);
        PetInfo Read(string _account, string _server, long _id, PetInfo _cache, AccountRefAdapter _accountref);
        bool Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref);
    }
}

