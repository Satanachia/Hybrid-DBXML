namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CastleBlockSerializer
    {
        public static void Deserialize(CastleBlock[] _blocks, Message _message)
        {
            if (_blocks == null)
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_blocks.Length);
                foreach (CastleBlock block in _blocks)
                {
                    _message.WriteString(block.gameName);
                    _message.WriteU8(block.flag);
                    _message.WriteU8(block.entry);
                }
            }
        }

        public static CastleBlock[] Serialize(Message _message)
        {
            int num = _message.ReadS32();
            if (num == 0)
            {
                return null;
            }
            CastleBlock[] blockArray = new CastleBlock[num];
            for (int i = 0; i < num; i++)
            {
                blockArray[i] = new CastleBlock();
                blockArray[i].gameName = _message.ReadString();
                blockArray[i].flag = _message.ReadU8();
                blockArray[i].entry = _message.ReadU8();
            }
            return blockArray;
        }
    }
}

