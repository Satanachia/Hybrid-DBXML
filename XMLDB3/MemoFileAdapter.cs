namespace XMLDB3
{
    using System;

    public class MemoFileAdapter : FileAdapter, MemoAdapter
    {
        public void Initialize(string _Argument)
        {
            base.Initialize(typeof(MemoList), ConfigManager.GetFileDBPath("memo"), ".xml");
        }

        public bool SendMemo(Memo _memo)
        {
            if ((_memo != null) && (_memo.receipants != null))
            {
                foreach (MemoCharacter character in _memo.receipants)
                {
                    MemoList list;
                    MemoContent[] contentArray;
                    if (!base.IsExistData(character.account))
                    {
                        list = new MemoList();
                        list.account = character.account;
                    }
                    else
                    {
                        list = (MemoList) base.ReadFromDB(character.account);
                    }
                    if (list == null)
                    {
                        return false;
                    }
                    if (list.memo != null)
                    {
                        contentArray = new MemoContent[list.memo.Length + 1];
                        list.memo.CopyTo(contentArray, 0);
                        contentArray[list.memo.Length] = new MemoContent();
                        contentArray[list.memo.Length].receipantName = character.name;
                        contentArray[list.memo.Length].receipantServer = character.server;
                        contentArray[list.memo.Length].content = _memo.content;
                        contentArray[list.memo.Length].sender = _memo.sender;
                    }
                    else
                    {
                        contentArray = new MemoContent[] { new MemoContent() };
                        contentArray[0].receipantName = character.name;
                        contentArray[0].receipantServer = character.server;
                        contentArray[0].content = _memo.content;
                        contentArray[0].sender = _memo.sender;
                    }
                    list.memo = contentArray;
                    base.WriteToDB(list, list.account);
                }
            }
            return true;
        }
    }
}

