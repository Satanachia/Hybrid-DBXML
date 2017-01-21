namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class AccountSerializer
    {
        public static void Deserialize(Account _account, Message _message)
        {
            byte[] buffer = new byte[0x20];
            for (int i = 0; i < _account.password.Length; i++)
            {
                if (i < 0x20)
                {
                    buffer[i] = (byte) _account.password[i];
                }
            }
            _message.WriteString(_account.id);
            _message.WriteBinary(buffer);
            _message.WriteString(_account.name);
            _message.WriteString(_account.serialnumber);
            _message.WriteString(_account.email);
            _message.WriteS16(_account.flag);
            _message.WriteS64(_account.blocking_date.Ticks);
            _message.WriteS16(_account.blocking_duration);
            _message.WriteU8(_account.authority);
            _message.WriteU8(_account.changePassword ? ((byte) 1) : ((byte) 0));
            if (_account.machineids != null)
            {
                _message.WriteString(_account.machineids);
            }
            else
            {
                _message.WriteString("");
            }
        }

        public static Account Serialize(Message _message)
        {
            Account account = new Account();
            account.id = _message.ReadString();
            byte[] buffer = _message.ReadBinary();
            account.name = _message.ReadString();
            account.serialnumber = _message.ReadString();
            account.email = _message.ReadString();
            account.flag = _message.ReadS16();
            account.blocking_date = new DateTime(_message.ReadS64());
            account.blocking_duration = _message.ReadS16();
            account.authority = _message.ReadU8();
            char[] chArray = new char[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                chArray[i] = (char) buffer[i];
            }
            account.password = new string(chArray);
            return account;
        }

        public static AccountActivation SerializeForActivation(Message _message)
        {
            AccountActivation activation = new AccountActivation();
            activation.id = _message.ReadString();
            byte[] buffer = _message.ReadBinary();
            activation.name = _message.ReadString();
            activation.serialnumber = _message.ReadString();
            activation.email = _message.ReadString();
            activation.flag = _message.ReadS16();
            activation.blocking_date = new DateTime(_message.ReadS64());
            activation.blocking_duration = _message.ReadS16();
            activation.authority = _message.ReadU8();
            activation.provider_code = _message.ReadU8();
            char[] chArray = new char[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
            {
                chArray[i] = (char) buffer[i];
            }
            activation.password = new string(chArray);
            return activation;
        }
    }
}

