namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class SessionStatistics
    {
        private DateTime m_CurrentTIme = DateTime.Now;
        private string m_Name = string.Empty;
        private int m_NetworkSession = 0;
        private string m_Status = string.Empty;
        private DateTime m_StatusTime = DateTime.MinValue;

        public SessionStatistics(string _name, string _status, DateTime _status_date, int _networksession)
        {
            this.m_Name = _name;
            this.m_Status = _status;
            this.m_StatusTime = _status_date;
            this.m_NetworkSession = _networksession;
        }

        public Message FromMessage(Message _input)
        {
            this.m_Name = _input.ReadString();
            this.m_Status = _input.ReadString();
            this.m_StatusTime = new DateTime(_input.ReadS64());
            this.m_NetworkSession = _input.ReadS32();
            this.m_CurrentTIme = new DateTime(_input.ReadS64());
            return _input;
        }

        public Message ToMessage()
        {
            Message message = new Message(0, 0L);
            message.WriteString(this.m_Name);
            message.WriteString(this.m_Status);
            message.WriteS64(this.m_StatusTime.Ticks);
            message.WriteS32(this.m_NetworkSession);
            message.WriteS64(this.m_CurrentTIme.Ticks);
            return message;
        }

        public DateTime CurrentTime
        {
            get
            {
                return this.m_CurrentTIme;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public int NetworkSession
        {
            get
            {
                return this.m_NetworkSession;
            }
        }

        public string Status
        {
            get
            {
                return this.m_Status;
            }
        }

        public DateTime StatusTime
        {
            get
            {
                return this.m_StatusTime;
            }
        }
    }
}

