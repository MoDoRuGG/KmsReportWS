using KmsReportWS.Model.Report;

namespace KmsReportWS.Handler
{
    public class ReportHandlerFactory
    {
        private readonly IReportHandler _f262Collector = new F262Handler();
        private readonly IReportHandler _f294Collector = new F294Handler();
        private readonly IReportHandler _iizlCollector = new IizlHandler(ReportType.Iizl);
        private readonly IReportHandler _iizlCollector2022 = new IizlHandler(ReportType.Iizl2022);
        private readonly IReportHandler _opedCollector = new FOpedHandler(ReportType.Oped);
        private readonly IReportHandler _opedUCollector = new FOpedUHandler(ReportType.OpedU);
        private readonly IReportHandler _opedCollectorQ = new FOpedHandler(ReportType.OpedQ);
        private readonly IReportHandler _infrormationResponseHandlerCollector = new ReportInfrormationResponseHandler();
        private readonly IReportHandler _cadreHandlerCollector = new ReportCadreHandler();
        private readonly IReportHandler _pgCollector = new PgHandler(ReportType.Pg);
        private readonly IReportHandler _pgQCollector = new PgHandler(ReportType.PgQ);
        private readonly IReportHandler _zpzCollector = new ZpzHandler(ReportType.Zpz);
        private readonly IReportHandler _zpzQCollector = new ZpzHandler(ReportType.ZpzQ);
        private readonly IReportHandler _vacCollector = new ReportVaccinationHander(ReportType.Vac);
        private readonly IReportHandler _fssCollector = new FSSMonitoringHandler(ReportType.MFSS);
        private readonly IReportHandler _mvcrCollector = new MonitoringVCRHandler(ReportType.MVCR);
        private readonly IReportHandler _proposalCollector = new ReportProposalHandler(ReportType.Proposal);
        private readonly IReportHandler _opedFinanceCollector = new ReportOpedFinanceHandler(ReportType.OpedFinance);
        //private readonly IReportHandler _infomaterialCollector = new ReportInfomaterialHandler(ReportType.Infomaterial);

        public IReportHandler GetHandler(ReportType reportType) =>
            reportType switch {
                ReportType.Oped => _opedCollector,
                ReportType.OpedU => _opedUCollector,
                ReportType.IR => _infrormationResponseHandlerCollector,
                ReportType.F262 => _f262Collector,
                ReportType.F294 => _f294Collector,
                ReportType.Iizl => _iizlCollector,
                ReportType.Iizl2022 => _iizlCollector2022,
                ReportType.Pg => _pgCollector,
                ReportType.PgQ => _pgQCollector,
                ReportType.Zpz => _zpzCollector,
                ReportType.ZpzQ => _zpzQCollector,
                ReportType.OpedQ => _opedCollectorQ,
                ReportType.Vac => _vacCollector,
                ReportType.MFSS => _fssCollector,
                ReportType.MVCR => _mvcrCollector,
                ReportType.Proposal => _proposalCollector,
                ReportType.OpedFinance => _opedFinanceCollector,
                ReportType.Cadre => _cadreHandlerCollector,
                //ReportType.Infomaterial => _infomaterialCollector,

            };
    }
}