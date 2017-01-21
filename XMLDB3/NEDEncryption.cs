namespace XMLDB3
{
    using NEDManaged;
    using System;

    public class NEDEncryption : IEncryptionMethod
    {
        private NEDRowAccessorManaged accessor = null;
        private string columnName;
        private static bool initialized = false;
        private ulong passCode = 0x381d8f66450c76d8L;

        public NEDEncryption(string _columnName, ulong _passCode)
        {
            this.columnName = _columnName;
        }

        public void Close()
        {
            if (NEDManaged.NEDGetRefCount() > 0)
            {
                NEDManaged.NEDTerminate(false);
            }
        }

        public string DecryptColumn(byte[] _inBuf)
        {
            string str;
            if ((this.accessor != null) && (this.accessor.DecryptColumn(this.columnName, _inBuf, out str) == 0))
            {
                return str;
            }
            return null;
        }

        public byte[] EncryptColumn(string _input)
        {
            if (this.accessor != null)
            {
                NEDManagedOriginal.NEDInit();
                int columnSize = NEDManagedOriginal.NEDGetColumnSize((uint) NEDManagedOriginal.NEDGetRowAccessor("mabinogi", 0x7868bb80681d456eL, "serialnumber", "serialnumber", null, null, null, null, null, null, null), "serialnumber");
                columnSize = this.accessor.GetColumnSize(this.columnName);
                if (columnSize <= 0)
                {
                    throw new Exception(this.accessor.GetLastErrorMessage());
                }
                byte[] aOutBuf = new byte[columnSize];
                if (this.accessor.EncryptColumn(this.columnName, _input, out aOutBuf) == 0)
                {
                    return aOutBuf;
                }
            }
            return null;
        }

        public void Init()
        {
            if (!initialized && !NEDManaged.NEDInit2("http://222.122.222.33/NED/NED.asmx"))
            {
                throw new Exception("Fail to init Encryption module.");
            }
            this.accessor = new NEDRowAccessorManaged(ConfigManager.EncryptionUser, this.passCode, "", this.columnName);
        }
    }
}

