using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;

namespace KmsReportWS.Handler
{
    public class ReportMonthlyVolHandler : BaseReportHandler
    {
        private readonly string _connStr = Settings.Default.ConnStr;
        public ReportMonthlyVolHandler(ReportType reportType) : base(reportType)
        {
        }
        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        public ReportMonthlyVolDataDto GetYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_MonthlyVol.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "MonthlyVol"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportMonthlyVolDataDto
            {  
            CountSluch = (int)x.Sum(g => g.CountSluch),
            CountAppliedSluch = (int)x.Sum(g => g.CountAppliedSluch),
            CountSluchMEE = (int)x.Sum(g => g.CountSluchMEE),
            CountSluchEKMP = (int)x.Sum(g => g.CountSluchEKMP),

            }).FirstOrDefault();

            return result;

        }


        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportMonthlyVol ??
                         throw new Exception("Error saving new report, because getting empty report");
            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data {
                    Id_Flow = flow.Id, Id_Report = flow.Id_Report_Type, Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var monVolDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (monVolDataList.Any())
                {
                    db.Report_MonthlyVol.InsertAllOnSubmit(monVolDataList);
                }
                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportMonthlyVol ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    var dataReport = db.Report_MonthlyVol.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_MonthlyVol.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var MonthlyVolDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (MonthlyVolDataList.Any())
                    {
                        db.Report_MonthlyVol.InsertAllOnSubmit(MonthlyVolDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportMonthlyVol { ReportDataList = new List<ReportMonthlyVolDto>()};
            MapFromReportFlow(rep_flow, outReport);

            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportMonthlyVolDto { Theme = theme, Data = new List<ReportMonthlyVolDataDto>()};

                var dataList = themeData.Report_MonthlyVol.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private ReportMonthlyVolDataDto MapThemeToPersist(Report_MonthlyVol data) =>
            new ReportMonthlyVolDataDto
            {
                Code = data.RowNum,
                CountSluch = data.CountSluch ?? 0,
                CountAppliedSluch = data.CountAppliedSluch ?? 0,
                CountSluchMEE = data.CountSluchMEE ?? 0,
                CountSluchEKMP = data.CountSluchEKMP ?? 0,

            };

        private Report_MonthlyVol MapThemeToPersist(int idThemeData, ReportMonthlyVolDataDto data) =>
            new Report_MonthlyVol
            {
                Id_Report_Data = idThemeData,
                RowNum = data.Code,
                CountSluch = data.CountSluch,
                CountAppliedSluch = data.CountAppliedSluch,
                CountSluchMEE = data.CountSluchMEE,
                CountSluchEKMP = data.CountSluchEKMP,

            };
    }
}