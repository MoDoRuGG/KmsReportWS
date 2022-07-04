using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Properties;

namespace KmsReportWS.Service
{
    public class EmailService
    {
        private readonly LinqToSqlKmsReportDataContext db;

        public EmailService()
        {
            db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr);
        }


        public List<KmsReportDictionary> GetEmails()
        {
            return db.Email.Select(x =>
            new KmsReportDictionary
            {
                Key = x.id.ToString(),
                Value = x.Email1,
                ForeignKey = x.Description
            }).ToList();
        }

        public void AddEmail(string email, string description)
        {
            var Email = new Email
            {
                Email1 = email,
                Description = description

            };

            db.Email.InsertOnSubmit(Email);
            db.SubmitChanges();

        }

        public void EditEmail(int idEmail, string email, string description)
        {
            var Email = db.Email.First(x => x.id == idEmail);
            Email.Email1 = email;
            Email.Description = description;
            db.SubmitChanges();

        }

        public void DeleteEmail(int idEmail)
        {
            var Email = db.Email.First(x => x.id == idEmail);
            db.Email.DeleteOnSubmit(Email);
            db.SubmitChanges();

        }
    }
}