namespace XMLDB3
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public class FileAdapter
    {
        private string directory;
        private string extension;
        private XmlSerializer serializer;

        public bool DeleteDB(object _obj)
        {
            return this.DeleteDB(_obj.ToString());
        }

        public bool DeleteDB(string _Name)
        {
            File.Delete(this.GetFileName(_Name));
            return true;
        }

        protected string GetFileName(string _Name)
        {
            return (this.directory + _Name + this.extension);
        }

        public void Initialize(Type _type, string _directory, string _extension)
        {
            this.serializer = new XmlSerializer(_type);
            this.directory = _directory;
            this.extension = _extension;
            if (!System.IO.Directory.Exists(_directory))
            {
                System.IO.Directory.CreateDirectory(_directory);
            }
        }

        public bool IsExistData(object _obj)
        {
            return this.IsExistData(_obj.ToString());
        }

        public bool IsExistData(string _Name)
        {
            return File.Exists(this.GetFileName(_Name));
        }

        public object ReadFromDB(object _obj)
        {
            return this.ReadFromDB(_obj.ToString());
        }

        public object ReadFromDB(string _Name)
        {
            TextReader textReader = new StreamReader(this.GetFileName(_Name), Encoding.Unicode);
            object obj2 = this.serializer.Deserialize(textReader);
            textReader.Close();
            return obj2;
        }

        public void WriteToDB(object _Data, object _Name)
        {
            this.WriteToDB(_Data, _Name.ToString());
        }

        public void WriteToDB(object _Data, string _Name)
        {
            TextWriter textWriter = new StreamWriter(this.GetFileName(_Name), false, Encoding.Unicode);
            this.serializer.Serialize(textWriter, _Data);
            textWriter.Close();
        }

        public string Directory
        {
            get
            {
                return this.directory;
            }
        }
    }
}

