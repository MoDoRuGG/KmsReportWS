using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public interface IReportCollector
    {
        AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd, ReportStatus status);
    }
}