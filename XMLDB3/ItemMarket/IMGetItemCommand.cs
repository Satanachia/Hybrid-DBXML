namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMGetItemCommand : ItemMarketCommand
    {
        private const byte packetType = 0x81;

        public IMGetItemCommand(int serverNo, string accountId, long tradeNo)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x81);
            writer.Write(IPAddress.HostToNetworkOrder(serverNo));
            PacketHelper.WriteStringPacket(writer, accountId);
            writer.Write(IPAddress.HostToNetworkOrder(tradeNo));
        }
    }
}

