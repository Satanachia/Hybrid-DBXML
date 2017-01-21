namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;

    public class IMHeartbeatResponse : ItemMarketResponse
    {
        public override void Build(BinaryReader _br, Message _message)
        {
            base.result = _br.ReadByte();
        }

        public override bool IsSystemMessage
        {
            get
            {
                return true;
            }
        }
    }
}

