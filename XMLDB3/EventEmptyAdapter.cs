namespace XMLDB3
{
    using System;

    public class EventEmptyAdapter : EventAdapter
    {
        public void Initialize(string _argument)
        {
        }

        public REPLY_RESULT Update(Event _event, ref byte _errorCode)
        {
            return REPLY_RESULT.FAIL;
        }
    }
}

