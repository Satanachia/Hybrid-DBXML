namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class GroupIDListSerializer
    {
        public static GroupIDList Serialize(Message _message)
        {
            GroupIDList list = new GroupIDList();
            int num = _message.ReadS32();
            if (num > 0)
            {
                list.group = new GroupID[num];
                for (int i = 0; i < num; i++)
                {
                    list.group[i] = new GroupID();
                    list.group[i].charID = _message.ReadS64();
                    list.group[i].groupID = _message.ReadU8();
                }
                return list;
            }
            list.group = null;
            return list;
        }
    }
}

