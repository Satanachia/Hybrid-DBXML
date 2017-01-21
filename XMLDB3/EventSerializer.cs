namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class EventSerializer
    {
        public static Message Deserialize(Event _e, Message _Msg)
        {
            _Msg.WriteU8(_e.eventType);
            _Msg.WriteString(_e.account);
            _Msg.WriteString(_e.charName);
            _Msg.WriteString(_e.serverName);
            return _Msg;
        }

        public static Event Serialize(Message _Msg)
        {
            Event event2 = new Event();
            event2.eventType = _Msg.ReadU8();
            event2.account = _Msg.ReadString();
            event2.charName = _Msg.ReadString();
            event2.serverName = _Msg.ReadString();
            return event2;
        }
    }
}

