using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Service;
using KmsReportWS.Support;
using NLog;

namespace KmsReportWS.Handler
{
    public class ReportFlowHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly AutoNotificationService _notificationService = new AutoNotificationService();

        public List<ReportFlowDto> GetReportFlows(string filial, string yymmStart, string yymmEnd)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flows = from r in db.Report_Flow
                        join rt in db.Report_Type on r.Id_Report_Type equals rt.Id
                        //join reg in db.Region on flow.Id_Region equals reg.id
                        where Convert.ToInt32(r.Yymm) >= Convert.ToInt32(yymmStart)
                              && Convert.ToInt32(r.Yymm) <= Convert.ToInt32(yymmEnd)
                        select new ReportFlowDto
                        {
                            IdRegion = r.Id_Region,
                            IdReport = r.Id_Report_Type,
                            Scan = r.Scan,
                            Yymm = r.Yymm,
                            DateEditCo = r.Date_edit_co,
                            DateIsDone = r.Date_is_done,
                            DateToCo = r.Date_to_co,
                            DataSource = DataSourseUtils.ParseDataSource(r.DataSource),
                            Status = StatusUtils.ParseStatus(r.Status)

                        };
            if (!string.IsNullOrEmpty(filial))
            {
                flows = flows.Where(x => x.IdRegion == filial);
            }

