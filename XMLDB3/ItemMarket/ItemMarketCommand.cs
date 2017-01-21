namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;
    using XMLDB3;

    public class ItemMarketCommand
    {
        protected MemoryStream ms = null;
        protected byte[] packetBuffer;

        public void BuildPacket(int _packetNo)
        {
            this.BuildPacket(_packetNo, this.ms.GetBuffer(), (int) this.ms.Position);
        }

        protected void BuildPacket(int _packetNo, byte[] _data, int _length)
        {
            byte num = 160;
            this.packetBuffer = new byte[_length + 9];
            MemoryStream output = new MemoryStream(this.packetBuffer);
            BinaryWriter writer = new BinaryWriter(output);
            try
            {
                writer.Write(num);
                writer.Write(IPAddress.HostToNetworkOrder((int) (_length + 4)));
                writer.Write(IPAddress.HostToNetworkOrder(_packetNo));
                writer.Write(_data, 0, _length);
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
            }
            finally
            {
                writer.Close();
            }
        }

        public byte[] Packet
        {
            get
            {
                return this.packetBuffer;
            }
        }
    }
}

