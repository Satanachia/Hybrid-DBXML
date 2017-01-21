namespace XMLDB3
{
    using System;

    public class LoginIdPoolFileAdapter : FileAdapter, LoginIdPoolAdapter
    {
        public long GetIdPool(int _size)
        {
            string str = "LoginIdPool";
            long count = 0L;
            if (base.IsExistData(str))
            {
                LoginIDPool pool = (LoginIDPool) base.ReadFromDB(str);
                if (pool != null)
                {
                    count = pool.count;
                    pool.count = count + _size;
                    base.WriteToDB(pool, str);
                    return count;
                }
            }
            LoginIDPool pool2 = new LoginIDPool();
            pool2.count = count + _size;
            base.WriteToDB(pool2, str);
            return count;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(LoginIDPool), ConfigManager.GetFileDBPath("loginidpool"), ".xml");
        }
    }
}

