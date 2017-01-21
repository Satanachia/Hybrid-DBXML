namespace XMLDB3
{
    using System;

    public interface ISection : ICacheItem
    {
        object[] ToArray();
        Array ToArray(Type type);

        int Count { get; }

        ILinkItem First { get; }
    }
}

