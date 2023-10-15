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
    public class ReportReqVCRHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportReqVCRHandler() : base(ReportType.ReqVCR)
        {
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportReqVCR ??
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

                var ReqVCRDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (ReqVCRDataList.Any())
                {
                    db.Report_ReqVCR.InsertAllOnSubmit(ReqVCRDataList);
                }

                db.SubmitChanges();
            }
        }


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportReqVCR ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var theme =
                    db.Report_Data.SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme);
                if (theme != null)
                {
                    var dataReport = db.Report_ReqVCR.Where(x => x.Id_Report_Data == theme.Id);
                    db.Report_ReqVCR.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var dataList = reportForms.Data.Select(data => MapThemeToPersist(theme.Id, data)).ToList();
                    if (dataList.Any())
                    {
                        db.Report_ReqVCR.InsertAllOnSubmit(dataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportReqVCR { ReportDataList = new List<ReportReqVCRDto>() };
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportReqVCRDto
                {
                    Theme = theme,
                    Data = new List<ReportReqVCRDataDto>(),
                };

                var dataList = themeData.Report_ReqVCR.Select(MapThemeFromPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }


        private ReportReqVCRDataDto MapThemeFromPersist(Report_ReqVCR data) =>
            new ReportReqVCRDataDto
            {
                RowNum = data.RowNum,
                Id = data.Id,
                y2019 = data.y2019 ?? 0,
                y2020 = data.y2020 ?? 0,
                y2021 = data.y2021 ?? 0,
                y2022 = data.y2022 ?? 0,
                y2023 = data.y2023 ?? 0,
            };

        private Report_ReqVCR MapThemeToPersist(int idThemeData, ReportReqVCRDataDto data) =>
            new Report_ReqVCR
            {
                Id_Report_Data = idThemeData,
                RowNum = data.RowNum,
                Id = data.Id,
                y2019 = data.y2019,
                y2020 = data.y2020,
                y2021 = data.y2021,
                y2022 = data.y2022,
                y2023 = data.y2023
            };


    }
}