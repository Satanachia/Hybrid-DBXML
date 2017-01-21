namespace XMLDB3
{
    using System;

    public interface MemoAdapter
    {
        void Initialize(string _Argument);
        bool SendMemo(Memo _memo);
    }
}

