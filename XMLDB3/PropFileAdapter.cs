namespace XMLDB3
{
    using System;
    using System.IO;

    public class PropFileAdapter : FileAdapter, PropAdapter
    {
        public bool Create(Prop _data)
        {
            if (!base.IsExistData(_data.id))
            {
                base.WriteToDB(_data, _data.id);
                return true;
            }
            return false;
        }

        public bool Delete(long _id)
        {
            return base.DeleteDB(_id);
        }

        public void Initialize(string _argument)
        {
            base.Initialize(typeof(Prop), ConfigManager.GetFileDBPath("prop"), ".xml");
        }

        public PropIDList LoadPropList()
        {
            PropIDList list = new PropIDList();
            string[] files = System.IO.Directory.GetFiles(base.Directory);
            if (files != null)
            {
                list.propID = new long[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    list.propID[i] = Convert.ToInt64(Path.GetFileNameWithoutExtension(files[i]), 10);
                }
            }
            return list;
        }

        public Prop Read(long _id)
        {
            if (base.IsExistData(_id))
            {
                return (Prop) base.ReadFromDB(_id);
            }
            return null;
        }

        public bool Write(Prop _data)
        {
            if (base.IsExistData(_data.id))
            {
                base.WriteToDB(_data, _data.id);
                return true;
            }
            return false;
        }
    }
}

