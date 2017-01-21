namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMSaleRequestCommand : ItemMarketCommand
    {
        private const byte packetType = 0x41;

        public IMSaleRequestCommand(int ServerNo, string accounId, Item item, string itemName, int price, int itemFee, int itemRegistFee, byte salePeriod)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x41);
            writer.Write(IPAddress.HostToNetworkOrder(ServerNo));
            PacketHelper.WriteStringPacket(writer, accounId);
            writer.Write(IPAddress.HostToNetworkOrder(item.@class));
            writer.Write(0);
            writer.Write(0);
            PacketHelper.WriteStringPacket(writer, itemName);
            writer.Write(IPAddress.HostToNetworkOrder((short) 1));
            writer.Write(IPAddress.HostToNetworkOrder(price));
            writer.Write(IPAddress.HostToNetworkOrder(itemFee));
            writer.Write(IPAddress.HostToNetworkOrder(itemRegistFee));
            writer.Write(salePeriod);
            PacketHelper.WriteItemPacket(writer, item, ServerNo);
        }
    }
}

