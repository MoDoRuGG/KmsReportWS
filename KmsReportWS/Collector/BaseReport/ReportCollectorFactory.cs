using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class ReportCollectorFactory
    {
        private readonly IReportCollector _f262Collector = new F262Collector();
        private readonly IReportCollector _f294Collector = new F294Collector();
        private readonly IReportCollector _iizlCollector = new IizlCollector();
        private readonly IReportCollector _pgCollector = new PgCollector(ReportType.Pg);
        private readonly IReportCollector _pgQCollector = new PgCollector(ReportType.PgQ);
        //добавил кадры
        //private readonly IReportCollector _CadreCollector = new CadreCollector(ReportType.Cadre);
     

        public IReportCollector GetCollector(ReportType reportType) =>
            reportType switch {
                ReportType.F262 => _f262Collector,
                ReportType.F294 => _f294Collector,
                ReportType.Iizl => _iizlCollector,
                ReportType.Pg => _pgCollector,
                //добавил кадры
                //ReportType.Cadre => _CadreCollector,
                
                _ => _pgQCollector
            };
    }
}