namespace XMLDB3
{
    using System;

    public interface ChannelingKeyPoolAdapter
    {
        bool Do(ChannelingKey _chKey);
        void Initialize(string _argument);
    }
}

