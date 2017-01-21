namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CountryReportSerializer
    {
        public static void Deserialize(CountryReport _report, Message _message)
        {
        }

        public static CountryReport Serialize(Message _message)
        {
            CountryReport report = new CountryReport();
            report.reportstring = _message.ReadString();
            return report;
        }
    }
}

