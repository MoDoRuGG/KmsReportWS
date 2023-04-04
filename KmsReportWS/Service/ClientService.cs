using System;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Constructor;
using KmsReportWS.Model.Service;
using KmsReportWS.Properties;
using NLog;
using Employee = KmsReportWS.LinqToSql.Employee;

namespace KmsReportWS.Service
{
    public class ClientService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnStr = Settings.Default.ConnStr;

        public KmsReportDictionary CheckPassword(string id, string password)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            Employee emp = db.Employee.SingleOrDefault(x => x.Id.ToString() == id && x.Password == password);

            return emp == null
                ? null
                : new KmsReportDictionary { Key = emp.Id.ToString(), Value = emp.Phone, ForeignKey = emp.Email };
        }


        public ClientContext CollectContext()
        {
            try
            {
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var regions = db.Region.Select(x => new KmsReportDictionary
                {
                    Key = x.id.Trim(),
                    Value = x.name.Trim(),
                    ForeignKey = x.name_devision.Trim()
                }).OrderBy(x => x.Value).ToList();

                var users = db.Employee.Where(x => x.IsActive).Select(x => new KmsReportDictionary
                {
                    Key = x.Id.ToString(),
                    Value = x.Surname.Trim() + " " + x.Name.Trim() + " " + x.MiddleName.Trim(),
                    ForeignKey = x.Region
                }).ToList();

                var reportTypes = db.Report_Type.Select(x => new KmsReportDictionary
                {
                    Key = x.Id,
                    Value = x.Name,
                    ForeignKey = x.Period,
                    AdditionalField = x.YymmEnd
                }).ToList();

                var emails = db.Email.Select(x => new KmsReportDictionary
                {
                    Key = x.id.ToString(),
                    Value = x.Email1,
                    ForeignKey = x.Description
                }).ToList();
                var heads = db.Region.Select(x =>
                    new HeadCompany { Phone = x.phone, Fio = x.name_head, Position = x.position, FilialCode = x.id }
                ).ToList();



                return new ClientContext { Regions = regions, Users = users, ReportTypes = reportTypes, Emails = emails, Heads = heads };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error collecting client context");
                throw;
            }
        }
    }
}