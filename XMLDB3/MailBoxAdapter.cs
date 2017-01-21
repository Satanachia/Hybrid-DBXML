namespace XMLDB3
{
    using System;
    using System.Runtime.InteropServices;

    public interface MailBoxAdapter
    {
        long CheckCharacterName(string _name, ref string _outName, ref byte _errorCode);
        bool DeleteMail(long _postID, long _itemID, byte _storedType, long _receiverCharID, long _senderCharID, ref byte _errorCode);
        bool GetUnreadCount(long _receiverID, out int _unreadCount);
        void Initialize(string _argument);
        bool ReadMail(long _charID, MailBox _recvBox, MailBox _sendBox);
        long SendMail(MailItem _item, ref byte _errorCode);
        bool UpdateMail(long _postID, byte _status, long _receiverCharID, long _senderCharID);
    }
}

