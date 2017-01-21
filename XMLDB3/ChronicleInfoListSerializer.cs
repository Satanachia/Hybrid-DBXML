namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ChronicleInfoListSerializer
    {
        public static ChronicleInfoList Serialize(Message _message)
        {
            ChronicleInfoList list = new ChronicleInfoList();
            list.serverName = _message.ReadString();
            int num = _message.ReadS32();
            if (num > 0)
            {
                list.infos = new ChronicleInfo[num];
                for (int i = 0; i < num; i++)
                {
                    list.infos[i] = new ChronicleInfo();
                    list.infos[i].questID = _message.ReadS32();
                    list.infos[i].questName = _message.ReadString();
                    list.infos[i].keyword = _message.ReadString();
                    list.infos[i].localtext = _message.ReadString();
                    list.infos[i].sort = _message.ReadString();
                    list.infos[i].group = _message.ReadString();
                    list.infos[i].source = _message.ReadString();
                    list.infos[i].width = _message.ReadS16();
                    list.infos[i].height = _message.ReadS16();
                }
            }
            return list;
        }
    }
}

