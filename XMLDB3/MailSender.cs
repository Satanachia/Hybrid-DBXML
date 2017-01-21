namespace XMLDB3
{
    using System;
    using System.Text;
    using System.Web.Mail;

    public class MailSender
    {
        private const string defaultTitle = "DB 서버에서 오류 발생";
        private const string header = "[mabi/dbserver/report]";

        public static void Send(string _html)
        {
            Send("DB 서버에서 오류 발생", _html);
        }

        public static void Send(string _title, string _html)
        {
            if (((ConfigManager.ReportServer != null) && (ConfigManager.ReportReceiver != null)) && (ConfigManager.ReportSender != null))
            {
                MailMessage message = new MailMessage();
                message.To = ConfigManager.ReportReceiver;
                message.From = ConfigManager.ReportSender;
                message.Subject = "[mabi/dbserver/report] " + _title;
                message.Body = _html;
                message.BodyFormat = MailFormat.Html;
                message.BodyEncoding = Encoding.Unicode;
                try
                {
                    SmtpMail.SmtpServer = ConfigManager.ReportServer;
                    SmtpMail.Send(message);
                }
                catch (Exception exception)
                {
                    ExceptionMonitor.ExceptionRaised(exception);
                }
            }
        }
    }
}

