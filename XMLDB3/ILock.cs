namespace XMLDB3
{
    using System;

    public interface ILock
    {
        void Close();
        void ForceUnregist();
        void Wait();

        IObjLockRegistHelper BeginHelper { get; }

        IObjLockRegistHelper EndHelper { get; }
    }
}

