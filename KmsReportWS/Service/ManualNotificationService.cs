using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Model.Service;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using NLog;
using Employee = KmsReportWS.Model.Service.Employee;

namespace KmsReportWS.Service
{
    public class ManualNotificationService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly string _connStr = Settings.Default.ConnStr;
        private readonly EmailSender _emailSender = new EmailSender();
        private readonly Dictionary<string, string> _regions;
        private readonly Dictionary<string, string> _reportTypes;

        public ManualNotificationService()
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            _regions = db.Region.ToDictionary(x => x.id, xn => xn.name);
            _reportTypes = db.Report_Type.ToDictionary(x => x.Id, xn => xn.Name);
        }

        public string SendNotification(NotificationRequest request)
        {
            Log.Info($"Start manual notification with request: {request}");
            try
            {
                var emails = CollectNotificationEmails(request);
                if (!emails.Any())
                {
                    var errorMessage = "Сервер вернул пустой список для отправки уведомлений. Это связано с тем, что ";
                    errorMessage += request.IsRefuse
                        ? "у выбранных филиалов сданы данные отчеты. Для отправки уведомления, верните их на доработку."
                        : "нет филиалов попадающих под данные критерии выборки.";
                    Log.Error(errorMessage);
                    return errorMessage;
                }

                string result = SendNotification(emails, request.Theme, request.Theme);
                Log.Info(result);

                SendMailForCurrentUser(emails, request);
                Log.Info("Successful end of notifications");
                return result;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error in manual notification");
                throw;
            }
        }

        private string SendNotification(List<Employee> emails, string theme, string text)
        {
            var notifications = (from e in emails
                                 group e by e.Region
                into eGr
                                 select new { Filial = eGr.Key, Emails = eGr.Select(x => x.Email) }).ToList();

            var err = 0;
            foreach (var notification in notifications)
            {
                Log.Debug($"Send emails. Region = {notification.Filial} Emails = {notification.Emails}");
                try
                {
                    _emailSender.Send(notification.Emails.ToList(), theme, text);
                }
                catch (Exception ex)
                {
                    err++;
                    Log.Error(ex, $"Error sending emails to {notification.Emails}");
                }
            }

            return $"Отправка уведомлений завершена. Успешно отправлено: {notifications.Count}. С ошибками {err}";
        }

        private void SendMailForCurrentUser(List<Employee> users, NotificationRequest request)
        {
            string period = YymmUtils.ConvertYymmToMonth(request.Yymm) + "20" + request.Yymm.Substring(2);
            var theme = $"Нотификация: {_reportTypes[request.ReportType]} за {period} года";
            var mail =
                "Через форму нотификаций были отправлены уведомления.\r\n" +
                $"Тема уведомления: {request.Theme}\r\n" +
                $"Текст уведомления: {request.Text}\r\n" +
                "Адреса рассылки: \r\n";
            var notificationRegions = users.Select(x => x.Region).OrderBy(x => x).Distinct().ToList();
            foreach (var region in notificationRegions)
            {
                var regionName = _regions.Single(r => r.Key == region).Value;
                mail += $"  Код региона: {region}, наименование региона: {regionName} " + Environment.NewLine;

                foreach (var user in users.Where(x => x.Region == region))
                {
                    var fio = $"{user.Surname.Trim()} {user.Name.Trim()} {user.MiddleName.Trim()}";
                    mail += $"    ФИО: {fio}, телефон: {user.Phone}, e-mail: {user.Email} " + Environment.NewLine;
                }
            }

            mail += Environment.NewLine + $"Всего отправлено уведомлений: {notificationRegions.Count}" +
                    Environment.NewLine;

            var currentEmailList = new List<string> { request.CurrentEmail };
            _emailSender.Send(currentEmailList, theme, mail);
            Log.Debug($"Письмо отправлено сотруднику ЦО на адрес {request.CurrentEmail}. Текст: {mail}");
        }

        private List<Employee> CollectNotificationEmails(NotificationRequest request)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var notExistsReports = (from filial in request.Filials
                                    let hasReport = db.Report_Flow.Any(
                                        x => x.Yymm == request.Yymm && x.Id_Report_Type == request.ReportType && x.Id_Region == filial)
                                    where !hasReport
                                    select filial).ToList();

            var flowsNotExisting = db.Employee.Where(x => notExistsReports.Contains(x.Region) && x.IsActive)
                .Select(emp => new Employee
                {
                    Surname = emp.Surname,
                    Name = emp.Name,
                    MiddleName = emp.MiddleName,
                    Email = emp.Email,
                    Phone = emp.Phone,
                    Region = emp.Region
                }).ToList();

            var flowsExistingReport = from emp in db.Employee
                                      join flow in db.Report_Flow on emp.Region equals flow.Id_Region
                                      where flow.Id_Report_Type == request.ReportType && flow.Yymm == request.Yymm &&
                                            flow.Status != ReportStatus.Done.GetDescriptionSt() && emp.IsActive
                                      select new Employee
                                      {
                                          Surname = emp.Surname,
                                          Name = emp.Name,
                                          MiddleName = emp.MiddleName,
                                          Email = emp.Email,
                                          Phone = emp.Phone,
                                          Region = emp.Region,
                                          Status = flow.Status
                                      };
            if (request.IsRefuse)
            {
                flowsExistingReport = flowsExistingReport.Where(x => x.Status == ReportStatus.Refuse.GetDescriptionSt());
            }

            if (request.Filials?.Length > 0)
            {
                flowsExistingReport = flowsExistingReport.Where(x => request.Filials.Contains(x.Region));
            }

            return flowsExistingReport.ToList().Union(flowsNotExisting).Distinct().ToList();
        }
    }
}