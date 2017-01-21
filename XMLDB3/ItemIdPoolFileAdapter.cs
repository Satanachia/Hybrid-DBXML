namespace XMLDB3
{
    using System;

    public class ItemIdPoolFileAdapter : FileAdapter, ItemIdPoolAdapter
    {
        public long GetIdPool()
        {
            long num2;
            try
            {
                ItemIdPoolMutex.Enter();
                string str = "itemidpool";
                long count = 0L;
                if (base.IsExistData(str))
                {
                    ItemIDPool pool = (ItemIDPool) base.ReadFromDB(str);
                    if (pool != null)
                    {
                        count = pool.count;
                        pool.count = count + 0x3e8L;
                        base.WriteToDB(pool, str);
                        return count;
                    }
                }
                ItemIDPool pool2 = new ItemIDPool();
                pool2.count = count + 0x3e8L;
                base.WriteToDB(pool2, str);
                num2 = count;
            }
            finally
            {
                ItemIdPoolMutex.Leave();
            }
            return num2;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(ItemIDPool), ConfigManager.GetFileDBPath("itemidpool"), ".xml");
        }
    }
}

