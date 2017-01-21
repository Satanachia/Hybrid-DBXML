namespace XMLDB3
{
    using System;

    public interface LoginIdPoolAdapter
    {
        long GetIdPool(int _size);
        void Initialize(string _argument);
    }
}

