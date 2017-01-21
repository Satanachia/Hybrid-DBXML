namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMGetItemRollbackCommand : ItemMarketCommand
    {
        private const byte packetType = 0x83;

        public IMGetItemRollbackCommand(int serverNo, string accountId, long tradeNo, int itemLocation)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x83);
            writer.Write(IPAddress.HostToNetworkOrder(serverNo));
            PacketHelper.WriteStringPacket(writer, accountId);
            writer.Write(IPAddress.HostToNetworkOrder(tradeNo));
            writer.Write(IPAddress.HostToNetworkOrder(itemLocation));
        }
    }
}

