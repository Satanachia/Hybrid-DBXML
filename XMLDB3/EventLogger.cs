namespace XMLDB3
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Threading;

    public class EventLogger
    {
        private static readonly int ErrorEventLogFull = 0x5de;
        private static bool m_AutoClear = false;
        private static EventLog m_EventLog = new EventLog();
        private static bool m_Initialize = false;
        public static readonly int MaxMessageSize = 0x2000;

        private EventLogger()
        {
        }

        private static bool BackupEventLog()
        {
            string path = "./" + m_EventLog.Log + "_" + DateTime.Now.ToString().Replace(":", "-") + ".log";
            FileStream stream = null;
            try
            {
                if (!File.Exists(path))
                {
                    stream = File.Create(path);
                }
                else
                {
                    stream = File.Open(path, FileMode.Append, FileAccess.Write);
                }
                StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
                foreach (EventLogEntry entry in m_EventLog.Entries)
                {
                    writer.WriteLine("{0};{1};{2}", entry.TimeGenerated, entry.EntryType, entry.Message);
                }
                writer.Close();
                return true;
            }
            catch (Exception exception)
            {
                WriteErrorLog("Fail to backup event log", EventLogEntryType.Error, exception.ToString());
                return false;
            }
        }

        private static void Initialize()
        {
            string name = Assembly.GetEntryAssembly().GetName().Name;
            m_EventLog.Log = name;
            m_EventLog.Source = name;
            if (!EventLog.SourceExists(name))
            {
                EventLog.CreateEventSource(name, name);
            }
            m_Initialize = true;
        }

        public static void SetAutoClear(bool _auto)
        {
            lock (m_EventLog)
            {
                m_AutoClear = _auto;
            }
        }

        private static void WriteErrorLog(string _message, EventLogEntryType _type, string _error)
        {
            string path = "./EventLogError.log";
            FileStream stream = null;
            try
            {
                if (!File.Exists(path))
                {
                    stream = File.Create(path);
                }
                else
                {
                    stream = File.Open(path, FileMode.Append, FileAccess.Write);
                }
                StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
                writer.WriteLine("{0};{1};{2};{3}", new object[] { DateTime.Now, _type, _message, _error });
                writer.Close();
            }
            catch
            {
            }
        }

        public static void WriteEventLog(string _message)
        {
            WriteEventLog(_message, EventLogEntryType.Information);
        }

        public static void WriteEventLog(string _message, EventLogEntryType _type)
        {
            EventLog log;
            if (_message.Length > MaxMessageSize)
            {
                _message.Substring(0, MaxMessageSize);
            }
            Monitor.Enter(log = m_EventLog);
            try
            {
                if (!m_Initialize)
                {
                    Initialize();
                }
                m_EventLog.WriteEntry((_message.Length > MaxMessageSize) ? _message.Substring(0, MaxMessageSize) : _message, _type);
            }
            catch (Win32Exception exception)
            {
                if (((exception.NativeErrorCode == ErrorEventLogFull) && m_AutoClear) && BackupEventLog())
                {
                    m_EventLog.Clear();
                    WriteEventLog(_message, _type);
                }
                else
                {
                    WriteErrorLog(_message, _type, exception.ToString());
                }
            }
            catch (Exception exception2)
            {
                WriteErrorLog(_message, _type, exception2.ToString());
            }
            finally
            {
                Monitor.Exit(log);
            }
        }
    }
}

