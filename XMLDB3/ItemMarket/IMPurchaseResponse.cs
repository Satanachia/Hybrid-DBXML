namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;
    using System.Net;

    public class IMPurchaseResponse : ItemMarketResponse
    {
        public override void Build(BinaryReader _br, Message _message)
        {
            _br.ReadInt32();
            PacketHelper.ReadStringPacket(_br);
            base.result = IPAddress.NetworkToHostOrder(_br.ReadInt32());
            if (base.result == 1)
            {
                _message.WriteU8(1);
            }
            else
            {
                _message.WriteU8(0x33);
                _message.WriteS32(base.result);
            }
        }
    }
}

