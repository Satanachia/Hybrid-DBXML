namespace XMLDB3
{
    using System;

    public interface IEncryptionMethod
    {
        void Close();
        string DecryptColumn(byte[] _inBuf);
        byte[] EncryptColumn(string _input);
        void Init();
    }
}

