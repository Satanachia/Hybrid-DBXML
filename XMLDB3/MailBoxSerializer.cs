namespace XMLDB3
{
    using Mabinogi;
    using System;

    internal class MailBoxSerializer
    {
        public static Message Deserialize(MailBox _mailBox, Message _message)
        {
            if (_mailBox.mailItem != null)
            {
                _message.WriteS32(_mailBox.mailItem.Length);
                foreach (MailItem item in _mailBox.mailItem)
                {
                    MailItemSeirializer.Deserialize(item, _message);
                }
                return _message;
            }
            _message.WriteS32(0);
            return _message;
        }
    }
}

