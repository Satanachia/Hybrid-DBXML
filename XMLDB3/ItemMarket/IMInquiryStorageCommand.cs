namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMInquiryStorageCommand : ItemMarketCommand
    {
        private const byte packetType = 0x22;

        public IMInquiryStorageCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, IMStorageType storageType)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x22);
            writer.Write(IPAddress.HostToNetworkOrder(ServerNo));
            PacketHelper.WriteStringPacket(writer, accounId);
            writer.Write(IPAddress.HostToNetworkOrder(pageNo));
            writer.Write(IPAddress.HostToNetworkOrder(pageItemCount));
            writer.Write(IPAddress.HostToNetworkOrder((int) storageType));
        }
    }
}

