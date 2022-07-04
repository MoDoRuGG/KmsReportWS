using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class PgCollector : BaseReportCollector
    {
        public PgCollector(ReportType reportType) : base(reportType)
        {
        }

        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flows = GetFilteredReportFlows(db, filials, yymmStart, yymmEnd, status);
                var groupTheme = from f in flows
                    group f by f.Theme
                    into fgr
                    select new ReportPgDto {Theme = fgr.Key, Data = new List<ReportPgDataDto>()};

                var outReport = new ReportPg {ReportDataList = new List<ReportPgDto>()};

                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportPgDto = new ReportPgDto {Theme = theme.Theme, Data = data.ToList()};
                    outReport.ReportDataList.Add(reportPgDto);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary PG");
                throw;
            }
        }

        private IQueryable<ReportPgDataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_Pg)
            group f by f.RowNum
            into fgr
            select new ReportPgDataDto {
                Code = fgr.Key,
                CountSmo = fgr.Sum(x => x.CountSmo ?? 0),
                CountSmoAnother = fgr.Sum(x => x.CountSmoAnother ?? 0),
                CountInsured = fgr.Sum(x => x.CountInsured ?? 0),
                CountInsuredRepresentative = fgr.Sum(x => x.CountInsuredRepresentative ?? 0),
                CountTfoms = fgr.Sum(x => x.CountTfoms ?? 0),
                CountProsecutor = fgr.Sum(x => x.CountProsecutor ?? 0),
                CountOutOfSmo = fgr.Sum(x => x.CountOutOfSmo ?? 0),
                CountAmbulatory = fgr.Sum(x => x.CountAmbulatory ?? 0),
                CountDs = fgr.Sum(x => x.CountDs ?? 0),
                CountDsVmp = fgr.Sum(x => x.CountDsVmp ?? 0),
                CountStac = fgr.Sum(x => x.CountStac),
                CountStacVmp = fgr.Sum(x => x.CountStacVmp ?? 0),
                CountOutOfSmoAnother = fgr.Sum(x => x.CountOutOfSmoAnother ?? 0),
                CountAmbulatoryAnother = fgr.Sum(x => x.CountAmbulatoryAnother ?? 0),
                CountDsAnother = fgr.Sum(x => x.CountDsAnother ?? 0),
                CountDsVmpAnother = fgr.Sum(x => x.CountDsVmpAnother ?? 0),
                CountStacAnother = fgr.Sum(x => x.CountStacAnother ?? 0),
                CountStacVmpAnother = fgr.Sum(x => x.CountStacVmpAnother ?? 0)
            };
    }
}