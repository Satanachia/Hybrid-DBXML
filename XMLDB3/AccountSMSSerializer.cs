namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountSMSSerializer
    {
        public static void Deserialize(Account _account, Message _message)
        {
            if (_account.SMSAuth == null)
            {
                _account.SMSAuth = new AccountSMSAuth();
                _account.SMSAuth.loginType = 0;
                _account.SMSAuth.cPhone = string.Empty;
                _account.SMSAuth.carrier = string.Empty;
                _account.SMSAuth.lastIP = string.Empty;
            }
            AccountSerializer.Deserialize(_account, _message);
            _message.WriteU8(_account.SMSAuth.loginType);
            _message.WriteString(_account.SMSAuth.cPhone);
            _message.WriteString(_account.SMSAuth.carrier);
            _message.WriteString(_account.SMSAuth.lastIP);
        }

        public static Account Serialize(Message _message)
        {
            Account account = AccountSerializer.Serialize(_message);
            account.SMSAuth = new AccountSMSAuth();
            account.SMSAuth.loginType = _message.ReadU8();
            account.SMSAuth.cPhone = _message.ReadString();
            account.SMSAuth.carrier = _message.ReadString();
            account.SMSAuth.lastIP = _message.ReadString();
            return account;
        }
    }
}

