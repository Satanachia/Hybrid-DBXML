namespace XMLDB3
{
    using System;

    public interface HuskyAdapter
    {
        bool Callprocedure(string _account, long _charId, string _charName);
        void Initialize(string _argument);
    }
}

