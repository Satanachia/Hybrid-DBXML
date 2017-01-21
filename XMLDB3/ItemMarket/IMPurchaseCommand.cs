namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMPurchaseCommand : ItemMarketCommand
    {
        private const byte packetType = 0x44;

        public IMPurchaseCommand(int serverNo, string accountId, string nexonId, long tradeNo)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x44);
            writer.Write(IPAddress.HostToNetworkOrder(serverNo));
            PacketHelper.WriteStringPacket(writer, accountId);
            PacketHelper.WriteStringPacket(writer, nexonId);
            writer.Write(IPAddress.HostToNetworkOrder((short) 7));
            writer.Write((byte) 0);
            writer.Write(IPAddress.HostToNetworkOrder(1));
            writer.Write(IPAddress.HostToNetworkOrder(tradeNo));
        }
    }
}

