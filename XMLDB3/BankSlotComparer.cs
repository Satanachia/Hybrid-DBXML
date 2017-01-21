namespace XMLDB3
{
    using System;
    using System.Collections;

    public class BankSlotComparer : IComparer
    {
        private IComparer internalComparer = new CaseInsensitiveComparer();

        int IComparer.Compare(object x, object y)
        {
            return this.internalComparer.Compare(((BankSlot) x).Name, ((BankSlot) y).Name);
        }
    }
}

