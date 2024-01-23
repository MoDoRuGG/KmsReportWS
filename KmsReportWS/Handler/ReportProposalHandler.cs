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
    public class ReportProposalHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;
        private string theme = "Предложения";

        public ReportProposalHandler(ReportType reportType) : base(reportType)
        {

        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportProposal ??
                  throw new Exception("Error saving new report, because getting empty report");

            var themeData = new Report_Data
            {

                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = theme
            };
            db.Report_Data.InsertOnSubmit(themeData);
            db.SubmitChanges();

            report.IdReportData = themeData.Id;

            db.Report_Proposal.InsertOnSubmit(new Report_Proposal
            {
                id_ReportData = report.IdReportData,
                Count_MoCheck = report.CountMoCheck,
                Count_Proporsals = report.CountProporsals,
                Count_MoCheckWithDefect = report.CountMoCheckWithDefect,
                Count_ProporsalsWithDefect = report.CountProporsalsWithDefect,
                Notes = report.Notes
                

            });

            db.SubmitChanges();


        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportProposal();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);

            var report = db.Report_Proposal.FirstOrDefault(x => x.Report_Data.Id_Flow == rep.Id);
            if (report != null)
            {
                outReport.IdReportData = report.id_ReportData;
                outReport.CountMoCheck = report.Count_MoCheck;
                outReport.CountMoCheckWithDefect = report.Count_MoCheckWithDefect;
                outReport.CountProporsals = report.Count_Proporsals;
                outReport.CountProporsalsWithDefect = report.Count_ProporsalsWithDefect;
                outReport.Notes = report.Notes;
            }

            return outReport;

        }
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportProposal ??
                     throw new Exception("Error update report, because getting empty report");

            var reportDb = db.Report_Proposal.FirstOrDefault(x => x.Report_Data.Id_Flow == report.IdFlow);

            if (reportDb != null)
            {
                reportDb.Count_MoCheck = report.CountMoCheck;
                reportDb.Count_Proporsals = report.CountProporsals;
                reportDb.Count_ProporsalsWithDefect = report.CountProporsalsWithDefect;
                reportDb.Count_MoCheckWithDefect = report.CountMoCheckWithDefect;
                reportDb.Notes = report.Notes;
            }

            db.SubmitChanges();
        }
    }
}