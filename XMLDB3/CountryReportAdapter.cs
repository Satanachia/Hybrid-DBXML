namespace XMLDB3
{
    using System;

    public interface CountryReportAdapter
    {
        void Initialize(string _argument);
        bool ReportCCU(CountryReport _report);
    }
}

