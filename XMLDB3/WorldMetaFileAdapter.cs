namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.IO;

    public class WorldMetaFileAdapter : FileAdapter, WorldMetaAdapter
    {
        public void Initialize(string _argument)
        {
            base.Initialize(typeof(WorldMeta), ConfigManager.GetFileDBPath("worldmeta"), ".xml");
        }

        public WorldMetaList Read()
        {
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if ((files == null) || (files.Length <= 0))
            {
                return new WorldMetaList();
            }
            ArrayList list = new ArrayList();
            foreach (string str in files)
            {
                WorldMeta meta = (WorldMeta) base.ReadFromDB(Path.GetFileNameWithoutExtension(str));
                if (meta != null)
                {
                    list.Add(meta);
                }
            }
            WorldMetaList list2 = new WorldMetaList();
            list2.metas = (WorldMeta[]) list.ToArray(typeof(WorldMeta));
            return list2;
        }

        private REPLY_RESULT Remove(string _key, ref byte _errorCode)
        {
            if (base.IsExistData(_key))
            {
                if (base.DeleteDB(_key))
                {
                    return REPLY_RESULT.SUCCESS;
                }
                return REPLY_RESULT.FAIL;
            }
            _errorCode = 0;
            return REPLY_RESULT.FAIL_EX;
        }

        private REPLY_RESULT Update(WorldMeta _worldmeta, ref byte _errorCode)
        {
            base.WriteToDB(_worldmeta, _worldmeta.key);
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT UpdateList(WorldMetaList _worldmetaUpdateList, string[] _removeKeys, ref byte _errorCode)
        {
            if (_worldmetaUpdateList != null)
            {
                foreach (WorldMeta meta in _worldmetaUpdateList.metas)
                {
                    REPLY_RESULT reply_result = this.Update(meta, ref _errorCode);
                    if (reply_result != REPLY_RESULT.SUCCESS)
                    {
                        return reply_result;
                    }
                }
            }
            if (_removeKeys != null)
            {
                foreach (string str in _removeKeys)
                {
                    REPLY_RESULT reply_result2 = this.Remove(str, ref _errorCode);
                    if (reply_result2 != REPLY_RESULT.SUCCESS)
                    {
                        return reply_result2;
                    }
                }
            }
            return REPLY_RESULT.SUCCESS;
        }
    }
}

