namespace XMLDB3
{
    using Mabinogi;
    using System;
    using System.Collections;
    using System.Data.SqlClient;
    using System.Diagnostics;

    public class ExceptionMonitor
    {
        private static ArrayList exceptionList = new ArrayList();

        public static void Clear()
        {
            lock (exceptionList)
            {
                exceptionList.Clear();
                exceptionList.TrimToSize();
            }
        }

        public static void ExceptionRaised(SqlException _ex)
        {
            ExceptionRaised((Exception) _ex, MakeSqlExceptionInfo(_ex));
        }

        public static void ExceptionRaised(Exception _ex)
        {
            ExceptionRaised(_ex, (string) null);
        }

        private static void ExceptionRaised(UserException _ex)
        {
            lock (exceptionList.SyncRoot)
            {
                exceptionList.Insert(0, _ex);
            }
            if (Console.Error != null)
            {
                Console.Error.WriteLine(_ex.FullMessage.ToString());
            }
            EventLogger.WriteEventLog(_ex.FullMessage, EventLogEntryType.Error);
        }

        public static void ExceptionRaised(SqlException _ex, object _obj)
        {
            ExceptionRaised(_ex, (_obj == null) ? null : _obj.ToString());
        }

        public static void ExceptionRaised(SqlException _ex, string _msg1)
        {
            ExceptionRaised((Exception) _ex, MakeSqlExceptionInfo(_ex), _msg1);
        }

        public static void ExceptionRaised(Exception _ex, object _obj)
        {
            ExceptionRaised(_ex, (_obj == null) ? null : _obj.ToString());
        }

        public static void ExceptionRaised(Exception _ex, string _msg)
        {
            UserException exception = new UserException(_ex, _msg);
            ExceptionRaised(exception);
        }

        public static void ExceptionRaised(SqlException _ex, object _obj1, object _obj2)
        {
            ExceptionRaised(_ex, ((_obj1 == null) ? "N/A" : _obj1.ToString()) + "][" + ((_obj2 == null) ? "N/A" : _obj2.ToString()));
        }

        public static void ExceptionRaised(SqlException _ex, string _msg1, string _msg2)
        {
            ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2));
        }

        public static void ExceptionRaised(Exception _ex, object _msg1, object _msg2)
        {
            ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1.ToString()) + "][" + ((_msg2 == null) ? "N/A" : _msg2.ToString()));
        }

        public static void ExceptionRaised(Exception _ex, string _msg1, string _msg2)
        {
            ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2));
        }

        public static void ExceptionRaised(SqlException _ex, object _obj1, object _obj2, object _obj3)
        {
            ExceptionRaised(_ex, ((_obj1 == null) ? "N/A" : _obj1.ToString()) + "][" + ((_obj2 == null) ? "N/A" : _obj2.ToString()) + "][" + ((_obj3 == null) ? "N/A" : _obj3.ToString()));
        }

        public static void ExceptionRaised(SqlException _ex, string _msg1, string _msg2, string _msg3)
        {
            ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2) + "][" + ((_msg3 == null) ? "N/A" : _msg3));
        }

        public static void ExceptionRaised(Exception _ex, object _msg1, object _msg2, object _msg3)
        {
            ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1.ToString()) + "][" + ((_msg2 == null) ? "N/A" : _msg2.ToString()) + "][" + ((_msg3 == null) ? "N/A" : _msg3.ToString()));
        }

        public static void ExceptionRaised(Exception _ex, string _msg1, string _msg2, string _msg3)
        {
            ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2) + "][" + ((_msg3 == null) ? "N/A" : _msg3));
        }

        private static string MakeSqlExceptionInfo(SqlException _ex)
        {
            return (_ex.Number.ToString() + ":" + ((_ex.Procedure == null) ? "N/A" : _ex.Procedure) + ":" + _ex.LineNumber.ToString());
        }

        public static Message ToMessage(int _startIdx, int _count)
        {
            Message message = new Message(0, 0L);
            lock (exceptionList.SyncRoot)
            {
                int num = ((_startIdx + _count) <= exceptionList.Count) ? _count : (exceptionList.Count - _startIdx);
                message.WriteS32(exceptionList.Count);
                if (num > 0)
                {
                    message.WriteS32(num);
                    for (int i = _startIdx; i < (_startIdx + num); i++)
                    {
                        UserException exception = (UserException) exceptionList[i];
                        message.WriteS64(exception.RaisedTime.Ticks);
                        message.WriteString(exception.Message);
                        message.WriteString(exception.FullMessage);
                        message.WriteString(exception.LastStatus);
                    }
                    return message;
                }
                message.WriteS32(0);
            }
            return message;
        }

        public class UserException
        {
            private string exMessage;
            private string fullExMessage;
            private string lastStatus;
            private DateTime raisedTime;
            private string userData;

            public UserException(Exception _ex, string _msg)
            {
                this.exMessage = _ex.Message;
                this.userData = _msg;
                this.raisedTime = DateTime.Now;
                this.lastStatus = WorkSession.GetLastStatus();
                this.fullExMessage = "[" + ((this.userData != null) ? this.userData : "N/A") + "]\r\n" + _ex.ToString();
            }

            public string FullMessage
            {
                get
                {
                    return this.fullExMessage;
                }
            }

            public string LastStatus
            {
                get
                {
                    return this.lastStatus;
                }
            }

            public string Message
            {
                get
                {
                    return this.exMessage;
                }
            }

            public DateTime RaisedTime
            {
                get
                {
                    return this.raisedTime;
                }
            }
        }
    }
}

