using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Report;
using KmsReportWS.Model.Service;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using Employee = KmsReportWS.Model.Service.Employee;

namespace KmsReportWS.Service
{
    public class CommonDbEngine
    {
        private readonly LinqToSqlKmsReportDataContext db;

        public CommonDbEngine()
        {
            db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr);
        }

        public List<KmsReportDictionary> CollectReportDisctionary() =>
            db.Report_Types.Select(x => new KmsReportDictionary
            {
                Key = x.Id,
                Value = x.Name,
                ForeignKey = x.Period
            }).ToList();


        public int GetExistReport(string filialCode, string reportYymm, string reportType)
        {
            var curReport = db.Report_Flow.SingleOrDefault(x =>
                x.Id_Region == filialCode && x.Id_Report_Type == reportType && x.Yymm == reportYymm && x.Version == 1);
            return curReport?.Id ?? 0;
        }

        public List<KmsReportDictionary> GetRegions() =>
            db.Region.Select(x => new KmsReportDictionary
            {
                Key = x.id.Trim(),
                Value = x.name.Trim(),
                ForeignKey = x.name_devision.Trim()
            }).ToList();

        public KmsReportDictionary CheckPassword(string filialCode, string login, string password)
        {
            LinqToSql.Employee emp = db.Employee.SingleOrDefault(x =>
                x.Login == login && x.Password == password && x.Region == filialCode);

            if (emp == null)
            {
                return null;
            }

            return new KmsReportDictionary
            {
                Key = emp.Id.ToString(),
                Value = emp.Phone,
                ForeignKey = emp.Email
            };

        }

        public List<KmsReportDictionary> GetEmloyees() =>
            db.Employee.Where(x=>x.IsActive).Select(x => new KmsReportDictionary
            {
                Key = x.Login.ToString(),
                Value = x.Surname.Trim() + " " + x.Name.Trim() + " " + x.MiddleName.Trim(),
                ForeignKey = x.Region
            }).ToList();

        public KmsReportDictionary GetHead(string filialCode) =>
            db.Region.Where(x => x.id == filialCode)
                .Select(x => new KmsReportDictionary
                {
                    Key = x.position.ToString(),
                    Value = x.name_head
                }).Single();






        public string GetFilialName(string key)
        {
            //todo delete at 1.0.7.2
            var devision = db.Region.SingleOrDefault(x => x.id == key);
            if (devision == null)
            {
                devision = db.Region.Single(x => x.name == key);
            }

            return devision?.name_devision ?? "";
        }

        public string GetUserByCode(int key)
        {
            var emp = db.Employee.Single(x => x.Id == key);
            return $"{emp.Surname} {emp.Name} {emp.MiddleName}";
        }

        
        public List<ReportFlowDto> GetReportFlows(string filial, string yymmStart, string yymmEnd)
        {
            var flows = from r in db.Report_Flow
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
                            Status = StatusUtils.ParseStatus(r.Status),
                            DataSource = DataSourseUtils.ParseDataSource(r.DataSource)
                        };
            if (!string.IsNullOrEmpty(filial))
            {
                flows = flows.Where(x => x.IdRegion == filial);
            }

            return flows.ToList();
        }

        
        public List<ReportComment> GetComments(string filialCode, string yymm, string id)
        {
            var idReport = GetExistReport(filialCode, yymm, id);
            var commentsList = new List<ReportComment>();
            if (idReport > 0)
            {
                var comments = db.Comment.Where(x => x.Id_Flow == idReport);
                foreach (var com in comments)
                {
                    var emp = com.Employee;
                    var name = $"{emp.Surname} {emp.Name} {emp.MiddleName}";
                    var comment = new ReportComment { Name = name, Comment = com.Comment1, DateIns = com.Date_ins };
                    commentsList.Add(comment);
                }
            }

            return commentsList;
        }

        public void AddComment(string filialCode, string yymm, string id, int idEmp, string comment)
        {
            var idReport = GetExistReport(filialCode, yymm, id);
            var com = new Comment
            {
                Comment1 = comment,
                Id_Flow = idReport,
                Id_Employee = idEmp,
                Date_ins = DateTime.Today
            };
            db.Comment.InsertOnSubmit(com);
            db.SubmitChanges();
        }

        public List<string> GetEmailsForNotification(string filialCode)
        {
            var emails = from emp in db.Employee
                         where emp.Region == filialCode
                         select emp.Email;
            return emails.ToList();
        }

        public List<Employee> GetEmailsForManualNotification(List<string> filialCode, bool isRefuse,
            string reportType, string yymm)
        {
            var flows = from emp in db.Employee
                        join flow in db.Report_Flow on emp.Region equals flow.Id_Region
                        where flow.Id_Report_Type == reportType && flow.Yymm == yymm &&
                              flow.Status != ReportStatus.Done.GetDescriptionSt()
                        select new
                        {
                            emp.Surname,
                            emp.Name,
                            emp.MiddleName,
                            emp.Region,
                            emp.Email,
                            emp.Phone,
                            flow.Status
                        };
            if (isRefuse)
            {
                flows = flows.Where(x => x.Status == ReportStatus.Refuse.GetDescriptionSt());
            }

            if (filialCode?.Count > 0)
            {
                flows = flows.Where(x => filialCode.Contains(x.Region));
            }

            return flows.Select(emp => new Employee
            {
                Surname = emp.Surname,
                Name = emp.Name,
                MiddleName = emp.MiddleName,
                Email = emp.Email,
                Phone = emp.Phone,
                Region = emp.Region
            }).Distinct().ToList();
        }

        public void EditFilial(string filialCode, string filialName, string fio, string position, string phone)
        {
            var filial = db.Region.Single(r => r.id == filialCode);
            filial.name_devision = filialName;
            filial.position = position;
            filial.phone = phone;

            db.SubmitChanges();
        }

        public void SaveUser(string filialCode, string fio, string email, string phone)
        {
            var parts = fio.Split(' ');
            var surname = parts[0];
            var name = parts[1];
            var middlename = "";

            if (parts.Length > 2)
            {
                for (var i = 2; i < parts.Length; i++)
                {
                    middlename += parts[i];
                }
            }

            var empl = db.Employee.SingleOrDefault(e =>
                e.Region == filialCode && e.Surname == surname && e.Name == name && e.MiddleName == middlename);

            if (empl != null)
            {
                empl.Surname = surname;
                empl.Name = name;
                empl.MiddleName = middlename;
                empl.Email = email;
                empl.Phone = phone;
            }
            else
            {
                var login = surname + name[0];
                login += middlename.Length > 0 ? middlename[0].ToString() : "";
                var newEmpl = new LinqToSql.Employee
                {
                    Surname = surname,
                    Name = name,
                    MiddleName = middlename,
                    Email = email,
                    Phone = phone,
                    Password = phone,
                    Login = login,
                    IsActive = true,
                    Region = filialCode
                };

                db.Employee.InsertOnSubmit(newEmpl);
            }

            db.SubmitChanges();
        }
    }
}