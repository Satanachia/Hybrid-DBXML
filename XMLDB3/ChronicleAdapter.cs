namespace XMLDB3
{
    using System;

    public interface ChronicleAdapter
    {
        bool Create(string _characterName, Chronicle _chronicle);
        void Initialize(string _Argument);
        bool UpdateChronicleInfoList(ChronicleInfoList _list);
    }
}

