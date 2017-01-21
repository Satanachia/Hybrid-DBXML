namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class WorldMetaSerializer
    {
        public static void Deserialize(WorldMeta _worldmeta, Message _message)
        {
            if (_worldmeta != null)
            {
                _message.WriteString(_worldmeta.key);
                _message.WriteU8(_worldmeta.type);
                switch (_worldmeta.type)
                {
                    case 1:
                        _message.WriteU8(byte.Parse(_worldmeta.value));
                        return;

                    case 2:
                        _message.WriteU16(ushort.Parse(_worldmeta.value));
                        return;

                    case 3:
                        _message.WriteU32(uint.Parse(_worldmeta.value));
                        return;

                    case 4:
                        _message.WriteU64(ulong.Parse(_worldmeta.value));
                        return;

                    case 5:
                        _message.WriteU8(bool.Parse(_worldmeta.value) ? ((byte) 1) : ((byte) 0));
                        return;

                    case 6:
                        _message.WriteFloat(float.Parse(_worldmeta.value));
                        return;

                    case 7:
                        return;

                    case 8:
                        _message.WriteString(_worldmeta.value);
                        return;
                }
            }
        }

        public static WorldMeta Serialize(Message _message)
        {
            WorldMeta meta = new WorldMeta();
            meta.key = _message.ReadString();
            meta.type = _message.ReadU8();
            switch (meta.type)
            {
                case 1:
                    meta.value = _message.ReadU8().ToString();
                    return meta;

                case 2:
                    meta.value = _message.ReadU16().ToString();
                    return meta;

                case 3:
                    meta.value = _message.ReadU32().ToString();
                    return meta;

                case 4:
                    meta.value = _message.ReadU64().ToString();
                    return meta;

                case 5:
                    meta.value = (_message.ReadU8() != 0).ToString();
                    return meta;

                case 6:
                    meta.value = _message.ReadFloat().ToString();
                    return meta;

                case 8:
                    meta.value = _message.ReadString();
                    return meta;
            }
            return null;
        }

        private enum EMetaElementType
        {
            metaNotAvail,
            metaByte,
            metaWord,
            metaDWord,
            metaQWord,
            metaBool,
            metaFloat,
            metaDouble,
            metaString,
            metaBinary,
            metaPtr
        }
    }
}

