namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class ShopAdvertisebaseSerializer
    {
        public static Message Deserialize(ShopAdvertisebase _advertise, Message _message)
        {
            if (_advertise == null)
            {
                _advertise = new ShopAdvertisebase();
                _advertise.account = string.Empty;
                _advertise.server = string.Empty;
                _advertise.shopName = string.Empty;
                _advertise.area = string.Empty;
                _advertise.characterName = string.Empty;
                _advertise.comment = string.Empty;
            }
            _message.WriteString(_advertise.account);
            _message.WriteString(_advertise.server);
            _message.WriteString(_advertise.shopName);
            _message.WriteString(_advertise.area);
            _message.WriteString(_advertise.characterName);
            _message.WriteString(_advertise.comment);
            _message.WriteS64(_advertise.startTime);
            _message.WriteS32(_advertise.region);
            _message.WriteS32(_advertise.x);
            _message.WriteS32(_advertise.y);
            _message.WriteS32(_advertise.leafletCount);
            return _message;
        }

        public static ShopAdvertisebase Serialize(Message _message)
        {
            ShopAdvertisebase advertisebase = new ShopAdvertisebase();
            advertisebase.account = _message.ReadString();
            advertisebase.server = _message.ReadString();
            advertisebase.shopName = _message.ReadString();
            advertisebase.area = _message.ReadString();
            advertisebase.characterName = _message.ReadString();
            advertisebase.comment = _message.ReadString();
            advertisebase.startTime = _message.ReadS64();
            advertisebase.region = _message.ReadS32();
            advertisebase.x = _message.ReadS32();
            advertisebase.y = _message.ReadS32();
            advertisebase.leafletCount = _message.ReadS32();
            return advertisebase;
        }
    }
}

