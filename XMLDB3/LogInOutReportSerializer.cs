namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class LogInOutReportSerializer
    {
        public static void Deserialize(LogInOutReport _report, Message _message)
        {
        }

        public static LogInOutReport Serialize(Message _message)
        {
            LogInOutReport report = new LogInOutReport();
            report.inout = _message.ReadString();
            report.account = _message.ReadString();
            report.ip = _message.ReadString();
            report.countrycode = _message.ReadString();
            return report;
        }
    }
}

