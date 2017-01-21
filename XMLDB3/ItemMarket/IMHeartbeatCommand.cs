namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;

    public class IMHeartbeatCommand : ItemMarketCommand
    {
        private const byte packetType = 0xff;

        public IMHeartbeatCommand(int gameNo, int serverNo)
        {
            base.ms = new MemoryStream(9);
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0xff);
            writer.Write(IPAddress.HostToNetworkOrder(gameNo));
            writer.Write(IPAddress.HostToNetworkOrder(serverNo));
        }
    }
}

