using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class ViolationsCollector : BaseReportCollector
    {
        public ViolationsCollector(ReportType reportType) : base(reportType)
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
                    select new ReportViolationsDto {Theme = fgr.Key, Data = new List<ReportViolationsDataDto>()};

                var outReport = new ReportViolations {ReportDataList = new List<ReportViolationsDto>()};

                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportViolationsDto = new ReportViolationsDto { Theme = theme.Theme, Data = data.ToList()};
                    outReport.ReportDataList.Add(reportViolationsDto);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary Violations");
                throw;
            }
        }

        private IQueryable<ReportViolationsDataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_Violations)
            group f by f.RowNum
            into fgr
            select new ReportViolationsDataDto
            {
                Code = fgr.Key,
                Count= fgr.Sum(x => x.Count ?? 0),

            };
    }
}