namespace XMLDB3
{
    using System;

    public class ChannelingKeyPoolFileAdapter : FileAdapter, ChannelingKeyPoolAdapter
    {
        public bool Do(ChannelingKey _chKey)
        {
            byte provider = _chKey.provider;
            string keystring = _chKey.keystring;
            string str2 = provider.ToString() + "_" + keystring;
            if (!base.IsExistData(str2))
            {
                base.WriteToDB(_chKey, str2);
                return true;
            }
            return false;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(ChannelingKey), ConfigManager.GetFileDBPath("ChannelingKey"), ".xml");
        }
    }
}

