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
    public class ReportViolationsHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportViolationsHandler(ReportType reportType) : base(reportType)
        {

        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportViolations ??
                  throw new Exception("Error saving new report, because getting empty report");
            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data
                {

                    Id_Flow = flow.Id,
                    Id_Report = flow.Id_Report_Type,
                    Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var ViolationsDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (ViolationsDataList.Any())
                {
                    db.Report_Violations.InsertAllOnSubmit(ViolationsDataList);
                }
                db.SubmitChanges();
            }

        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportViolations { ReportDataList = new List<ReportViolationsDto>() };
            MapFromReportFlow(rep_flow, outReport);

            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportViolationsDto { Theme = theme, Data = new List<ReportViolationsDataDto>() };

                var dataList = themeData.Report_Violations.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }


        private ReportViolationsDataDto MapThemeToPersist(Report_Violations data) =>
        new ReportViolationsDataDto
        {
            Code = data.RowNum ?? "2.1.",
            Count = data.Count ?? 0,

        };

        private Report_Violations MapThemeToPersist(int idThemeData, ReportViolationsDataDto data) =>
            new Report_Violations
            {
                Id_Report_Data = idThemeData,
                RowNum = data.Code ?? "2.1.",
                Count = data.Count,

            };



        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportViolations ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    var dataReport = db.Report_Violations.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_Violations.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var ViolationsDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (ViolationsDataList.Any())
                    {
                        db.Report_Violations.InsertAllOnSubmit(ViolationsDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }
    }
}