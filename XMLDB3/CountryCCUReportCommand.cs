namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class CountryCCUReportCommand : BasicCommand
    {
        private CountryReport m_CountryReport = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 국가별 동접 정보를 기록합니다");
            this.m_Result = QueryManager.CountryReport.ReportCCU(this.m_CountryReport);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 국가별 동접 정보를 기록하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 국가별 동접 정보 기록에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("CountryCCUReportCommand.MakeMessage() : 함수에 진입하였습니다");
            Message message = new Message(base.ID, 0L);
            message.WriteU32(base.QueryID);
            if (this.m_Result)
            {
                message.WriteU8(1);
                return message;
            }
            message.WriteU8(0);
            return message;
        }

        protected override void ReceiveData(Message _Msg)
        {
            this.m_CountryReport = CountryReportSerializer.Serialize(_Msg);
        }
    }
}

