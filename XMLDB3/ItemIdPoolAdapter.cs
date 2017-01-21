namespace XMLDB3
{
    using System;

    public interface ItemIdPoolAdapter
    {
        long GetIdPool();
        void Initialize(string _argument);
    }
}

