namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;

    public class BidFileAdapter : FileAdapter, BidAdapter
    {
        public bool Add(Bid _bid)
        {
            if (base.IsExistData(_bid.bidID.ToString()))
            {
                return false;
            }
            base.WriteToDB(_bid, _bid.bidID);
            return true;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Bid), ConfigManager.GetFileDBPath("bid"), ".xml");
        }

        public BidList Read()
        {
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if ((files == null) || (files.Length <= 0))
            {
                return new BidList();
            }
            ArrayList list = new ArrayList();
            foreach (string str in files)
            {
                Bid bid = (Bid) base.ReadFromDB(Path.GetFileNameWithoutExtension(str));
                if (bid != null)
                {
                    list.Add(bid);
                }
            }
            BidList list2 = new BidList();
            list2.bids = (Bid[]) list.ToArray(typeof(Bid));
            return list2;
        }

        public REPLY_RESULT Remove(long _bidID, ref byte _errorCode)
        {
            if (base.IsExistData(_bidID))
            {
                if (base.DeleteDB(_bidID))
                {
                    return REPLY_RESULT.SUCCESS;
                }
                return REPLY_RESULT.FAIL;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        public REPLY_RESULT Update(Bid _bid, ref byte _errorCode)
        {
            if (base.IsExistData(_bid.bidID))
            {
                base.WriteToDB(_bid, _bid.bidID);
                return REPLY_RESULT.SUCCESS;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }
    }
}

