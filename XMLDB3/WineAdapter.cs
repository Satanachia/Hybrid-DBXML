namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public interface WineAdapter
    {
        bool Delete(long _charID);
        void Initialize(string _argument);
        REPLY_RESULT Read(long _charID, out Wine _wine);
        bool Update(Wine _wine);
    }
}

