namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;
    using System.Net;
    using XMLDB3;

    public class IMGetItemResponse : ItemMarketResponse
    {
        public override void Build(BinaryReader _br, Message _message)
        {
            _br.ReadInt32();
            PacketHelper.ReadStringPacket(_br);
            base.result = IPAddress.NetworkToHostOrder(_br.ReadInt32());
            if (base.result == 1)
            {
                _message.WriteU8(1);
                _message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
                long network = _br.ReadInt64();
                _message.WriteS64(IPAddress.NetworkToHostOrder(network));
                _br.ReadInt32();
                PacketHelper.ReadStringPacket(_br);
                _br.ReadInt16();
                string str = string.Empty;
                Item item = PacketHelper.ReadItemPacket(_br, true, out str);
                if (item != null)
                {
                    _message.WriteU8(1);
                    int num2 = Convert.ToInt32(str.Split(new char[] { ':' })[0]);
                    if (ConfigManager.ItemMarketServerNo != num2)
                    {
                        _message.WriteU8(1);
                    }
                    else
                    {
                        _message.WriteU8(0);
                    }
                    _message.WriteString(str);
                    ItemSerializer.Deserialize(item, _message);
                }
                else
                {
                    ExceptionMonitor.ExceptionRaised(new Exception("Invalid Item String"), network);
                    _message.WriteU8(0);
                }
            }
            else
            {
                _message.WriteU8(0x33);
                _message.WriteS32(base.result);
            }
        }
    }
}

