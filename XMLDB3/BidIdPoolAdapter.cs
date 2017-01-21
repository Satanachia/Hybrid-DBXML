namespace XMLDB3
{
    using System;

    public interface BidIdPoolAdapter
    {
        long GetIdPool();
        void Initialize(string _argument);
    }
}

