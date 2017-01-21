namespace XMLDB3
{
    using System;

    public interface LogInOutReportAdapter
    {
        void Initialize(string _argument);
        bool ReportLogInOut(LogInOutReport _report);
    }
}

