namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class FamilyListSerializer
    {
        public static void Deserialize(FamilyList _list, Message _message)
        {
            if (((_list == null) || (_list.family == null)) || (_list.family.Length == 0))
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_list.family.Length);
                foreach (FamilyListFamily family in _list.family)
                {
                    FamilySerializer.Deserialize(family, _message);
                }
            }
        }
    }
}

