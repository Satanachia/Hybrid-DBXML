namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;

    public class RoyalAlchemistFileAdapter : FileAdapter, RoyalAlchemistAdapter
    {
        public REPLY_RESULT Add(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            base.WriteToDB(_royalAlchemist, _royalAlchemist.charID);
            return REPLY_RESULT.SUCCESS;
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(RoyalAlchemist), ConfigManager.GetFileDBPath("RoyalAlchemist"), ".xml");
        }

        public RoyalAlchemist Read(long _charID)
        {
            if (base.IsExistData(_charID))
            {
                return (RoyalAlchemist) base.ReadFromDB(_charID.ToString());
            }
            return null;
        }

        public RoyalAlchemistList ReadList()
        {
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if ((files == null) || (files.Length <= 0))
            {
                return new RoyalAlchemistList();
            }
            ArrayList list = new ArrayList();
            foreach (string str in files)
            {
                RoyalAlchemist alchemist = (RoyalAlchemist) base.ReadFromDB(Path.GetFileNameWithoutExtension(str));
                if (alchemist != null)
                {
                    list.Add(alchemist);
                }
            }
            RoyalAlchemistList list2 = new RoyalAlchemistList();
            list2.alchemists = (RoyalAlchemist[]) list.ToArray(typeof(RoyalAlchemist));
            return list2;
        }

        public REPLY_RESULT Remove(long[] _removeIDs, ref byte _errorCode)
        {
            foreach (long num in _removeIDs)
            {
                if (base.IsExistData(num))
                {
                    base.DeleteDB(num);
                }
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT Update(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            base.WriteToDB(_royalAlchemist, _royalAlchemist.charID);
            return REPLY_RESULT.SUCCESS;
        }
    }
}

