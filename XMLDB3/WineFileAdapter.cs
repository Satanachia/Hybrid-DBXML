namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public class WineFileAdapter : FileAdapter, WineAdapter
    {
        public bool Delete(long _charID)
        {
            if (base.IsExistData(_charID))
            {
                base.DeleteDB(_charID);
            }
            return true;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Wine), ConfigManager.GetFileDBPath("wine"), ".xml");
        }

        public REPLY_RESULT Read(long _charID, out Wine _wine)
        {
            if (base.IsExistData(_charID))
            {
                _wine = (Wine) base.ReadFromDB(_charID.ToString());
                return REPLY_RESULT.SUCCESS;
            }
            _wine = null;
            return REPLY_RESULT.FAIL_EX;
        }

        public bool Update(Wine _wine)
        {
            base.WriteToDB(_wine, _wine.charID);
            return true;
        }
    }
}

