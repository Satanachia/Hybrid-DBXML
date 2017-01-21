namespace XMLDB3
{
    using System;

    public interface CharIdPoolAdapter
    {
        long GetIdPool();
        void Initialize(string _argument);
    }
}

