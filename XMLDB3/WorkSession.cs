namespace XMLDB3
{
    using System;
    using System.Collections;
    using System.Threading;

    public class WorkSession
    {
        private string m_Command = string.Empty;
        private string m_Name = string.Empty;
        private int m_NetworkSession = 0;
        private static Hashtable m_SessionTable = new Hashtable();
        private long m_StartTime = 0L;
        private string m_Status = string.Empty;
        private DateTime m_StatusDate = DateTime.MinValue;
        private Thread m_Thread = null;

        private WorkSession(string _name, string _command, int _networksession)
        {
            this.m_Thread = Thread.CurrentThread;
            if ((_name != null) && (_name != string.Empty))
            {
                this.m_Name = this.m_Thread.GetHashCode().ToString() + "_" + _name;
            }
            else
            {
                this.m_Name = this.m_Thread.GetHashCode().ToString();
            }
            if ((_command != null) && (_command != string.Empty))
            {
                this.m_Name = this.m_Name + "_" + _command;
                this.m_Command = _command;
            }
            this.m_NetworkSession = _networksession;
            this.m_StartTime = Stopwatch.GetTimestamp();
        }

        private void _End()
        {
            CommandStatistics.RegisterSessionTime(this.m_Command, Stopwatch.GetElapsedMilliseconds(this.m_StartTime));
        }

        public static bool Abort(string _name)
        {
            WorkSession session = FindSession(_name);
            if (session == null)
            {
                return false;
            }
            if (session.m_Thread.IsAlive)
            {
                session.m_Thread.Abort();
                if (session.m_Thread.Join(0x2710))
                {
                    m_SessionTable.Remove(session.m_Thread);
                    return true;
                }
                return false;
            }
            m_SessionTable.Remove(session.m_Thread);
            return true;
        }

        public static void Begin(string _name, string _command, int _networksession)
        {
            WorkSession session = new WorkSession(_name, _command, _networksession);
            lock (m_SessionTable.SyncRoot)
            {
                if (m_SessionTable.Contains(session.m_Thread))
                {
                    throw new Exception("already exists session in this thread");
                }
                m_SessionTable.Add(session.m_Thread, session);
            }
        }

        public static void End()
        {
            WorkSession currentSession = CurrentSession;
            if (currentSession != null)
            {
                currentSession._End();
            }
            lock (m_SessionTable.SyncRoot)
            {
                m_SessionTable.Remove(Thread.CurrentThread);
            }
        }

        private static WorkSession FindSession(string _name)
        {
            lock (m_SessionTable.SyncRoot)
            {
                foreach (WorkSession session in m_SessionTable.Values)
                {
                    if (session.m_Name == _name)
                    {
                        return session;
                    }
                }
                return null;
            }
        }

        public static string GetLastStatus()
        {
            WorkSession currentSession = CurrentSession;
            if (currentSession != null)
            {
                return ("[" + currentSession.m_Name + "]" + currentSession.m_Status);
            }
            return string.Empty;
        }

        public static void WriteStatus(string _status)
        {
            WorkSession currentSession = CurrentSession;
            if (currentSession != null)
            {
                currentSession.m_Status = _status;
                currentSession.m_StatusDate = DateTime.Now;
                Profiler.AddProfileString(string.Concat(new object[] { "[", currentSession.m_Name, "][", currentSession.m_StatusDate.ToString("T"), ".", currentSession.m_StatusDate.Millisecond, "]", currentSession.m_Status }));
            }
        }

        public static void WriteStatus(string _status, object _exData)
        {
            WriteStatus(_status, (_exData == null) ? null : _exData.ToString());
        }

        public static void WriteStatus(string _status, string _exData)
        {
            WriteStatus("[" + ((_exData == null) ? "N/A" : _exData) + "]" + _status);
        }

        private static WorkSession CurrentSession
        {
            get
            {
                lock (m_SessionTable.SyncRoot)
                {
                    return (WorkSession) m_SessionTable[Thread.CurrentThread];
                }
            }
        }

        public static SessionStatistics[] Statistics
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (WorkSession session in m_SessionTable.Values)
                {
                    SessionStatistics statistics = new SessionStatistics(session.m_Name, session.m_Status, session.m_StatusDate, session.m_NetworkSession);
                    list.Add(statistics);
                }
                return (SessionStatistics[]) list.ToArray(typeof(SessionStatistics));
            }
        }
    }
}

