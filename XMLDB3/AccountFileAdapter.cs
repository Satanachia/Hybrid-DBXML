namespace XMLDB3
{
    using System;

    public class AccountFileAdapter : FileAdapter, AccountAdapter
    {
        private readonly string SNColumn = "serialnumber";

        public bool Ban(string _account, short _bantype, string _manager, short _duration, string _purpose)
        {
            WorkSession.WriteStatus("AccountFileAdapter.Ban() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountFileAdapter.Ban() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountFileAdapter.Ban() : 데이터 파일 [" + _account + "] 계정을 밴합니다");
                Account account = (Account) base.ReadFromDB(_account);
                account.flag = _bantype;
                account.blocking_date = DateTime.Now;
                account.blocking_duration = _duration;
                base.WriteToDB(account, _account);
                return true;
            }
            WorkSession.WriteStatus("AccountFileAdapter.Ban() : 데이터 파일 [" + _account + "]가 존재하지 않습니다. 밴에 실패하였습니다");
            return false;
        }

        public bool Create(Account _data)
        {
            WorkSession.WriteStatus("AccountFileAdapter.Create() : 함수에 진입하였습니다");
            string id = _data.id;
            WorkSession.WriteStatus("AccountFileAdapter.Create() : 데이터 파일 [" + id + "]가 존재하는지 확인합니다");
            if (!base.IsExistData(id))
            {
                WorkSession.WriteStatus("AccountFileAdapter.Create() : 데이터 파일 [" + id + "]를 생성합니다");
                byte[] inArray = EncryptionManager.Encrypt(this.SNColumn, _data.serialnumber);
                if (inArray != null)
                {
                    _data.eserialnumber = Convert.ToBase64String(inArray, 0, inArray.Length);
                }
                base.WriteToDB(_data, id);
                return true;
            }
            WorkSession.WriteStatus("AccountFileAdapter.Create() : 데이터 파일 [" + id + "]가 존재합니다. 생성에 실패하였습니다");
            return false;
        }

        public void Initialize(string _Argument)
        {
            base.Initialize(typeof(Account), ConfigManager.GetFileDBPath("account"), ".xml");
        }

        public bool LoginSignal(string _account, long _sessionKey, string _address, int _ispCode)
        {
            WorkSession.WriteStatus("AccountFileAdapter.LoginSignal() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountFileAdapter.LoginSignal() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountFileAdapter.LoginSignal() : 데이터 파일 [" + _account + "]를 읽습니다");
                Account account = (Account) base.ReadFromDB(_account);
                if ((account != null) && (account.SMSAuth != null))
                {
                    account.SMSAuth.lastIP = _address;
                    base.WriteToDB(account, _account);
                }
            }
            else
            {
                WorkSession.WriteStatus("AccountFileAdapter.LoginSignal() : 데이터 파일 [" + _account + "]가 존재하지 않습니다. 읽기에 실패하였습니다");
            }
            return true;
        }

        public bool LogoutSignal(string _account)
        {
            WorkSession.WriteStatus("AccountFileAdapter.LogoutSignal() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountFileAdapter.LogoutSignal() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountFileAdapter.LogoutSignal() : 데이터 파일 [" + _account + "]를 읽습니다");
                Account account = (Account) base.ReadFromDB(_account);
                if ((account == null) || (account.SMSAuth == null))
                {
                }
            }
            else
            {
                WorkSession.WriteStatus("AccountFileAdapter.LogoutSignal() : 데이터 파일 [" + _account + "]가 존재하지 않습니다. 읽기에 실패하였습니다");
            }
            return true;
        }

        public Account Read(string _id)
        {
            WorkSession.WriteStatus("AccountFileAdapter.Read() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountFileAdapter.Read() : 데이터 파일 [" + _id + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_id))
            {
                WorkSession.WriteStatus("AccountFileAdapter.Read() : 데이터 파일 [" + _id + "]를 읽습니다");
                Account account = (Account) base.ReadFromDB(_id);
                if ((ConfigManager.IsEncryptionEnabled && (account.eserialnumber != null)) && (account.eserialnumber.Length > 0))
                {
                    string str = EncryptionManager.Decrypt(this.SNColumn, Convert.FromBase64String(account.eserialnumber));
                    bool flag1 = account.serialnumber != str;
                }
                return account;
            }
            WorkSession.WriteStatus("AccountFileAdapter.Read() : 데이터 파일 [" + _id + "]가 존재하지 않습니다. 읽기에 실패하였습니다");
            return null;
        }

        public Account ReadSMS(string _id)
        {
            return this.Read(_id);
        }

        public bool Unban(string _account, string _manager)
        {
            WorkSession.WriteStatus("AccountFileAdapter.Unban() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("AccountFileAdapter.Unban() : 데이터 파일 [" + _account + "]가 존재하는지 확인합니다");
            if (base.IsExistData(_account))
            {
                WorkSession.WriteStatus("AccountFileAdapter.Unban() : 데이터 파일 [" + _account + "] 계정을 밴합니다");
                Account account = (Account) base.ReadFromDB(_account);
                account.flag = -1;
                base.WriteToDB(account, _account);
                return true;
            }
            WorkSession.WriteStatus("AccountFileAdapter.Unban() : 데이터 파일 [" + _account + "]가 존재하지 않습니다. 밴에 실패하였습니다");
            return false;
        }
    }
}

