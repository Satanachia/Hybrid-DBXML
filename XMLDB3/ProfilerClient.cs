namespace XMLDB3
{
    using Mabinogi;
    using Mabinogi.Network;
    using System;

    public class ProfilerClient
    {
        public int ID;
        private bool m_Running;
        private int m_SendCount;
        private const int MaxProfileCount = 0x2710;

        public ProfilerClient(int _id)
        {
            this.ID = _id;
            this.m_Running = true;
            this.m_SendCount = 0;
        }

        public int ResetSendCount()
        {
            int num = 0;
            lock (this)
            {
                if (this.m_SendCount > 0x2710)
                {
                    num = this.m_SendCount - 0x2710;
                }
                this.m_SendCount = 0;
                this.m_Running = true;
            }
            return num;
        }

        public void SendProfile(ServerHandler _handler, Message _msg)
        {
            if (this.m_Running)
            {
                _handler.SendMessage(this.ID, _msg);
            }
            lock (this)
            {
                this.m_SendCount++;
                if (this.m_Running && (this.m_SendCount >= 0x2710))
                {
                    this.m_Running = false;
                }
            }
        }
    }
}

