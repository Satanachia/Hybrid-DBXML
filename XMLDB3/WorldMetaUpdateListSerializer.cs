namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class WorldMetaUpdateListSerializer
    {
        public static bool Serialize(Message _message, out WorldMetaList _updatelist, out string[] _removeKeys)
        {
            ArrayList list = new ArrayList();
            ArrayList list2 = new ArrayList();
            _updatelist = null;
            _removeKeys = null;
            uint num = _message.ReadU32();
            for (uint i = 0; i < num; i++)
            {
                WorldMeta meta;
                switch (_message.ReadU8())
                {
                    case 1:
                        meta = WorldMetaSerializer.Serialize(_message);
                        if (meta != null)
                        {
                            break;
                        }
                        return false;

                    case 2:
                    {
                        string str = _message.ReadString();
                        list2.Add(str);
                        continue;
                    }
                    default:
                        return false;
                }
                list.Add(meta);
            }
            _updatelist = new WorldMetaList();
            _updatelist.metas = (WorldMeta[]) list.ToArray(typeof(WorldMeta));
            _removeKeys = (string[]) list2.ToArray(typeof(string));
            return true;
        }

        private enum EMetaElementType
        {
            eRemoveValue = 2,
            eSetValue = 1
        }
    }
}

