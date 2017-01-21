namespace XMLDB3
{
    using System;

    public interface PropIdPoolAdapter
    {
        long GetIdPool();
        void Initialize(string _argument);
    }
}

