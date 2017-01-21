namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WorldMetaListSerializer
    {
        public static void Deserialize(WorldMetaList _list, Message _message)
        {
            if (((_list == null) || (_list.metas == null)) || (_list.metas.Length == 0))
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_list.metas.Length);
                foreach (WorldMeta meta in _list.metas)
                {
                    WorldMetaSerializer.Deserialize(meta, _message);
                }
            }
        }
    }
}

