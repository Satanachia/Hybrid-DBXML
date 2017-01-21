namespace XMLDB3
{
    using System;

    public interface GuildIdPoolAdapter
    {
        long GetIdPool();
        void Initialize(string _argument);
    }
}

