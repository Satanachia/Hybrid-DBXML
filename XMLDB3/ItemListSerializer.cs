namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ItemListSerializer
    {
        public static ItemList[] Serialize(Message _message)
        {
            int num = _message.ReadS32();
            if (num <= 0)
            {
                return null;
            }
            ItemList[] listArray = new ItemList[num];
            for (int i = 0; i < num; i++)
            {
                listArray[i] = new ItemList();
                listArray[i].itemID = _message.ReadS64();
                listArray[i].storedtype = _message.ReadU8();
            }
            return listArray;
        }
    }
}

