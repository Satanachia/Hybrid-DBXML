namespace XMLDB3
{
    using System;

    public class EventFileAdapter : FileAdapter, EventAdapter
    {
        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Event), ConfigManager.GetFileDBPath("event"), ".xml");
        }

        public REPLY_RESULT Update(Event _event, ref byte _errorCode)
        {
            base.WriteToDB(_event, _event.account);
            return REPLY_RESULT.SUCCESS;
        }
    }
}

