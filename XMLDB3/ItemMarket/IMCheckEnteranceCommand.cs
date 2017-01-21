namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMCheckEnteranceCommand : ItemMarketCommand
    {
        private const byte packetType = 0x11;

        public IMCheckEnteranceCommand(int gameNo, int ServerNo, string nexonId, string accounId, string accountName)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x11);
            writer.Write(IPAddress.HostToNetworkOrder(gameNo));
            writer.Write(IPAddress.HostToNetworkOrder(ServerNo));
            PacketHelper.WriteStringPacket(writer, nexonId);
            PacketHelper.WriteStringPacket(writer, accounId);
            PacketHelper.WriteStringPacket(writer, accountName);
        }
    }
}

