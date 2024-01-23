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
    public class ReportInfrormationResponseHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportInfrormationResponseHandler() : base(ReportType.IR)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        public ReportInfrormationResponseDataDto GetYearData(string yymm, string theme, string fillial)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_InfrormationResponse.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportInfrormationResponseDataDto
            {
                Informed = x.Sum(g => g.Informed),
                CountPast = x.Sum(g => g.CountPast),
                CountRegistry = x.Sum(g => g.CountRegistry),
                Plan = x.Sum(g => g.Plan)
                

            }).FirstOrDefault();

            return result;

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportInfrormationResponse ??
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

                var fIRList = MapMainThemeFromPersist(themeData.Id, reportForms.Data);
                if (fIRList != null)
                {
                    db.Report_InfrormationResponse.InsertOnSubmit(fIRList);
                }

                db.SubmitChanges();
            }
        }


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportInfrormationResponse ??
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


                var row = db.Report_InfrormationResponse
                       .SingleOrDefault(x => x.Id_Report_Data == idTheme);

                if (report != null)
                {
                    row.Plan = reportForms.Data.Plan;
                    row.Informed = reportForms.Data.Informed;
                    row.CountPast = reportForms.Data.CountPast;
                    row.CountRegistry = reportForms.Data.CountRegistry;
                }
                else
                {
                    var rep = MapMainThemeFromPersist(idTheme, reportForms.Data);
                    db.Report_InfrormationResponse.InsertOnSubmit(rep);
                }


                db.SubmitChanges();


            }
        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportInfrormationResponse { ReportDataList = new List<ReportInfrormationResponseDto>() };
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();

                var dto = new ReportInfrormationResponseDto
                {
                    Theme = theme,
                    Data = new ReportInfrormationResponseDataDto(),

                };


                var dataList = themeData.Report_InfrormationResponse.Select(MapReportDto);
                dto.Data = dataList.First();

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }


        private Report_InfrormationResponse MapMainThemeFromPersist(int idThemeData, ReportInfrormationResponseDataDto data)
        {
            if (data != null)
            {
                return new Report_InfrormationResponse
                {
                    Id = data.Id,
                    Plan = data.Plan,
                    Informed = data.Informed,
                    CountPast = data.CountPast,
                    CountRegistry = data.CountRegistry,
                    Id_Report_Data = idThemeData


                };
            }


            return new Report_InfrormationResponse
            {
                Id = 0,
                Plan = 0,
                Informed = 0,
                CountPast = 0,
                CountRegistry = 0,
                Id_Report_Data = idThemeData
            };


        }


        private ReportInfrormationResponseDataDto MapReportDto(Report_InfrormationResponse report) =>

            new ReportInfrormationResponseDataDto
            {
                Id = report.Id,
                Plan = report.Plan,
                Informed = report.Informed,
                CountPast = report.CountPast,
                CountRegistry = report.CountRegistry
            };


    }
}