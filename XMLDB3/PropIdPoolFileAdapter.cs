namespace XMLDB3
{
    using System;

    public class PropIdPoolFileAdapter : FileAdapter, PropIdPoolAdapter
    {
        public long GetIdPool()
        {
            string str = "propidpool";
            long count = 0L;
            if (base.IsExistData(str))
            {
                PropIDPool pool = (PropIDPool) base.ReadFromDB(str);
                if (pool != null)
                {
                    count = pool.count;
                    pool.count = count + 0x3e8L;
                    base.WriteToDB(pool, str);
                    return count;
                }
            }
            PropIDPool pool2 = new PropIDPool();
            pool2.count = count + 0x3e8L;
            base.WriteToDB(pool2, str);
            return count;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(PropIDPool), ConfigManager.GetFileDBPath("propidpool"), ".xml");
        }
    }
}

