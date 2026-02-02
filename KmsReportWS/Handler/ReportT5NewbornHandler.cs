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
    public class ReportT5NewbornHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportT5NewbornHandler() : base(ReportType.T5Newborn)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport report) { }

        public ReportT5NewbornDataDto GetYearData(string yymm, string theme, string filial)
        {
            return null;
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportT5Newborn ??
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

                var fT5NewbornList = MapMainThemeFromPersist(themeData.Id, reportForms.Data);
                if (fT5NewbornList != null)
                {
                    db.Report_T5Newborn.InsertOnSubmit(fT5NewbornList);
                }

                db.SubmitChanges();
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportT5Newborn { ReportDataList = new List<ReportT5NewbornDto>() };
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();

                var dto = new ReportT5NewbornDto
                {
                    Theme = theme,
                    Data = new ReportT5NewbornDataDto(),
                };


                var dataList = themeData.Report_T5Newborn.Select(MapReportDto);
                dto.Data = dataList.FirstOrDefault();

                if (dto.Data != null)
                {
                    outReport.ReportDataList.Add(dto);
                }
            }

            return outReport;
        }

        private Report_T5Newborn MapMainThemeFromPersist(int idThemeData, ReportT5NewbornDataDto data)
        {
            if (data != null)
            {
                return new Report_T5Newborn
                {
                    Id = data.Id,
                    MarketShare = data.MarketShare,
                    CountNewborn = data.CountNewborn,
                    CountMaterinityBills = data.CountMaterinityBills,
                    Id_Report_Data = idThemeData
                };
            }


            return new Report_T5Newborn
            {
                Id = 0,
                MarketShare = 0,
                CountNewborn = 0,
                CountMaterinityBills = 0,
                Id_Report_Data = idThemeData
            };


        }


        private ReportT5NewbornDataDto MapReportDto(Report_T5Newborn report) =>

        new ReportT5NewbornDataDto
        {
            Id = report.Id,
            MarketShare = report.MarketShare ?? 0,
            CountNewborn = report.CountNewborn ?? 0,
            CountMaterinityBills = report.CountMaterinityBills ?? 0,
        };


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportT5Newborn ??
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


                var row = db.Report_T5Newborn
                       .SingleOrDefault(x => x.Id_Report_Data == idTheme);

                if (report != null)
                {
                    row.MarketShare = reportForms.Data.MarketShare;
                    row.CountNewborn = reportForms.Data.CountNewborn;
                    row.CountMaterinityBills = reportForms.Data.CountMaterinityBills;
            

                }
                else
                {
                    var rep = MapMainThemeFromPersist(idTheme, reportForms.Data);
                    db.Report_T5Newborn.InsertOnSubmit(rep);
                }


                db.SubmitChanges();


            }
        }
    }
}