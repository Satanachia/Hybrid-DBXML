namespace XMLDB3
{
    using System;

    public interface AccountAdapter
    {
        bool Ban(string _account, short _bantype, string _manager, short _duration, string _purpose);
        bool Create(Account _data);
        void Initialize(string _argument);
        bool LoginSignal(string _account, long _sessionKey, string _address, int _ispCode);
        bool LogoutSignal(string _account);
        Account Read(string _id);
        Account ReadSMS(string _id);
        bool Unban(string _account, string _manager);
    }
}

