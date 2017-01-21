namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailItemSeirializer
    {
        public static Message Deserialize(MailItem _mailItem, Message _message)
        {
            _message.WriteS64(_mailItem.postID);
            _message.WriteS64(_mailItem.receiverCharID);
            _message.WriteString(_mailItem.receiverCharName);
            _message.WriteS64(_mailItem.senderCharID);
            _message.WriteString(_mailItem.senderCharName);
            _message.WriteS32(_mailItem.itemCharge);
            _message.WriteString(_mailItem.senderMsg);
            _message.WriteS64(_mailItem.sendDate.Ticks);
            _message.WriteU8(_mailItem.postType);
            _message.WriteString(_mailItem.location);
            _message.WriteU8(_mailItem.status);
            if (_mailItem.item != null)
            {
                ItemSerializer.Deserialize(_mailItem.item, _message);
                return _message;
            }
            _message.WriteS64(0L);
            return _message;
        }

        public static MailItem Serialize(Message _message)
        {
            MailItem item = new MailItem();
            item.receiverCharID = _message.ReadS64();
            item.receiverCharName = _message.ReadString();
            item.senderCharID = _message.ReadS64();
            item.senderCharName = _message.ReadString();
            item.itemCharge = _message.ReadS32();
            item.senderMsg = _message.ReadString();
            item.sendDate = new DateTime(_message.ReadS64());
            item.postType = _message.ReadU8();
            item.location = _message.ReadString();
            item.status = _message.ReadU8();
            if (_message.ReadU8() == 1)
            {
                item.item = ItemSerializer.Serialize(_message);
            }
            return item;
        }
    }
}

