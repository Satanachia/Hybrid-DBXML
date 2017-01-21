namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public interface AccountRefAdapter
    {
        bool CacheKeyCheck(string _account, int _cacheKey, out int _oldCacheKey);
        bool Create(Accountref _data);
        void Initialize(string _argument);
        bool PlayIn(string _account, int _remainTime);
        bool PlayOut(string _account, int _remainTime, string _server, GroupIDList _charGroupID, GroupIDList _petGroupID, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, byte _macroCheckFailure, byte _macroCheckSuccess, bool _beginnerFlag);
        Accountref Read(string _id);
        bool SetFlag(string _account, int _flag);
        bool SetLobbyOption(string _account, int _lobbyOption, LobbyTabList _charLobbyTabList, LobbyTabList _petLobbyTabList);
        bool SetPetSlotFlag(string _account, long _id, string _server, long _flag);
        bool SetSlotFlag(string _account, long _id, string _server, long _flag);
    }
}

