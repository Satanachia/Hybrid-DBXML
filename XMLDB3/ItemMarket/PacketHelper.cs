namespace XMLDB3.ItemMarket
{
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml.Serialization;
    using XMLDB3;

    public class PacketHelper
    {
        private static XmlSerializer itemSerializer;
        private static Encoding strEncoder;

        public static void Init(int codePage)
        {
            strEncoder = Encoding.GetEncoding(codePage);
            itemSerializer = new XmlSerializer(typeof(Item));
        }

        public static Item ReadItemPacket(BinaryReader _br, bool _bReadSecurityInfo)
        {
            string str = null;
            return ReadItemPacket(_br, _bReadSecurityInfo, out str);
        }

        public static Item ReadItemPacket(BinaryReader _br, bool _bReadSecurityInfo, out string _itemSecurityInfo)
        {
            _itemSecurityInfo = string.Empty;
            try
            {
                Item item = new Item();
                item.data = ReadStringPacket(_br);
                if (_bReadSecurityInfo)
                {
                    _itemSecurityInfo = ReadStringPacket(_br);
                }
                int num = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                item.id = IPAddress.HostToNetworkOrder(_br.ReadInt64());
                num--;
                num--;
                item.@class = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.color_01 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.color_02 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.color_03 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.price = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.sellingprice = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.bundle = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.figure = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.flag = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.durability = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.durability_max = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.origin_durability_max = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.attack_min = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.attack_max = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.wattack_min = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.wattack_max = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.balance = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.critical = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.defence = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.protect = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.effective_range = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.attack_speed = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.down_hit_count = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.experience = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.exp_point = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.upgraded = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.upgrade_max = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.grade = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.prefix = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.suffix = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.expiration = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.varint = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                item.storedtype = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                int num2 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                int num3 = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                num--;
                if ((num3 == 8) && (num2 > 0))
                {
                    item.options = new ItemOption[num2];
                    for (int i = 0; i < num2; i++)
                    {
                        item.options[i] = new ItemOption();
                        item.options[i].type = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].flag = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].execute = (short) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].execdata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].open = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].opendata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].enable = (byte) IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                        item.options[i].enabledata = IPAddress.NetworkToHostOrder(_br.ReadInt32());
                        num--;
                    }
                }
                if (num != 0)
                {
                    throw new Exception("Invalid Item Packet");
                }
                return item;
            }
            catch (Exception exception)
            {
                ExceptionMonitor.ExceptionRaised(exception);
                return null;
            }
        }

        public static string ReadStringPacket(BinaryReader _br)
        {
            ushort count = (ushort) IPAddress.NetworkToHostOrder(_br.ReadInt16());
            byte[] bytes = _br.ReadBytes(count);
            return strEncoder.GetString(bytes, 0, count);
        }

        public static void WriteItemPacket(BinaryWriter _bw, Item _item, int _serverNo)
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            WriteStringPacket(_bw, _item.data);
            WriteStringPacket(_bw, string.Format("{0}:{1}", _serverNo.ToString(), _item.id.ToString()));
            int host = 0;
            writer.Write(IPAddress.HostToNetworkOrder(_item.id));
            host++;
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.@class));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.color_01));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.color_02));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.color_03));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.price));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.sellingprice));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.bundle));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.figure));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.flag));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.durability));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.durability_max));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.origin_durability_max));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.attack_min));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.attack_max));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.wattack_min));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.wattack_max));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.balance));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.critical));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.defence));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.protect));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.effective_range));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.attack_speed));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.down_hit_count));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.experience));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.exp_point));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.upgraded));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.upgrade_max));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.grade));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.prefix));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.suffix));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.expiration));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.varint));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder((int) _item.storedtype));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(_item.options.Length));
            host++;
            writer.Write(IPAddress.HostToNetworkOrder(8));
            host++;
            foreach (ItemOption option in _item.options)
            {
                writer.Write(IPAddress.HostToNetworkOrder((int) option.type));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder(option.flag));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder((int) option.execute));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder(option.execdata));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder((int) option.open));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder(option.opendata));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder((int) option.enable));
                host++;
                writer.Write(IPAddress.HostToNetworkOrder(option.enabledata));
                host++;
            }
            _bw.Write(IPAddress.HostToNetworkOrder(host));
            _bw.Write(output.GetBuffer(), 0, (int) output.Position);
        }

        public static void WriteStringPacket(BinaryWriter _bw, string _text)
        {
            byte[] bytes = strEncoder.GetBytes(_text);
            _bw.Write(IPAddress.HostToNetworkOrder((short) bytes.Length));
            _bw.Write(bytes);
        }
    }
}

