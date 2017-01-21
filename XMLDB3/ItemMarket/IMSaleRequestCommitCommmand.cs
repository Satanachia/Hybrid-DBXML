namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMSaleRequestCommitCommmand : ItemMarketCommand
    {
        private const byte packetType = 0x42;

        public IMSaleRequestCommitCommmand(int serverNo, string accountId, long tradeNo)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x42);
            writer.Write(IPAddress.HostToNetworkOrder(serverNo));
            PacketHelper.WriteStringPacket(writer, accountId);
            writer.Write(IPAddress.HostToNetworkOrder(tradeNo));
        }
    }
}

