namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMItemListCommand : ItemMarketCommand
    {
        private const byte packetType = 0x31;

        public IMItemListCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, IMSortingType sorting, bool bAsc)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x31);
            writer.Write(IPAddress.HostToNetworkOrder(-1));
            PacketHelper.WriteStringPacket(writer, accounId);
            writer.Write(IPAddress.HostToNetworkOrder(pageNo));
            writer.Write(IPAddress.HostToNetworkOrder(pageItemCount));
            writer.Write((byte) sorting);
            writer.Write(bAsc ? ((byte) 1) : ((byte) 0));
        }
    }
}

