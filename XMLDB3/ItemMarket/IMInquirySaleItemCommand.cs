namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMInquirySaleItemCommand : ItemMarketCommand
    {
        private const byte packetType = 0x21;

        public IMInquirySaleItemCommand(int ServerNo, string accounId, int pageNo, int pageItemCount)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x21);
            writer.Write(IPAddress.HostToNetworkOrder(ServerNo));
            PacketHelper.WriteStringPacket(writer, accounId);
            writer.Write(IPAddress.HostToNetworkOrder(pageNo));
            writer.Write(IPAddress.HostToNetworkOrder(pageItemCount));
        }
    }
}

