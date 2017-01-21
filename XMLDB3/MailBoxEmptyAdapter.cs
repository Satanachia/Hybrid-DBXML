namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    internal class MailBoxEmptyAdapter : MailBoxAdapter
    {
        public long CheckCharacterName(string _name, ref string _outName, ref byte _errorCode)
        {
            return 0L;
        }

        public bool DeleteMail(long _postID, long _itemID, byte _storedType, long _receiverCharID, long _senderCharID, ref byte _errorCode)
        {
            return false;
        }

        public bool GetUnreadCount(long _receiverID, out int _unreadCount)
        {
            _unreadCount = 0;
            return false;
        }

        public void Initialize(string _argument)
        {
        }

        public bool ReadMail(long _charID, MailBox _recvBox, MailBox _sendBox)
        {
            return false;
        }

        public long SendMail(MailItem _item, ref byte _errorCode)
        {
            return 0L;
        }

        public bool UpdateMail(long _postID, byte _status, long _receiverCharID, long _senderCharID)
        {
            return false;
        }
    }
}

