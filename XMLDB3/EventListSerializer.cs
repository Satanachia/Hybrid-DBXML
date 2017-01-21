namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class EventListSerializer
    {
        public static void Deserialize(EventList _list, Message _message)
        {
            if (((_list == null) || (_list.events == null)) || (_list.events.Length == 0))
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_list.events.Length);
                foreach (Event event2 in _list.events)
                {
                    EventSerializer.Deserialize(event2, _message);
                }
            }
        }
    }
}

