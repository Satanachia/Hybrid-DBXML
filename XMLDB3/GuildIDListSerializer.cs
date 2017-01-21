namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GuildIDListSerializer
    {
        public static void Deserialize(GuildIDList _list, Message _messsage)
        {
            if (_list == null)
            {
                _list = new GuildIDList();
            }
            if (_list.guildID != null)
            {
                _messsage.WriteS32(_list.guildID.Length);
                foreach (long num in _list.guildID)
                {
                    _messsage.WriteS64(num);
                }
            }
            else
            {
                _messsage.WriteU32(0);
            }
        }
    }
}

