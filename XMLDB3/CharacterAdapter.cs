namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public interface CharacterAdapter
    {
        bool CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo _character, AccountRefAdapter _accountref, BankAdapter _bank, WebSynchAdapter _websynch);
        bool DeleteEx(string _account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string _server, long _idcharacter, AccountRefAdapter _accountref, BankAdapter _bank, GuildAdapter _guild, WebSynchAdapter _websynch);
        bool DeleteItem(long _id, ItemList[] _list);
        uint GetAccumLevel(string _account, string _name);
        bool GetWriteCounter(long _id, out byte _counter);
        void Initialize(string _argument);
        bool IsUsableName(string _name, string _account);
        CharacterInfo Read(long _id, CharacterInfo _cache);
        bool RemoveReservedCharName(string _name, string _account);
        bool Write(CharacterInfo _character, CharacterInfo _cache);
    }
}

