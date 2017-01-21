namespace XMLDB3
{
    using System;

    public class GuildIdPoolFileAdapter : FileAdapter, GuildIdPoolAdapter
    {
        public long GetIdPool()
        {
            string str = "guildidpool";
            long count = 1L;
            if (base.IsExistData(str))
            {
                GuildIDPool pool = (GuildIDPool) base.ReadFromDB(str);
                if (pool != null)
                {
                    count = pool.count;
                    pool.count = count + 0x3e8L;
                    base.WriteToDB(pool, str);
                    return count;
                }
            }
            GuildIDPool pool2 = new GuildIDPool();
            pool2.count = count + 0x3e8L;
            base.WriteToDB(pool2, str);
            return count;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(GuildIDPool), ConfigManager.GetFileDBPath("guildidpool"), ".xml");
        }
    }
}

