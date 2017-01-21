namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class RoyalAlchemistListSerializer
    {
        public static void Deserialize(RoyalAlchemistList _list, Message _message)
        {
            if (((_list == null) || (_list.alchemists == null)) || (_list.alchemists.Length == 0))
            {
                _message.WriteS32(0);
            }
            else
            {
                _message.WriteS32(_list.alchemists.Length);
                foreach (RoyalAlchemist alchemist in _list.alchemists)
                {
                    RoyalAlchemistSerializer.Deserialize(alchemist, _message);
                }
            }
        }
    }
}