            return flows.ToList();
        }


        public List<ReportScanModel> GetScans(int idReport)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            List<ReportScanModel> scans = new List<ReportScanModel>();
            try
            {
                scans = db.Scans_Base.Where(x => x.Id_flow == idReport).Select(x => new ReportScanModel
                {
                    IdScan = x.Id_Scan,
                    FileName = x.File_Name,
                    DateAdded = x.Dte_added,
                    UserAdded = $"{x.Employee.Surname} {x.Employee.Name} {x.Employee.MiddleName}",
                    DateUpdated = x.Dte_update,
                    UserUpdate = $"{x.Employee1.Surname} {x.Employee1.Name} {x.Employee1.MiddleName}",
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error GetScan: idReport = {idReport}");
                throw;
            }

            return scans;

        }

        public void DeleteScan(int idReport, int idUser, int num)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            try
            {
                var flow = db.Report_Flow.Single(x => x.Id == idReport);
                if (num == 1)
                {
                    flow.Scan = null;
                }

                else if (num == 2)
                {
                    flow.Scan2 = null;
                }

                else if (num == 3)
                {
                    flow.Scan3 = null;
                }

                else if (num == 4)
                {
                    flow.Scan4 = null;
                }

                else if (num == 5)
                {
                    flow.Scan5 = null;
                }

                else if (num == 6)
                {
                    flow.Scan6 = null;
                }

                else if (num == 7)
                {
                    flow.Scan7 = null;
                }

                else if (num == 8)
                {
                    flow.Scan8 = null;
                }

                else if (num == 9)
                {
                    flow.Scan9 = null;
                }

                else if (num == 10)
                {
                    flow.Scan10 = null;
                }

                if (String.IsNullOrEmpty(flow.Scan) && String.IsNullOrEmpty(flow.Scan2) && String.IsNullOrEmpty(flow.Scan3) && String.IsNullOrEmpty(flow.Scan4)
                 && String.IsNullOrEmpty(flow.Scan5)
                  && String.IsNullOrEmpty(flow.Scan6)
                   && String.IsNullOrEmpty(flow.Scan7)
                    && String.IsNullOrEmpty(flow.Scan8)
                     && String.IsNullOrEmpty(flow.Scan9)
                      && String.IsNullOrEmpty(flow.Scan10))
                {
                    flow.Status = ReportStatus.Saved.GetDescriptionSt();

                }
                flow.Updated = DateTime.Today.Date;
                flow.Id_Employee_Upd = idUser;
                db.SubmitChanges();

            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error saving scan: idReport = {idReport}");
                throw;
            }

        }

        public void SaveScanToDb(int idReport, int idUser, string uri, int num)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            if (idReport <= 0)
            {
                Log.Error($"Error saving scan: idReport = {idReport}, idUser = {idUser}");
                throw new Exception($"IdReport must be more than 0. idReport = {idReport}");
            }

            try
            {
                var flow = db.Report_Flow.Single(x => x.Id == idReport);
                //if (string.IsNullOrEmpty(flow.Scan))
                //{
                //    flow.Scan = uri;
                //}

                //if (!string.IsNullOrEmpty(flow.Scan) && string.IsNullOrEmpty(flow.Scan2))
                //{
                //    flow.Scan2 = uri;
                //}

                //if (!string.IsNullOrEmpty(flow.Scan) && !string.IsNullOrEmpty(flow.Scan2) && string.IsNullOrEmpty(flow.Scan3))
                //{
                //    flow.Scan3 = uri;
                //}

                if (num == 1)
                {
                    flow.Scan = uri;
                }

                if (num == 2)
                {
                    flow.Scan2 = uri;
                }

                if (num == 3)
                {
                    flow.Scan3 = uri;
                }

                if (num == 4)
                {
                    flow.Scan4 = uri;
                }

                if (num == 5)
                {
                    flow.Scan5 = uri;
                }

                if (num == 6)
                {
                    flow.Scan6 = uri;
                }

                if (num == 7)
                {
                    flow.Scan7 = uri;
                }

                if (num == 8)
                {
                    flow.Scan8 = uri;
                }

                if (num == 9)
                {
                    flow.Scan9 = uri;
                }

                if (num == 10)
                {
                    flow.Scan10 = uri;
                }

                flow.Status = ReportStatus.Scan.GetDescriptionSt();
                flow.Updated = DateTime.Today.Date;
                flow.Id_Employee_Upd = idUser;
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error saving scan: idReport = {idReport}, idUser = {idUser}");
                throw;
            }
        }

        public void SaveScanToDb2(int idReport, int idUser, string uri)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            if (idReport <= 0)
            {
                Log.Error($"Error saving scan: idReport = {idReport}, idUser = {idUser}");
                throw new Exception($"IdReport must be more than 0. idReport = {idReport}");
            }

            try
            {
                var flow = db.Report_Flow.Single(x => x.Id == idReport);
                flow.Scan = uri;
                flow.Status = ReportStatus.Scan.GetDescriptionSt();
                flow.Updated = DateTime.Today.Date;
                flow.Id_Employee_Upd = idUser;
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error saving scan: idReport = {idReport}, idUser = {idUser}");
                throw;
            }
        }

        public void SaveScanToDb3(int idReport, int idUser, string uri, int num)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            if (idReport <= 0)
            {
                Log.Error($"Error saving scan: idReport = {idReport}, idUser = {idUser}");
                throw new Exception($"IdReport must be more than 0. idReport = {idReport}");
            }

            try
            {
                var flow = db.Report_Flow.Single(x => x.Id == idReport);
                switch (num)
                {
                    case 1:
                    {
                        flow.Scan = uri;
                        break;
                    }
                    case 2:
                    {
                        flow.Scan2 = uri;
                        break;
                    }
                    case 3:
                    {
                        flow.Scan3 = uri;
                        break;
                    }
                    case 4:
                    {
                        flow.Scan4 = uri;
                        break;
                    }
                    case 5:
                    {
                        flow.Scan5 = uri;
                        break;
                    }
                    case 6:
                    {
                        flow.Scan6 = uri;
                        break;
                    }
                    case 7:
                    {
                        flow.Scan7 = uri;
                        break;
                    }
                    case 8:
                    {
                        flow.Scan8 = uri;
                        break;
                    }
                    case 9:
                    {
                        flow.Scan9 = uri;
                        break;
                    }
                    case 10:
                    {
                        flow.Scan10 = uri;
                        break;
                    }
                }

                flow.Status = ReportStatus.Scan.GetDescriptionSt();
                flow.Updated = DateTime.Today.Date;
                flow.Id_Employee_Upd = idUser;
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error saving scan: idReport = {idReport}, idUser = {idUser}");
                throw;
            }
        }

        public void ChangeStatus(int idReport, int idUser, ReportStatus status)
        {
            if (idReport <= 0)
            {
                Log.Error($"Error changing status: idReport = {idReport}, idUser = {idUser}");
                throw new Exception($"IdReport must be more than 0. idReport = {idReport}");
            }

            try
            {
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flow = db.Report_Flow.Single(x => x.Id == idReport);
                flow.Status = status.GetDescriptionSt();
                switch (status)
                {
                    case ReportStatus.Submit:
                        flow.Date_to_co = DateTime.Today.Date;
                        flow.User_to_co = idUser;
                        break;
                    case ReportStatus.Refuse:
                        flow.Date_edit_co = DateTime.Today.Date;
                        flow.User_edit_co = idUser;
                        break;
                    case ReportStatus.Done:
                        flow.Date_is_done = DateTime.Today.Date;
                        flow.User_submit = idUser;
                        break;
                }

                db.SubmitChanges();
                _notificationService.SendChangeStatusNotification(flow);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error changing status = {status}: idReport = {idReport}, idUser = {idUser}");
                throw;
            }
        }


        public void ChangeDataSource(int idReport, int idUser, DataSource datasource)
        {
            if (idReport <= 0)
            {
                Log.Error($"Error changing datasource: idReport = {idReport}, idUser = {idUser}");
                throw new Exception($"IdReport must be more than 0. idReport = {idReport}");
            }

            try
            {
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flow = db.Report_Flow.Single(x => x.Id == idReport);
                flow.DataSource = datasource.GetDescriptionDS();
                switch (datasource)
                {
                    case DataSource.New:
                        flow.Date_to_co = DateTime.Today.Date;
                        flow.User_to_co = idUser;
                        break;
                    case DataSource.Excel:
                        flow.Date_edit_co = DateTime.Today.Date;
                        flow.User_edit_co = idUser;
                        break;
                    case DataSource.Handle:
                        flow.Date_is_done = DateTime.Today.Date;
                        flow.User_submit = idUser;
                        break;
                }

                db.SubmitChanges();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error changing datasourse = {datasource}: idReport = {idReport}, idUser = {idUser}");
                throw;
            }
        }

        private bool IsFullyApproved(LinqToSqlKmsReportDataContext db, int reportFlowId, string reportType)
        {
            // Получаем требуемые Direction для этого отчёта
            var requiredDirections = db.Approval_Scheme
                .Where(s => s.Report_Type == reportType)
                .Select(s => s.Required_Direction)
                .ToList();

            if (!requiredDirections.Any())
                return true; // нет схемы → старая логика

            // Получаем Direction тех, кто уже утвердил
            var approvedDirections = db.Report_Approval_Log
                .Where(l => l.Id_Report_Flow == reportFlowId && l.Action == "Approved")
                .Select(l => l.Approver_Direction)
                .ToHashSet();

            return requiredDirections.All(dir => approvedDirections.Contains(dir));
        }

        public void ApproveReport(int idReportFlow, int idUser)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flow = db.Report_Flow.Single(f => f.Id == idReportFlow);
            var emp = db.Employee.Single(e => e.Id == idUser);

            string userDirection = emp.Direction?.Trim();
            if (string.IsNullOrEmpty(userDirection))
                throw new InvalidOperationException("У сотрудника не указана дирекция (поле Direction).");

            // Проверяем: есть ли схема согласования для этого типа отчёта?
            bool hasScheme = db.Approval_Scheme.Any(s => s.Report_Type == flow.Id_Report_Type);
            if (!hasScheme)
            {
                // Старая логика: один акцепт → Done
                ChangeStatus(idReportFlow, idUser, ReportStatus.Done);
                return;
            }

            // Проверяем: требуется ли именно эта дирекция?
            bool isRequired = db.Approval_Scheme
                .Any(s => s.Report_Type == flow.Id_Report_Type && s.Required_Direction == userDirection);
            if (!isRequired)
                throw new UnauthorizedAccessException($"Ваша дирекция ({userDirection}) не участвует в согласовании этого отчёта.");

            // Проверяем: не утверждал ли уже?
            if (db.Report_Approval_Log.Any(l => l.Id_Report_Flow == idReportFlow && l.Employee_Id == idUser))
                throw new InvalidOperationException("Вы уже утверждали этот отчёт.");

            // Записываем утверждение
            db.Report_Approval_Log.InsertOnSubmit(new Report_Approval_Log
            {
                Id_Report_Flow = idReportFlow,
                Approver_Direction = userDirection,
                Employee_Id = idUser,
                Action = "Approved",
                Created_At = DateTime.Now
            });

            // Проверяем: все ли утвердили?
            var required = db.Approval_Scheme
                .Where(s => s.Report_Type == flow.Id_Report_Type)
                .Select(s => s.Required_Direction)
                .ToList();

            var approved = db.Report_Approval_Log
                .Where(l => l.Id_Report_Flow == idReportFlow && l.Action == "Approved")
                .Select(l => l.Approver_Direction)
                .ToHashSet();

            if (required.All(dir => approved.Contains(dir)))
            {
                // Все утвердили → Done
                flow.Status = ReportStatus.Done.GetDescriptionSt();
                flow.Date_is_done = DateTime.Today;
                flow.User_submit = idUser;
            }
            else
            {
                // Частично утверждён
                flow.Status = ReportStatus.PartiallyApproved.GetDescriptionSt();
            }

            db.SubmitChanges();
        }

        public void RefuseReportMultiApproval(int idReportFlow, int idUser)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flow = db.Report_Flow.Single(f => f.Id == idReportFlow);

            // Удаляем ВСЕ утверждения
            var approvals = db.Report_Approval_Log.Where(l => l.Id_Report_Flow == idReportFlow);
            db.Report_Approval_Log.DeleteAllOnSubmit(approvals);

            // Ставим статус "Refuse"
            flow.Status = ReportStatus.Refuse.GetDescriptionSt();
            flow.Date_edit_co = DateTime.Today;
            flow.User_edit_co = idUser;
            flow.Updated = DateTime.Today;
            flow.Id_Employee_Upd = idUser;

            db.SubmitChanges();
        }

        public List<ApprovalInfoDto> GetApprovalInfo(int idReportFlow)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flow = db.Report_Flow.Single(f => f.Id == idReportFlow);

            var required = db.Approval_Scheme
                .Where(s => s.Report_Type == flow.Id_Report_Type)
                .OrderBy(s => s.Sort_Order)
                .Select(s => s.Required_Direction)
                .ToList();

            if (!required.Any())
                return new List<ApprovalInfoDto>();

            var approvedDict = db.Report_Approval_Log
                .Where(l => l.Id_Report_Flow == idReportFlow && l.Action == "Approved")
                .Join(db.Employee, l => l.Employee_Id, e => e.Id, (l, e) => new
                {
                    l.Approver_Direction,
                    EmployeeName = $"{e.Surname} {e.Name} {e.MiddleName}",
                    l.Created_At
                })
                .ToDictionary(x => x.Approver_Direction, x => x);

            return required.Select(dir => new ApprovalInfoDto
            {
                Direction = dir,
                DirectionName = ExpandDirection(dir),
                EmployeeName = approvedDict.TryGetValue(dir, out var a) ? a.EmployeeName : null,
                ApprovedDate = approvedDict.TryGetValue(dir, out var b) ? b.Created_At : (DateTime?)null,
                IsApproved = approvedDict.ContainsKey(dir)
            }).ToList();
        }

        private string ExpandDirection(string dir) => dir switch
        {
            "БУХ" => "Бухгалтерия",
            "ВАУД" => "Внутренний аудитор",
            "ДКР" => "Дирекция комплексного развития",
            "ДЗПЗ" => "Дирекция по ЗПЗ и ЭКМП",
            "ДИТ" => "Дирекция по информационным технологиям",
            "ДИБ" => "Дирекция по обеспечению информационной безопасности",
            "ДОМС" => "Дирекция по организации ОМС",
            "ДПОБ" => "Дирекция по правовому обеспечению бизнеса",
            "ДПРП" => "Дирекция по работе с персоналом",
            "ДТОМС" => "Дирекция технологий ОМС",
            "ДФАК" => "Дирекция финансового аудита и контроля",
            "ПРИЕМ" => "Приемная",
            "РУК" => "Руководство",
            "СДО" => "Служба документационного обеспечения",
            "СРЕГ" => "Служба по работе с регистром",
            "СЮП" => "Служба юридической практики",
            "ФИН" => "Финансовая дирекция",
            _ => dir ?? "Неизвестно"
        };
    }
}