namespace XMLDB3
{
    using System;

    public class LogInOutReportFileAdapter : FileAdapter, LogInOutReportAdapter
    {
        public void Initialize(string _argument)
        {
            base.Initialize(typeof(LogInOutReport), ConfigManager.GetFileDBPath("LogInOutReport"), ".xml");
        }

        public bool ReportLogInOut(LogInOutReport _report)
        {
            DateTime.Now.Ticks.ToString();
            return true;
        }
    }
}

