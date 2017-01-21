namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMInitializeCommand : ItemMarketCommand
    {
        private const byte packetType = 1;

        public IMInitializeCommand(int gameNo, int serverNo)
        {
            base.ms = new MemoryStream(9);
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 1);
            writer.Write(IPAddress.HostToNetworkOrder(gameNo));
            writer.Write(IPAddress.HostToNetworkOrder(serverNo));
        }
    }
}

