namespace XMLDB3
{
    using System;
    using System.Collections;

    public class CommandQueue
    {
        private Queue m_CmdQueue = new Queue();

        public BasicCommand Pop()
        {
            lock (this.m_CmdQueue.SyncRoot)
            {
                if (this.m_CmdQueue.Count > 0)
                {
                    return (BasicCommand) this.m_CmdQueue.Dequeue();
                }
                return null;
            }
        }

        public void Push(BasicCommand _cmd)
        {
            lock (this.m_CmdQueue.SyncRoot)
            {
                this.m_CmdQueue.Enqueue(_cmd);
            }
        }

        public CacheStatistics Statistics
        {
            get
            {
                lock (this.m_CmdQueue.SyncRoot)
                {
                    return new CacheStatistics("CommandQueue", this.m_CmdQueue.Count);
                }
            }
        }
    }
}

