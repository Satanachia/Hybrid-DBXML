namespace XMLDB3
{
    using System;

    public class CountryReportFileAdapter : FileAdapter, CountryReportAdapter
    {
        public void Initialize(string _argument)
        {
            base.Initialize(typeof(CountryReport), ConfigManager.GetFileDBPath("CountryReport"), ".xml");
        }

        public bool ReportCCU(CountryReport _report)
        {
            _report.reportstring.Substring(0, 12);
            return true;
        }
    }
}

