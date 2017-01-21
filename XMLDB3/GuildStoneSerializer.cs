namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildStoneSerializer
    {
        public static void Deserialize(GuildStone _stone, Message _messsage)
        {
            if (_stone == null)
            {
                _stone = new GuildStone();
            }
            _messsage.WriteString(_stone.server);
            _messsage.WriteS64(_stone.position_id);
            _messsage.WriteS32(_stone.type);
            _messsage.WriteS16(_stone.region);
            _messsage.WriteS32(_stone.x);
            _messsage.WriteS32(_stone.y);
            _messsage.WriteFloat(_stone.direction);
        }

        public static GuildStone Serialize(Message _message)
        {
            GuildStone stone = new GuildStone();
            stone.server = _message.ReadString();
            stone.position_id = _message.ReadS64();
            stone.type = _message.ReadS32();
            stone.region = _message.ReadS16();
            stone.x = _message.ReadS32();
            stone.y = _message.ReadS32();
            stone.direction = _message.ReadFloat();
            return stone;
        }
    }
}

