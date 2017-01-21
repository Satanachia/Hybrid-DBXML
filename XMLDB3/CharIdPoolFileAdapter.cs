namespace XMLDB3
{
    using System;

    public class CharIdPoolFileAdapter : FileAdapter, CharIdPoolAdapter
    {
        public long GetIdPool()
        {
            string str = "charidpool";
            long count = 0L;
            if (base.IsExistData(str))
            {
                CharIDPool pool = (CharIDPool) base.ReadFromDB(str);
                if (pool != null)
                {
                    count = pool.count;
                    pool.count = count + 0x3e8L;
                    base.WriteToDB(pool, str);
                    return count;
                }
            }
            CharIDPool pool2 = new CharIDPool();
            pool2.count = count + 0x3e8L;
            base.WriteToDB(pool2, str);
            return count;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(CharIDPool), ConfigManager.GetFileDBPath("charidpool"), ".xml");
        }
    }
}

