namespace XMLDB3
{
    using Mabinogi;
    using System;

    public class LogInOutReportCommand : BasicCommand
    {
        private LogInOutReport m_LogInOutReport = null;
        private bool m_Result = false;

        public override bool DoProcess()
        {
            WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 국가별 접속 정보를 기록합니다");
            this.m_Result = QueryManager.LogInOutReport.ReportLogInOut(this.m_LogInOutReport);
            if (this.m_Result)
            {
                WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 국가별 접속 정보를 기록하였습니다");
            }
            else
            {
                WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 국가별 접속 정보 기록에 실패하였습니다");
            }
            return this.m_Result;
        }

        public override Message MakeMessage()
        {
            WorkSession.WriteStatus("LogInOutReportCommand.MakeMessage() : 함수에 진입하였습니다");
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
            this.m_LogInOutReport = LogInOutReportSerializer.Serialize(_Msg);
        }
    }
}

