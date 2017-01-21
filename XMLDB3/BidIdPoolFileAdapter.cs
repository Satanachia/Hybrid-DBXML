namespace XMLDB3
{
    using System;

    public class BidIdPoolFileAdapter : FileAdapter, BidIdPoolAdapter
    {
        public long GetIdPool()
        {
            string str = "bididpool";
            long count = 0L;
            if (base.IsExistData(str))
            {
                BidIDPool pool = (BidIDPool) base.ReadFromDB(str);
                if (pool != null)
                {
                    count = pool.count;
                    pool.count = count + 0x3e8L;
                    base.WriteToDB(pool, str);
                    return count;
                }
            }
            BidIDPool pool2 = new BidIDPool();
            pool2.count = count + 0x3e8L;
            base.WriteToDB(pool2, str);
            return count;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(BidIDPool), ConfigManager.GetFileDBPath("bididpool"), ".xml");
        }
    }
}

