namespace XMLDB3
{
    using System;

    public class HuskyFileAdapter : HuskyAdapter
    {
        public bool Callprocedure(string _account, long _charId, string _charName)
        {
            Console.WriteLine("HuskyFileAdapter.Callprocedure(" + _account + ", " + _charId.ToString() + ", " + _charName + ")");
            return true;
        }

        public void Initialize(string _argument)
        {
        }
    }
}

