namespace XMLDB3
{
    using System;

    public interface ISerializableCommand
    {
        void OnSerialize(IObjLockRegistHelper _helper, bool bBegin);
    }
}

