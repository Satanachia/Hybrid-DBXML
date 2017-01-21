namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class PropListSerializer
    {
        public static void Deserialize(PropIDList _list, Message _message)
        {
            if (_list == null)
            {
                _list = new PropIDList();
            }
            if (_list.propID != null)
            {
                _message.WriteS32(_list.propID.Length);
                foreach (long num in _list.propID)
                {
                    _message.WriteS64(num);
                }
            }
            else
            {
                _message.WriteS32(0);
            }
        }
    }
}

