using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;
using Org.BouncyCastle.Ocsp;

namespace KmsReportWS.Handler
{
    public class ReportTargetedAllowancesHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;
        private string theme = "Целевые надбавки";

        public ReportTargetedAllowancesHandler(ReportType reportType) : base(reportType)
        {

        }


        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportTargetedAllowances ??
                  throw new Exception("Error saving new report, because getting empty report");

            var themeData = new Report_Data
            {

                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = theme
            };
            db.Report_Data.InsertOnSubmit(themeData);
            db.SubmitChanges();

            report.Id_Report_Data = themeData.Id;

            db.Report_Targeted_Allowances.InsertAllOnSubmit(report.Data.Select(x => new LinqToSql.Report_Targeted_Allowances
            {
                Id_Report_Data = themeData.Id,
                RowNumID = x.RowNumID,
                FIO = x.FIO,
                Speciality = x.Speciality,
                Period = x.Period,
                CountEKMP = x.CountEKMP,
                AmountSank = x.AmountSank,
                AmountPayment = x.AmountPayment,
                ProvidedBy = x.ProvidedBy,
                Comments = x.Comments
            }));

            db.SubmitChanges();


        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportTargetedAllowances();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);
            var reportRows = db.Report_Targeted_Allowances.Where(x => x.Report_Data.Id_Flow == rep.Id);
            if (reportRows != null)
            {
                outReport.Id_Report_Data = reportRows.ToList().ElementAt(0).Id_Report_Data;
                foreach (var row in reportRows)
                {
                    outReport.Data.Add(new TargetedAllowancesData
                    {
                        RowNumID = row.RowNumID,
                        FIO = row.FIO,
                        Speciality = row.Speciality,
                        Period = row.Period,
                        CountEKMP = row.CountEKMP,
                        AmountSank = row.AmountSank,
                        AmountPayment = row.AmountPayment,
                        ProvidedBy = row.ProvidedBy,
                        Comments = row.Comments
                    });
                }

            }
            return outReport;
        }


        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportTargetedAllowances ??
                     throw new Exception("Error update report, because getting empty report");
            var RowCounter = report.Data.Count();
            var reportDb = db.Report_Targeted_Allowances.Where(x => x.Report_Data.Id_Flow == report.IdFlow);

            var razcount = RowCounter - reportDb.Count();
            if (razcount > 0)
            {

                for (var i = 0; i < razcount; i++)
                {

                    var repIn = report.Data.SingleOrDefault(x => x.RowNumID == reportDb.Count() + i);

                    Report_Targeted_Allowances file_row = new Report_Targeted_Allowances
                    {
                        Id_Report_Data = report.Id_Report_Data,
                        RowNumID = repIn.RowNumID,
                        FIO = repIn.FIO,
                        Speciality = repIn.Speciality,
                        Period = repIn.Period,
                        CountEKMP = repIn.CountEKMP,
                        AmountSank = repIn.AmountSank,
                        AmountPayment = repIn.AmountPayment,
                        ProvidedBy = repIn.ProvidedBy,
                        Comments = repIn.Comments
                    };

                    db.GetTable<Report_Targeted_Allowances>().InsertOnSubmit(file_row);
                }
            }


            db.SubmitChanges();
        }


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportTargetedAllowances ??
                     throw new Exception("Error update report, because getting empty report");
            var InRepRowCounter = report.Data.Count();
            var reporDb = db.Report_Targeted_Allowances.Where(x => x.Report_Data.Id_Flow == report.IdFlow);

            if (InRepRowCounter != reporDb.Count()) 
            {
                InsertReport(db, inReport);
            }
            else 
            { 
                var reportDb = db.Report_Targeted_Allowances.Where(x => x.Report_Data.Id_Flow == report.IdFlow);

                foreach (var rep in reportDb)
                {
                    var repIn = report.Data.FirstOrDefault(x => x.RowNumID == rep.RowNumID);

                    if (repIn != null)
                    {
                        rep.Id_Report_Data = report.Id_Report_Data;
                        rep.RowNumID = repIn.RowNumID;
                        rep.FIO = repIn.FIO;
                        rep.Speciality = repIn.Speciality;
                        rep.Period = repIn.Period;
                        rep.CountEKMP = repIn.CountEKMP;
                        rep.AmountSank = repIn.AmountSank;
                        rep.AmountPayment = repIn.AmountPayment;
                        rep.ProvidedBy = repIn.ProvidedBy;
                        rep.Comments = repIn.Comments;
                    }

                    db.SubmitChanges();
                }
            }
        }
    }
}