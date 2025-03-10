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
        private readonly IReportHandler _zpz2025Collector = new Zpz2025Handler(ReportType.Zpz2025);
        private readonly IReportHandler _zpz10Collector = new ZpzHandler(ReportType.Zpz10);
        private readonly IReportHandler _zpz10_2025Collector = new Zpz2025Handler(ReportType.Zpz10_2025);
        private readonly IReportHandler _zpzLethalCollector = new ZpzHandler(ReportType.ZpzLethal);
        private readonly IReportHandler _zpz2025LethalCollector = new Zpz2025Handler(ReportType.ZpzL2025);
        private readonly IReportHandler _zpzQCollector = new ZpzHandler(ReportType.ZpzQ);
        private readonly IReportHandler _zpzQ2025Collector = new Zpz2025Handler(ReportType.ZpzQ2025);
        private readonly IReportHandler _vacCollector = new ReportVaccinationHander(ReportType.Vac);
        private readonly IReportHandler _fssCollector = new FSSMonitoringHandler(ReportType.MFSS);
        private readonly IReportHandler _mvcrCollector = new MonitoringVCRHandler(ReportType.MVCR);
        private readonly IReportHandler _proposalCollector = new ReportProposalHandler(ReportType.Proposal);
        private readonly IReportHandler _quantityCollector = new ReportQuantityHandler(ReportType.Quantity);
        private readonly IReportHandler _opedFinanceCollector = new ReportOpedFinanceHandler(ReportType.OpedFinance);
        private readonly IReportHandler _opedFinance3Collector = new ReportOpedFinance3Handler(ReportType.OpedFinance3);
        private readonly IReportHandler _targetedAllowancesCollector = new ReportTargetedAllowancesHandler(ReportType.TarAllow);
        private readonly IReportHandler _effectiveHandlerCollector = new ReportEffectivenessHandler(ReportType.Effective);
        private readonly IReportHandler _reqVCRHandlerCollector = new ReportReqVCRHandler();
        private readonly IReportHandler _PVPLoadCollector = new ReportPVPLoadHandler(ReportType.PVPLoad);
        private readonly IReportHandler _doffCollector = new ReportDoffHandler(ReportType.Doff);
        private readonly IReportHandler _ViolMEECollector = new ReportViolationsHandler(ReportType.ViolMEE);
        private readonly IReportHandler _ViolEKMPCollector = new ReportViolationsHandler(ReportType.ViolEKMP);
        private readonly IReportHandler _VerifyPlanCollector = new ReportViolationsHandler(ReportType.VerifyPlan);
        private readonly IReportHandler _MonthlyVolCollector = new ReportMonthlyVolHandler(ReportType.MonthlyVol);
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
                ReportType.Zpz10 => _zpz10Collector,
                ReportType.ZpzLethal => _zpzLethalCollector,
                ReportType.ZpzQ => _zpzQCollector,
                ReportType.Zpz2025 => _zpz2025Collector,
                ReportType.Zpz10_2025 => _zpz10_2025Collector,
                ReportType.ZpzL2025 => _zpz2025LethalCollector,
                ReportType.ZpzQ2025 => _zpzQ2025Collector,
                ReportType.OpedQ => _opedCollectorQ,
                ReportType.Vac => _vacCollector,
                ReportType.MFSS => _fssCollector,
                ReportType.MVCR => _mvcrCollector,
                ReportType.Proposal => _proposalCollector,
                ReportType.OpedFinance => _opedFinanceCollector,
                ReportType.OpedFinance3 => _opedFinance3Collector,
                ReportType.Cadre => _cadreHandlerCollector,
                ReportType.ReqVCR => _reqVCRHandlerCollector,
                ReportType.Effective => _effectiveHandlerCollector,
                ReportType.Quantity => _quantityCollector,
                ReportType.TarAllow => _targetedAllowancesCollector,
                ReportType.PVPLoad => _PVPLoadCollector,
                ReportType.Doff => _doffCollector,
                ReportType.ViolMEE => _ViolMEECollector,
                ReportType.ViolEKMP => _ViolEKMPCollector,
                ReportType.VerifyPlan => _VerifyPlanCollector,
                ReportType.MonthlyVol => _MonthlyVolCollector,

            };
    }
}