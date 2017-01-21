namespace XMLDB3.ItemMarket
{
    using Mabinogi;
    using System;
    using System.IO;

    public class IMInitializeResponse : ItemMarketResponse
    {
        public override void Build(BinaryReader _br, Message _message)
        {
            _br.ReadInt32();
            _br.ReadInt32();
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

