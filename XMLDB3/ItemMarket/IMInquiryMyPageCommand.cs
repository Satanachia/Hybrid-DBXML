namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMInquiryMyPageCommand : ItemMarketCommand
    {
        private const byte packetType = 0x24;

        public IMInquiryMyPageCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, bool bSale, IMSortingType sorting, bool bAsc)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x24);
            writer.Write(IPAddress.HostToNetworkOrder(ServerNo));
            PacketHelper.WriteStringPacket(writer, accounId);
            writer.Write(IPAddress.HostToNetworkOrder(pageNo));
            writer.Write(IPAddress.HostToNetworkOrder(pageItemCount));
            writer.Write(bSale ? ((byte) 1) : ((byte) 0));
            writer.Write((byte) sorting);
            writer.Write(bAsc ? ((byte) 1) : ((byte) 0));
        }
    }
}

