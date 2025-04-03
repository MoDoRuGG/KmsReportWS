using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.BaseReport
{
    public class MonthlyVolCollector : BaseReportCollector
    {
        public MonthlyVolCollector(ReportType reportType) : base(reportType)
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
                                 select new ReportMonthlyVolDto { Theme = fgr.Key, Data = new List<ReportMonthlyVolDataDto>() };

                var outReport = new ReportMonthlyVol { ReportDataList = new List<ReportMonthlyVolDto>() };

                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportMonthlyVolDto = new ReportMonthlyVolDto { Theme = theme.Theme, Data = data.ToList() };
                    outReport.ReportDataList.Add(reportMonthlyVolDto);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary MonthlyVol");
                throw;
            }
        }

        private IQueryable<ReportMonthlyVolDataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_MonthlyVol)
            group f by f.RowNum
            into fgr
            select new ReportMonthlyVolDataDto
            {
                Code = Convert.ToInt32(fgr.Key),
                CountSluch = fgr.Sum(x => x.CountSluch ?? 0),
                CountAppliedSluch = fgr.Sum(x => x.CountAppliedSluch ?? 0),
                CountSluchMEE = fgr.Sum(x => x.CountSluchMEE ?? 0),
                CountSluchEKMP = fgr.Sum(x => x.CountSluchEKMP ?? 0),  

            };
    }
}