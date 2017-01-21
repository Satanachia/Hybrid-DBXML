namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml.Serialization;

    public class MailBoxFileAdapter : FileAdapter, MailBoxAdapter
    {
        private MailIDPool idPool = null;
        private XmlSerializer idSerializer = null;

        private MailBox _DeleteMail(long _postID, string _mailBox, ref byte _errorCode)
        {
            bool flag = false;
            if (base.IsExistData(_mailBox))
            {
                MailBox box = (MailBox) base.ReadFromDB(_mailBox);
                if (box != null)
                {
                    if ((box.mailItem != null) && (box.mailItem.Length > 0))
                    {
                        ArrayList list = new ArrayList();
                        foreach (MailItem item in box.mailItem)
                        {
                            if (item.postID == _postID)
                            {
                                flag = true;
                            }
                            else
                            {
                                list.Add(item);
                            }
                        }
                        if (flag)
                        {
                            if (list.Count > 0)
                            {
                                box.mailItem = (MailItem[]) list.ToArray(typeof(MailItem));
                                return box;
                            }
                            box.mailItem = null;
                            return box;
                        }
                        _errorCode = 3;
                        return null;
                    }
                    _errorCode = 3;
                }
                return null;
            }
            _errorCode = 3;
            return null;
        }

        private MailBox _SendMail(string _mailBox, MailItem _mail)
        {
            MailItem[] itemArray;
            if (!base.IsExistData(_mailBox))
            {
                MailBox box = new MailBox();
                box.mailItem = new MailItem[] { _mail };
                return box;
            }
            MailBox box2 = (MailBox) base.ReadFromDB(_mailBox);
            if (box2 == null)
            {
                return null;
            }
            if ((box2.mailItem == null) || (box2.mailItem.Length == 0))
            {
                itemArray = new MailItem[] { _mail };
            }
            else
            {
                itemArray = new MailItem[box2.mailItem.Length + 1];
                box2.mailItem.CopyTo(itemArray, 0);
                itemArray[box2.mailItem.Length] = _mail;
            }
            box2.mailItem = itemArray;
            return box2;
        }

        private MailBox _UpdateMail(long _postID, string _mailBox, byte _status)
        {
            if (base.IsExistData(_mailBox))
            {
                MailBox box = (MailBox) base.ReadFromDB(_mailBox);
                if (box == null)
                {
                    return null;
                }
                if ((box.mailItem == null) || (box.mailItem.Length <= 0))
                {
                    return null;
                }
                foreach (MailItem item in box.mailItem)
                {
                    if (item.postID == _postID)
                    {
                        item.status = _status;
                        return box;
                    }
                }
            }
            return null;
        }

        public long CheckCharacterName(string _name, ref string _outName, ref byte _errorCode)
        {
            CharacterFileAdapter character = (CharacterFileAdapter) QueryManager.Character;
            DirectoryInfo info = new DirectoryInfo(character.Directory);
            foreach (FileInfo info2 in info.GetFiles("*.xml"))
            {
                long num = Convert.ToInt64(info2.Name.Substring(0, info2.Name.LastIndexOf(".")));
                CharacterInfo info3 = character.Read(num, null);
                if ((info3 != null) && (info3.name == _name))
                {
                    _outName = info3.name;
                    return info3.id;
                }
            }
            _errorCode = 4;
            return 0L;
        }

        public bool DeleteMail(long _postID, long _itemID, byte _storedType, long _receiverCharID, long _senderCharID, ref byte _errorCode)
        {
            _errorCode = 0;
            string receiveBoxName = this.GetReceiveBoxName(_receiverCharID);
            string sendBoxName = this.GetSendBoxName(_senderCharID);
            MailBox box = this._DeleteMail(_postID, receiveBoxName, ref _errorCode);
            MailBox box2 = this._DeleteMail(_postID, sendBoxName, ref _errorCode);
            if ((box != null) && (box2 != null))
            {
                base.WriteToDB(box, receiveBoxName);
                base.WriteToDB(box2, sendBoxName);
                return true;
            }
            return false;
        }

        private long GetNextMailID()
        {
            this.idPool.MailID += 1L;
            TextWriter textWriter = new StreamWriter(base.GetFileName("mailID"), false, Encoding.Unicode);
            this.idSerializer.Serialize(textWriter, this.idPool);
            textWriter.Close();
            return this.idPool.MailID;
        }

        private string GetReceiveBoxName(long _charID)
        {
            return ("recv" + _charID.ToString());
        }

        private string GetSendBoxName(long _charID)
        {
            return ("send" + _charID.ToString());
        }

        public bool GetUnreadCount(long _receiverID, out int _unreadCount)
        {
            _unreadCount = 0;
            string receiveBoxName = this.GetReceiveBoxName(_receiverID);
            if (!base.IsExistData(receiveBoxName))
            {
                return false;
            }
            MailBox box = base.ReadFromDB(receiveBoxName) as MailBox;
            if ((box == null) || (box.mailItem == null))
            {
                return false;
            }
            foreach (MailItem item in box.mailItem)
            {
                if ((item != null) && (item.status == 1))
                {
                    _unreadCount++;
                }
            }
            return true;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(MailBox), ConfigManager.GetFileDBPath("MailBox"), ".xml");
            this.idSerializer = new XmlSerializer(typeof(MailIDPool));
            if (base.IsExistData("mailID"))
            {
                TextReader textReader = new StreamReader(base.GetFileName("mailID"), Encoding.Unicode);
                this.idPool = (MailIDPool) this.idSerializer.Deserialize(textReader);
            }
            else
            {
                this.idPool = new MailIDPool();
                this.idPool.MailID = 0L;
            }
        }

        public bool ReadMail(long _charID, MailBox _recvBox, MailBox _sendBox)
        {
            string receiveBoxName = this.GetReceiveBoxName(_charID);
            string sendBoxName = this.GetSendBoxName(_charID);
            if (base.IsExistData(receiveBoxName))
            {
                MailBox box = (MailBox) base.ReadFromDB(receiveBoxName);
                if (box == null)
                {
                    return false;
                }
                _recvBox.mailItem = box.mailItem;
            }
            else
            {
                _recvBox.mailItem = null;
            }
            if (base.IsExistData(sendBoxName))
            {
                MailBox box2 = (MailBox) base.ReadFromDB(sendBoxName);
                if (box2 == null)
                {
                    return false;
                }
                _sendBox.mailItem = box2.mailItem;
            }
            else
            {
                _sendBox.mailItem = null;
            }
            return true;
        }

        public long SendMail(MailItem _mail, ref byte _errorCode)
        {
            _mail.postID = this.GetNextMailID();
            string receiveBoxName = this.GetReceiveBoxName(_mail.receiverCharID);
            string sendBoxName = this.GetSendBoxName(_mail.senderCharID);
            MailBox box = this._SendMail(receiveBoxName, _mail);
            MailBox box2 = this._SendMail(sendBoxName, _mail);
            if ((box != null) && (box2 != null))
            {
                base.WriteToDB(box, receiveBoxName);
                base.WriteToDB(box2, sendBoxName);
                return _mail.postID;
            }
            _errorCode = 0;
            return 0L;
        }

        public bool UpdateMail(long _postID, byte _status, long _receiverCharID, long _senderCharID)
        {
            string receiveBoxName = this.GetReceiveBoxName(_receiverCharID);
            string sendBoxName = this.GetSendBoxName(_senderCharID);
            MailBox box = this._UpdateMail(_postID, receiveBoxName, _status);
            MailBox box2 = this._UpdateMail(_postID, sendBoxName, _status);
            if ((box != null) && (box2 != null))
            {
                base.WriteToDB(box, receiveBoxName);
                base.WriteToDB(box2, sendBoxName);
                return true;
            }
            return false;
        }

        public class MailIDPool
        {
            public long MailID;
        }
    }
}

