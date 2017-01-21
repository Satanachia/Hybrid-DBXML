namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;
    using System.Net;
    using XMLDB3;

    public class IMItemSearchResponse : ItemMarketResponse
    {
        public override void Build(BinaryReader _br, Message _message)
        {
            base.result = IPAddress.NetworkToHostOrder(_br.ReadInt32());
            if (base.result != 1)
            {
                _message.WriteU8(0x33);
                _message.WriteS32(base.result);
            }
            else
            {
                _message.WriteU8(1);
                _message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
                int num = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                _message.WriteS32(num);
                for (int i = 0; i < num; i++)
                {
                    _br.ReadInt32();
                    _message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
                    long network = _br.ReadInt64();
                    _message.WriteS64(IPAddress.NetworkToHostOrder(network));
                    _br.ReadInt32();
                    PacketHelper.ReadStringPacket(_br);
                    _br.ReadInt16();
                    _message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
                    _message.WriteS32(IPAddress.NetworkToHostOrder(_br.ReadInt32()));
                    _message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
                    _message.WriteS64(IPAddress.NetworkToHostOrder(_br.ReadInt64()));
                    _message.WriteS64(0L);
                    PacketHelper.ReadStringPacket(_br);
                    PacketHelper.ReadStringPacket(_br);
                    _message.WriteString(PacketHelper.ReadStringPacket(_br));
                    Item item = PacketHelper.ReadItemPacket(_br, false);
                    if (item != null)
                    {
                        _message.WriteU8(1);
                        ItemSerializer.Deserialize(item, _message);
                    }
                    else
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("Invalid Item String"), network);
                        _message.WriteU8(0);
                    }
                }
            }
        }
    }
}

