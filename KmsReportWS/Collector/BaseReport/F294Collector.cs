using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class F294Collector : BaseReportCollector
    {
        public F294Collector() : base(ReportType.F294)
        {
        }

        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status, DataSource datasource)
        {
            try
            {
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flows = GetFilteredReportFlows(db, filials, yymmStart, yymmEnd, status, datasource);
                var groupTheme = from f in flows
                    group f by f.Theme
                    into fgr
                    select new Report294Dto {Theme = fgr.Key, Data = new List<Report294DataDto>()};

                var outReport = new Report294 {ReportDataList = new List<Report294Dto>()};
                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportData = new Report294Dto {Theme = theme.Theme, Data = data.ToList()};
                    outReport.ReportDataList.Add(reportData);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary f294");
                throw;
            }
        }

        private IQueryable<Report294DataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_f294)
            group f by f.RowNum
            into fgr
            select new Report294DataDto {
                RowNum = fgr.Key,
                CountAddress = fgr.Sum(x => Convert.ToInt32(x.CountAddress ?? 0)),
                CountAnother = fgr.Sum(x => Convert.ToInt32(x.CountAnother ?? 0)),
                CountAnotherDisease = fgr.Sum(x => Convert.ToInt32(x.CountAnotherDisease ?? 0)),
                CountBloodDisease = fgr.Sum(x => Convert.ToInt32(x.CountBloodDisease ?? 0)),
                CountBronchoDisease = fgr.Sum(x => Convert.ToInt32(x.CountBronchoDisease ?? 0)),
                CountEmail = fgr.Sum(x => Convert.ToInt32(x.CountEmail ?? 0)),
                CountEndocrineDisease = fgr.Sum(x => Convert.ToInt32(x.CountEndocrineDisease ?? 0)),
                CountMessengers = fgr.Sum(x => Convert.ToInt32(x.CountMessangers ?? 0)),
                CountOncologicalDisease = fgr.Sum(x => Convert.ToInt32(x.CountOncologicalDisease ?? 0)),
                CountPhone = fgr.Sum(x => Convert.ToInt32(x.CountPhone ?? 0)),
                CountPost = fgr.Sum(x => Convert.ToInt32(x.CountPost ?? 0)),
                CountPpl = fgr.Sum(x => Convert.ToInt32(x.CountPpl ?? 0)),
                CountSms = fgr.Sum(x => Convert.ToInt32(x.CountSms ?? 0))
            };
    }
}