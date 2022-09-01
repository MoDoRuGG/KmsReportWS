using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using NLog;

namespace KmsReportWS.Service
{
    public class AutoNotificationService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnStr = Settings.Default.ConnStr;
        
        private readonly EmailSender _emailSender = new EmailSender();

        public void SendChangeStatusNotification(Report_Flow flow)
        {
            try
            {
                string message;
                if (flow.Status == ReportStatus.Refuse.GetDescriptionSt())
                    message = "возвращен на доработку. ";
                else if (flow.Status == ReportStatus.Done.GetDescriptionSt())
                    message = "утвержден. ";
                else
                    return;

                string period = YymmUtils.ConvertYymmToText(flow.Yymm);
                string reportName = CollectReportName(flow.Id_Report_Type);
                string[] regions = {flow.Id_Region};
                var emails = CollectFilialEmails(regions);

                string theme = $"{reportName} за {period} {message}";
                string body = $"{reportName} за {period} {message}" + Environment.NewLine;

                _emailSender.Send(emails, theme, body);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error sending notification for change status");
            }
        }

        public void SendNewCommentNotification(int idReport, int idEmp, string comment)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var userName = db.Employee.SingleOrDefault(x => x.Region == "RU" && x.Id == idEmp);
            if (userName == null)
            {
                return;
            }

            var report = db.Report_Flow.Single(x => x.Id == idReport);

            string text = $"Пользователь: {userName.Surname} {userName.Name} {userName.MiddleName} - оставил новый комментарий отчету: " + Environment.NewLine +
                          comment + Environment.NewLine;
            string yymmReport = YymmUtils.ConvertYymmToText(report.Yymm);
            string reportName = CollectReportName(report.Id_Report_Type);
            string theme = $"{reportName} за {yymmReport} добавлен новый комментарий. ";
            string[] regions = {report.Id_Region};
            var emails = CollectFilialEmails(regions);

            _emailSender.Send(emails, theme, text);
        }

        private List<string> CollectFilialEmails(string[] regions)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            return db.Employee.Where(x => regions.Contains(x.Region) && x.IsActive).Select(x => x.Email).ToList();
        }

        private string CollectReportName(string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            return db.Report_Types.Single(x => x.Id == reportType).Name.Trim();
        }
    }
}