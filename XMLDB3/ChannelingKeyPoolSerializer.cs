namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ChannelingKeyPoolSerializer
    {
        public static void Deserialize(ChannelingKey _chKeyPool, Message _message)
        {
        }

        public static ChannelingKey Serialize(Message _message)
        {
            ChannelingKey key = new ChannelingKey();
            key.provider = _message.ReadU8();
            key.keystring = _message.ReadString();
            return key;
        }
    }
}

