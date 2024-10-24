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
    public class ReportDoffHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportDoffHandler(ReportType reportType) : base(reportType)
        {

        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportDoff ??
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

                var doffDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (doffDataList.Any())
                {
                    db.Report_Doff.InsertAllOnSubmit(doffDataList);
                }
                db.SubmitChanges();
            }

        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportDoff { ReportDataList = new List<ReportDoffDto>() };
            MapFromReportFlow(rep_flow, outReport);

            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportDoffDto { Theme = theme, Data = new List<ReportDoffDataDto>() };

                var dataList = themeData.Report_Doff.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }


        private ReportDoffDataDto MapThemeToPersist(Report_Doff data) =>
        new ReportDoffDataDto
        {
            RowNum = data.RowNum ?? "1",
            Column1 = data.Column1,
            Column2 = data.Column2,
            Column3 = data.Column3,
        };

        private Report_Doff MapThemeToPersist(int idThemeData, ReportDoffDataDto data) =>
            new Report_Doff
            {
                Id_Report_Data = idThemeData,
                RowNum = data.RowNum ?? "1",
                Column1 = data.Column1,
                Column2 = data.Column2,
                Column3 = data.Column3,
            };



        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportDoff ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    var dataReport = db.Report_Doff.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_Doff.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var doffDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (doffDataList.Any())
                    {
                        db.Report_Doff.InsertAllOnSubmit(doffDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        public ReportDoffDataDto GetYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = Convert.ToInt32(yymm) < 2501 ? "2403" : yymm.Substring(0, 2) + "01";
            var result = db.Report_Doff.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "Doff"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportDoffDataDto
            {
                RowNum = rowNum,
                Column2 = x.Sum(g => Convert.ToInt32(g.Column1)).ToString(),

            }).FirstOrDefault();

            return result;
        }


        public ReportDoffDataDto GetBeginningData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = "2403";
            var result = db.Report_Doff.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "Doff"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportDoffDataDto
            {
                RowNum = rowNum,
                Column3 = x.Sum(g => Convert.ToInt32(g.Column1)).ToString(),

            }).FirstOrDefault();

            return result;
        }
    }
}