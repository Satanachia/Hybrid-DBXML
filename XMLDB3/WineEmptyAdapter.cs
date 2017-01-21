namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public class WineEmptyAdapter : WineAdapter
    {
        public bool Delete(long _charID)
        {
            return false;
        }

        public void Initialize(string _argument)
        {
        }

        public REPLY_RESULT Read(long _charID, out Wine _wine)
        {
            _wine = null;
            return REPLY_RESULT.FAIL;
        }

        public bool Update(Wine _wine)
        {
            return false;
        }
    }
}

