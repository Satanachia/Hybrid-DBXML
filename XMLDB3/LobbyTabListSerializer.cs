namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class LobbyTabListSerializer
    {
        public static LobbyTabList Serialize(Message _message)
        {
            LobbyTabList list = new LobbyTabList();
            int num = _message.ReadS32();
            if (num > 0)
            {
                list.tabInfo = new LobbyTab[num];
                for (int i = 0; i < num; i++)
                {
                    list.tabInfo[i] = new LobbyTab();
                    list.tabInfo[i].charID = _message.ReadS64();
                    list.tabInfo[i].server = _message.ReadString();
                    list.tabInfo[i].tab = _message.ReadU8() == 1;
                }
            }
            return list;
        }
    }
}

