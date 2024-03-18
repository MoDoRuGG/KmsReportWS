using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Service;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Service
{
    public class CommentService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly AutoNotificationService _notificationService = new AutoNotificationService();

        public bool IsReportHasComments(int idReport)
        {
            if (idReport == 0)
            {
                return false;
            }

            try
            {
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                return db.Comment.Any(x => x.Id_Flow == idReport);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error getting comment for Report Id = {idReport}");
                throw;
            }
        }

        public List<ReportComment> GetComments(int idReport, string filialCode)
        {
            try
            {
                Log.Debug($"Get comments for idReport = {idReport}");
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var commentsList = new List<ReportComment>();

                var comments = db.Comment.Where(x => x.Id_Flow == idReport);
                foreach (var com in comments)
                {
                    var emp = com.Employee;
                    if (emp.Region != filialCode)
                    {
                        com.Date_read = DateTime.Today;
                    }

                    var name = $"{emp.Surname} {emp.Name} {emp.MiddleName}";
                    var comment = new ReportComment { Name = name, Comment = com.Comment1, DateIns = com.Date_ins };
                    commentsList.Add(comment);
                }
                db.SubmitChanges();
                return commentsList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error getting comment for Report Id = {idReport}");
                throw;
            }
        }

        public List<UnreadComment> GetReportsWithUnreadComments(string filialCode)
        {
            try
            {
                Log.Debug($"Get reports with unread comments for region = {filialCode}");
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);

                var unreadComments = from com in db.Comment
                                     join user in db.Employee on com.Id_Employee equals user.Id
                                     join flow in db.Report_Flow on com.Id_Flow equals flow.Id
                                     where com.Date_read == null
                                     select new { com, user, flow };
                unreadComments = filialCode == "RU"
                    ? unreadComments.Where(r => r.user.Region != "RU")
                    : unreadComments.Where(r => r.flow.Id_Region == filialCode && r.user.Region == "RU");

                return unreadComments.Select(r => new UnreadComment
                {
                    IdFlow = r.flow.Id,
                    Yymm = r.flow.Yymm,
                    IdRegion = r.flow.Id_Region
                }).Distinct().ToList();               
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error reports with unread comments for region = {filialCode}");
                throw;
            }
        }
        
        public void AddComment(int idReport, int idEmp, string comment)
        {
            try
            {
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var com = new Comment
                {
                    Comment1 = comment,
                    Id_Flow = idReport,
                    Id_Employee = idEmp,
                    Date_ins = DateTime.Today
                };
                db.Comment.InsertOnSubmit(com);
                db.SubmitChanges();
                 
                _notificationService.SendNewCommentNotification(idReport, idEmp, comment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Add comment for Report id = {idReport} text = {comment} ");
                throw;
            }
        }
    }
}