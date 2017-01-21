namespace XMLDB3
{
    using System;

    public class MemoEmptyAdapter : MemoAdapter
    {
        public void Initialize(string _Argument)
        {
        }

        public bool SendMemo(Memo _memo)
        {
            return false;
        }
    }
}

