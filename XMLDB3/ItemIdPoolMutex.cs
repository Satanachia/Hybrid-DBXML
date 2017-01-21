namespace XMLDB3
{
    using System;
    using System.Threading;

    public class ItemIdPoolMutex
    {
        private static Mutex idpoolmutex = new Mutex();

        public static void Enter()
        {
            idpoolmutex.WaitOne();
        }

        public static void Leave()
        {
            idpoolmutex.ReleaseMutex();
        }
    }
}

