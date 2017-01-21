namespace XMLDB3
{
    using System;

    public class AccountActivationFileAdapter : FileAdapter, AccountActivationAdapter
    {
        public bool Create(AccountActivation _data)
        {
            WorkSession.WriteStatus("AccountActivationFileAdapter.Create() : 함수에 진입하였습니다");
            string id = _data.id;
            WorkSession.WriteStatus("AccountActivationFileAdapter.Create() : 데이터 파일 [" + id + "]가 존재하는지 확인합니다");
            if (!base.IsExistData(id))
            {
                WorkSession.WriteStatus("AccountActivationFileAdapter.Create() : 데이터 파일 [" + id + "]를 생성합니다");
                Account account = new Account();
                account.authority = _data.authority;
                account.blocking_date = _data.blocking_date;
                account.email = _data.email;
                account.flag = _data.flag;
                account.id = _data.id;
                account.name = _data.name;
                account.password = _data.password;
                account.serialnumber = _data.serialnumber;
                account.SMSAuth = null;
                base.WriteToDB(account, id);
                return true;
            }
            WorkSession.WriteStatus("AccountActivationFileAdapter.Create() : 데이터 파일 [" + id + "]가 존재합니다. 생성에 실패하였습니다");
            return false;
        }

        public void Initialize(string _Argument)
        {
            base.Initialize(typeof(Account), ConfigManager.GetFileDBPath("account"), ".xml");
        }
    }
}

