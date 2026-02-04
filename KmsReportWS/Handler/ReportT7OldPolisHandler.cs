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
    public class ReportT7OldPolisHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportT7OldPolisHandler() : base(ReportType.T7OldPolis)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport report) { }

        public ReportT7OldPolisDataDto GetYearData(string yymm, string theme, string filial)
        {
            return null;
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportT7OldPolis ??
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

                var f7OldPolisList = MapMainThemeFromPersist(themeData.Id, reportForms.Data);
                if (f7OldPolisList != null)
                {
                    db.Report_T7OldPolis.InsertOnSubmit(f7OldPolisList);
                }

                db.SubmitChanges();
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportT7OldPolis { ReportDataList = new List<ReportT7OldPolisDto>() };
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();

                var dto = new ReportT7OldPolisDto
                {
                    Theme = theme,
                    Data = new ReportT7OldPolisDataDto(),
                };


                var dataList = themeData.Report_T7OldPolis.Select(MapReportDto);
                dto.Data = dataList.FirstOrDefault();

                if (dto.Data != null)
                {
                    outReport.ReportDataList.Add(dto);
                }
            }

            return outReport;
        }

        private Report_T7OldPolis MapMainThemeFromPersist(int idThemeData, ReportT7OldPolisDataDto data)
        {
            if (data != null)
            {
                return new Report_T7OldPolis
                {
                    Id = data.Id,
                    CurrentQuantity = data.CurrentQuantity,
                    CountOldPolis = data.CountOldPolis,
                    Id_Report_Data = idThemeData
                };
            }

            return new Report_T7OldPolis
            {
                Id = 0,
                CurrentQuantity = 0,
                CountOldPolis = 0,
                Id_Report_Data = idThemeData
            };
        }


        private ReportT7OldPolisDataDto MapReportDto(Report_T7OldPolis report) =>

        new ReportT7OldPolisDataDto
        {
            Id = report.Id,
            CurrentQuantity = report.CurrentQuantity ?? 0,
            CountOldPolis = report.CountOldPolis ?? 0,
        };


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportT7OldPolis ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id ?? 0;
                if (idTheme == 0)
                {
                    Log.Error(
                        $"Error getting data. idTheme = 0; IdFlow = {inReport.IdFlow}, Theme = {reportForms.Theme}");
                    continue;
                }


                var row = db.Report_T7OldPolis
                       .SingleOrDefault(x => x.Id_Report_Data == idTheme);

                if (report != null)
                {
                    row.CurrentQuantity = reportForms.Data.CurrentQuantity;
                    row.CountOldPolis = reportForms.Data.CountOldPolis;
                }
                else
                {
                    var rep = MapMainThemeFromPersist(idTheme, reportForms.Data);
                    db.Report_T7OldPolis.InsertOnSubmit(rep);
                }


                db.SubmitChanges();


            }
        }
    }
}