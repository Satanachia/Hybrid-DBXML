namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildRobeSerializer
    {
        public static void Deserialize(GuildRobe _robe, Message _messsage)
        {
            if (_robe == null)
            {
                _robe = new GuildRobe();
            }
            _messsage.WriteU8(_robe.emblemChestIcon);
            _messsage.WriteU8(_robe.emblemChestDeco);
            _messsage.WriteU8(_robe.emblemBeltDeco);
            _messsage.WriteS32(_robe.color1);
            _messsage.WriteU8(_robe.color2Index);
            _messsage.WriteU8(_robe.color3Index);
            _messsage.WriteU8(_robe.color4Index);
            _messsage.WriteU8(_robe.color5Index);
        }

        public static GuildRobe Serialize(Message _message)
        {
            GuildRobe robe = new GuildRobe();
            robe.emblemChestIcon = _message.ReadU8();
            robe.emblemChestDeco = _message.ReadU8();
            robe.emblemBeltDeco = _message.ReadU8();
            robe.color1 = _message.ReadS32();
            robe.color2Index = _message.ReadU8();
            robe.color3Index = _message.ReadU8();
            robe.color4Index = _message.ReadU8();
            robe.color5Index = _message.ReadU8();
            return robe;
        }
    }
}

