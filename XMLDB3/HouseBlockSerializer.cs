namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class HouseBlockSerializer
    {
        public static void Deserialize(HouseBlock[] _blocks, Message _message)
        {
            if (_blocks != null)
            {
                _message.WriteS32(_blocks.Length);
                foreach (HouseBlock block in _blocks)
                {
                    _message.WriteString(block.gameName);
                    _message.WriteU8(block.flag);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
        }

        public static HouseBlock[] Serialize(Message _message)
        {
            int num = _message.ReadS32();
            if (num <= 0)
            {
                return null;
            }
            HouseBlock[] blockArray = new HouseBlock[num];
            for (int i = 0; i < num; i++)
            {
                blockArray[i] = new HouseBlock();
                blockArray[i].gameName = _message.ReadString();
                blockArray[i].flag = _message.ReadU8();
            }
            return blockArray;
        }
    }
}

