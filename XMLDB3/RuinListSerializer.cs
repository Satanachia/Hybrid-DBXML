namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RuinListSerializer
    {
        public static Message Deserialize(RuinList _list, Message _Msg)
        {
            if ((_list.ruins != null) && (_list.ruins.Length > 0))
            {
                _Msg.WriteS32(_list.ruins.Length);
                foreach (Ruin ruin in _list.ruins)
                {
                    RuinSerializer.Deserialize(ruin, _Msg);
                }
                return _Msg;
            }
            _Msg.WriteS32(0);
            return _Msg;
        }

        public static RuinList Serialize(Message _Msg)
        {
            RuinList list = new RuinList();
            int num = _Msg.ReadS32();
            if (num > 0)
            {
                list.ruins = new Ruin[num];
                for (int i = 0; i < num; i++)
                {
                    list.ruins[i] = RuinSerializer.Serialize(_Msg);
                }
                return list;
            }
            list.ruins = null;
            return list;
        }
    }
}

