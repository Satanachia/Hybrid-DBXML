namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;

    public class IMCheckBalanceCommand : ItemMarketCommand
    {
        private const byte packetType = 0x12;

        public IMCheckBalanceCommand(string strNexonId)
        {
            base.ms = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(base.ms);
            writer.Write((byte) 0x12);
            PacketHelper.WriteStringPacket(writer, strNexonId);
        }
    }
}

