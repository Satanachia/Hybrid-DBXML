namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RoyalAlchemistSerializer
    {
        public static void Deserialize(RoyalAlchemist _data, Message _message)
        {
            if (_data != null)
            {
                _message.WriteS64(_data.charID);
                _message.WriteString(_data.charName);
                _message.WriteU8(_data.registrationFlag);
                _message.WriteU16(_data.rank);
                _message.WriteString(_data.meta);
            }
        }

        public static RoyalAlchemist Serialize(Message _message)
        {
            RoyalAlchemist alchemist = new RoyalAlchemist();
            alchemist.charID = _message.ReadS64();
            alchemist.charName = _message.ReadString();
            alchemist.registrationFlag = _message.ReadU8();
            alchemist.rank = _message.ReadU16();
            alchemist.meta = _message.ReadString();
            return alchemist;
        }
    }
}

