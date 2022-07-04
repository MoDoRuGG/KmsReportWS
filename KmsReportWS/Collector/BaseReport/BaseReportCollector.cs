using System;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using NLog;

namespace KmsReportWS.Collector.BaseReport
{
    public abstract class BaseReportCollector : IReportCollector
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected static readonly string ConnStr = Settings.Default.ConnStr;
        
        private readonly ReportType _reportType;

        protected BaseReportCollector(ReportType reportType)
        {
            _reportType = reportType;
        }


    

        public abstract AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status);

        protected IQueryable<Report_Data> GetFilteredReportFlows(LinqToSqlKmsReportDataContext db, string[] filials,
            string yymmStart, string yymmEnd, ReportStatus status)
        {
            var reports = from r in db.Report_Flow
                where r.Id_Report_Type == _reportType.GetDescription()
                      && Convert.ToInt32(r.Yymm) >= Convert.ToInt32(yymmStart)
                      && Convert.ToInt32(r.Yymm) <= Convert.ToInt32(yymmEnd)
                select r;
            if (filials.Any())
                reports = reports.Where(x => filials.Contains(x.Id_Region.Trim()));

            switch (status)
            {
                case ReportStatus.Scan:
                    reports = reports.Where(x => !string.IsNullOrEmpty(x.Scan));
                    break;
                case ReportStatus.Submit:
                    reports = reports.Where(x => x.Status == ReportStatus.Submit.GetDescription()
                                                 || x.Status == ReportStatus.Done.GetDescription());
                    break;
                case ReportStatus.Refuse:
                    reports = reports.Where(x => x.Status == ReportStatus.Refuse.GetDescription());
                    break;
                case ReportStatus.Done:
                    reports = reports.Where(x => x.Status == ReportStatus.Done.GetDescription());
                    break;
                default:
                    reports = reports.Where(x => x.Status != ReportStatus.Refuse.GetDescription());
                    break;
            }

            return reports.SelectMany(x => x.Report_Data);
        }
    }
}