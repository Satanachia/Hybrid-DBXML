namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMItemSearchCommand : ItemMarketCommand
    {
        private const byte packetType = 50;

        public IMItemSearchCommand(int ServerNo, string accounId, int pageNo, int pageItemCount, string itemName, IMSortingType sorting, bool bAsc, int itemGroup)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 50);
            writer.Write(IPAddress.HostToNetworkOrder(-1));
            PacketHelper.WriteStringPacket(writer, accounId);
            writer.Write(IPAddress.HostToNetworkOrder(pageNo));
            writer.Write(IPAddress.HostToNetworkOrder(pageItemCount));
            PacketHelper.WriteStringPacket(writer, itemName);
            writer.Write(IPAddress.HostToNetworkOrder(-1));
            writer.Write(IPAddress.HostToNetworkOrder(-1));
            writer.Write(IPAddress.HostToNetworkOrder(itemGroup));
            writer.Write((byte) sorting);
            writer.Write(bAsc ? ((byte) 1) : ((byte) 0));
        }
    }
}

