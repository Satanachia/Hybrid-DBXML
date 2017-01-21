namespace XMLDB3
{
    using System;

    public interface EventAdapter
    {
        void Initialize(string _argument);
        REPLY_RESULT Update(Event _event, ref byte _errorCode);
    }
}

