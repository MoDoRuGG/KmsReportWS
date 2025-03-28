using KmsReportWS.Collector.ConsolidateReport;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class ReportCollectorFactory
    {
        private readonly IReportCollector _f262Collector = new F262Collector();
        private readonly IReportCollector _f294Collector = new F294Collector();
        private readonly IReportCollector _iizlCollector = new IizlCollector();
        private readonly IReportCollector _opedCollector = new OpedCollector();
        private readonly IReportCollector _pgCollector = new PgCollector(ReportType.Pg);
        private readonly IReportCollector _pgQCollector = new PgCollector(ReportType.PgQ);
        private readonly IReportCollector _zpzCollector = new ZpzCollector(ReportType.Zpz);
        private readonly IReportCollector _zpzQCollector = new ZpzCollector(ReportType.ZpzQ);
        private readonly IReportCollector _zpz2025Collector = new Zpz2025Collector(ReportType.Zpz2025);
        private readonly IReportCollector _zpzQ2025Collector = new Zpz2025Collector(ReportType.ZpzQ2025);


        public IReportCollector GetCollector(ReportType reportType) =>
            reportType switch {
                ReportType.F262 => _f262Collector,
                ReportType.F294 => _f294Collector,
                ReportType.Iizl => _iizlCollector,
                ReportType.Oped => _opedCollector,
                ReportType.Pg => _pgCollector,
                ReportType.Zpz => _zpzCollector,
                ReportType.Zpz2025 => _zpz2025Collector,
                ReportType.PgQ => _pgQCollector,
                ReportType.ZpzQ => _zpzQCollector,
                ReportType.ZpzQ2025 => _zpzQ2025Collector,
            };
    }
}