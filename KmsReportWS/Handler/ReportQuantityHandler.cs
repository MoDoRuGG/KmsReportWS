using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Handler
{
    public class ReportQuantityHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;
        private string theme = "Численность";

        public ReportQuantityHandler(ReportType reportType) : base(reportType)
        {

        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportQuantity ??
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

            db.Report_Quantity.InsertOnSubmit(new Report_Quantity
            {
                Id_Report_Data = report.Id_Report_Data,
                Col_1 = report.Col_1,
                Col_2 = report.Col_2,
                Col_3 = report.Col_3,
                Col_4 = report.Col_4,
                Col_5 = report.Col_5,
                Col_6 = report.Col_6,
                Col_7 = report.Col_7,
                Col_8 = report.Col_8,
                Col_9 = report.Col_9,
                Col_10 = report.Col_10,
                Col_11 = report.Col_11,
                Col_12 = report.Col_12,
                Col_13 = report.Col_13,
                Col_14 = report.Col_14,
                Col_15 = report.Col_15,
                Col_16 = report.Col_16
            });

            db.SubmitChanges();


        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportQuantity();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);

            var report = db.Report_Quantity.FirstOrDefault(x => x.Report_Data.Id_Flow == rep.Id);
            if (report != null)
            {
                outReport.Id_Report_Data = report.Id_Report_Data;
                outReport.Col_1 = report.Col_1;
                outReport.Col_2 = report.Col_2;
                outReport.Col_3 = report.Col_3;
                outReport.Col_4 = report.Col_4;
                outReport.Col_5 = report.Col_5;
                outReport.Col_6 = report.Col_6;
                outReport.Col_7 = report.Col_7;
                outReport.Col_8 = report.Col_8;
                outReport.Col_9 = report.Col_9;
                outReport.Col_10 = report.Col_10;
                outReport.Col_11 = report.Col_11;
                outReport.Col_12 = report.Col_12;
                outReport.Col_13 = report.Col_13;
                outReport.Col_14 = report.Col_14;
                outReport.Col_15 = report.Col_15;
                outReport.Col_16 = report.Col_16;
            }

            return outReport;

        }
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportQuantity ??
                     throw new Exception("Error update report, because getting empty report");

            var reportDb = db.Report_Quantity.FirstOrDefault(x => x.Report_Data.Id_Flow == report.IdFlow);

            if (reportDb != null)
            {
                reportDb.Col_1 = report.Col_1;
                reportDb.Col_2 = report.Col_2;
                reportDb.Col_3 = report.Col_3;
                reportDb.Col_4 = report.Col_4;
                reportDb.Col_5 = report.Col_5;
                reportDb.Col_6 = report.Col_6;
                reportDb.Col_7 = report.Col_7;
                reportDb.Col_8 = report.Col_8;
                reportDb.Col_9 = report.Col_9;
                reportDb.Col_10 = report.Col_10;
                reportDb.Col_11 = report.Col_11;
                reportDb.Col_12 = report.Col_12;
                reportDb.Col_13 = report.Col_13;
                reportDb.Col_14 = report.Col_14;
                reportDb.Col_15 = report.Col_15;
                reportDb.Col_16 = report.Col_16;
            }

            db.SubmitChanges();
        }
    }
}