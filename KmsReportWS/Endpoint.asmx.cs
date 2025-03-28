using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Services;
using System.Web.Services.Description;
using KmsReportWS.Collector.BaseReport;
using KmsReportWS.Collector.ConsolidateReport;
using KmsReportWS.Handler;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Constructor;
using KmsReportWS.Model.Dictionary;
using KmsReportWS.Model.Report;
using KmsReportWS.Model.Service;
using KmsReportWS.Service;
using System.Threading.Tasks;
using System.Linq;

namespace KmsReportWS
{
    /// <summary>
    ///     Веб-сервис для предоставления и формирования отчетности филиалами ООО «Капитал МС»
    /// </summary>
    [WebService(Namespace = "http://kms-oms.ru/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Endpoint : WebService
    {
        private readonly EmailSender emailSender = new EmailSender();

        private readonly EmailService emailService = new EmailService();
        private readonly FilialService _filialService = new FilialService();
        private readonly CommentService _commentService = new CommentService();
        private readonly ClientService _clientService = new ClientService();
        private readonly ReportFlowHandler _flowHandler = new ReportFlowHandler();
        private readonly ReportCollectorFactory _reportCollectorFactory = new ReportCollectorFactory();
        private readonly ReportHandlerFactory _reportHandlerFactory = new ReportHandlerFactory();
        private readonly ReportDynamicFlowHandler _reportDynamicFlowHandler = new ReportDynamicFlowHandler();
        private readonly ManualNotificationService _notificationService = new ManualNotificationService();
        private readonly FileProcessor _fileProcessor = new FileProcessor();
        private readonly DynamicReportHandler _dynamicReportHandler = new DynamicReportHandler();



        [WebMethod]
        public List<ConsolidateProposal> ConsolidateProposalCollect(string yymm)
        {
            return new ConsolidateProposalCollector().Collect(yymm);
        }


        [WebMethod]
        public CheckFFOMS2022CommonData GetCheckFFOMS2022CommonData(string year, string idReport)
        {
            return new DynamicReportCommonHandler().GetCheckFFOMS2022CommonData(year, idReport);
        }

        [WebMethod]
        public List<OpedFinancePlanDictionaryItem> GetOpedFinanceList(string year)
        {
            return new OpedFinancePlanDictionaryHandler().GetList(year);

        }

        [WebMethod]
        public List<QuantityPlanDictionaryItem> GetQuantityPlanList(string year)
        {
            return new QuantityPlanDictionaryHandler().GetList(year);

        }

        [WebMethod]
        public List<ConsolidateQuantityAR> CreateConsolidateQuantityAddRemove(string year)
        {
            return new ConsolidateQuantityARCollector().Collect(year);

        }

        [WebMethod]
        public List<ConsolidateQuantityQ> CreateConsolidateQuantityQ(string yymm)
        {
            return new ConsolidateQuantityQCollector().Collect(yymm);

        }

        [WebMethod]
        public List<ConsolidateQuantityFP> CreateConsolidateQuantityFactPlan(string year)
        {
            return new ConsolidateQuantityFPCollector().Collect(year);

        }


        [WebMethod]
        public List<ConsolidateQuantityInfo> CreateConsolidateQuantityInformation(string year)
        {
            return new ConsolidateQuantityInfoCollector().Collect(year);

        }


        [WebMethod]
        public List<ConsolidateOpedFinance_3> ConsolidateOpedFinance3(string year)
        {
            return new ConsolidateOpedFinance_3Collector().Collect(year);

        }

        [WebMethod]
        public List<ConsolidateOpedFinance_2> ConsolidateOpedFinance2(string year)
        {
            return new ConsolidateOpedFinance_2Collector().Collect(year);

        }

        [WebMethod]
        public List<ConsolidateOpedFinance_1> ConsolidateOpedFinance1(string year)
        {
            return new ConsolidateOpedFinance_1Collector().Collect(year);

        }


        [WebMethod]
        public void SaveOpedFinanceList(List<OpedFinancePlanDictionaryItem> plans)
        {
            new OpedFinancePlanDictionaryHandler().Save(plans);

        }

        [WebMethod]
        public void SaveQuantityPlanList(List<QuantityPlanDictionaryItem> plans)
        {
            new QuantityPlanDictionaryHandler().Save(plans);

        }

        [WebMethod]
        public List<ConsolidateOpedQ> ConsolidateOpedQCollect(string yymm)
        {
            return new ConsolidateOpedQCollector().Collect(yymm);

        }

        [WebMethod]
        public List<ConsolidateCPNP_Q_2> ConsolidateCPNP2QCollect(string yymm)
        {
            return new ConsolidateCPNP_Q_2_Collector().Collect(yymm);

        }

        [WebMethod]
        public List<ReportScanModel> GetScans(int idReport)
        {
            return _flowHandler.GetScans(idReport);

        }


        [WebMethod]
        public List<KmsReportDictionary> GetEmails()
        {
           return emailService.GetEmails();
            
        }

        [WebMethod]
        public ReportOpedDto[] GetYearOpedData(string yymm,string filiall)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Oped);
            return handler.GetYearOpedData(yymm, filiall);
           
        }

        [WebMethod]
        public ReportOpedUDto[] GetYearOpedUData(string yymm, string filiall)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.OpedU);
            return handler.GetYearOpedUData(yymm, filiall);

        }

        [WebMethod]
        public ReportInfrormationResponseDataDto GetIRYearData(string yymm, string theme, string fillial)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.IR);
            return (handler as ReportInfrormationResponseHandler).GetYearData(yymm,theme,fillial);
        }

        [WebMethod]
        public ReportZpzDataDto GetZpz10YearData(string yymm, string theme, string fillial, string rowNum)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Zpz10);
            return (handler as ZpzHandler).GetYearData(yymm, theme, fillial, rowNum);
        }


        [WebMethod]
        public ReportZpz2025DataDto GetZpz10_2025YearData(string yymm, string theme, string fillial, string rowNum)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Zpz10_2025);
            return (handler as Zpz2025Handler).GetYearData(yymm, theme, fillial, rowNum);
        }


        [WebMethod]
        public ReportDoffDataDto GetDoffYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Doff);
            return (handler as ReportDoffHandler).GetYearData(yymm, theme, fillial, rowNum);
        }


        [WebMethod]
        public ReportDoffDataDto GetDoffBeginningData(string yymm, string theme, string fillial, string rowNum)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Doff);
            return (handler as ReportDoffHandler).GetBeginningData(yymm, theme, fillial, rowNum);
        }


        [WebMethod]
        public ReportZpzDataDto GetZpzLethalYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.ZpzLethal);
            return (handler as ZpzHandler).GetLethalYearData(yymm, theme, fillial, rowNum);
        }

        [WebMethod]
        public ReportZpz2025DataDto GetZpz2025LethalYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.ZpzL2025);
            return (handler as Zpz2025Handler).GetLethalYearData(yymm, theme, fillial, rowNum);
        }

        [WebMethod]
        public ReportCadreDataDto GetCadreYearData(string yymm, string theme, string fillial)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Cadre);
            return (handler as ReportCadreHandler).GetYearData(yymm, theme, fillial);
        }

        [WebMethod]
        public ReportVaccination GetVacYearData(string yymm, string fillial)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.Vac);
            return (handler as ReportVaccinationHander).GetYearData(yymm,fillial);

        }

        [WebMethod]
        public List<FSSMonitoringPgDataDto> GetFSSMonitoringPGData(string yymm, string fillial)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.MFSS);
            return (handler as FSSMonitoringHandler).GetPgData(yymm, fillial);

        }

        [WebMethod]
        public List<MonitoringVCRPgDataDto> GetMonitoringVCRPGData(string yymm, string fillial)
        {
            var handler = _reportHandlerFactory.GetHandler(ReportType.MVCR);
            return (handler as MonitoringVCRHandler).GetPgData(yymm, fillial);

        }

        [WebMethod]
        public void SendEmail(List<string> emails,string theme,string body)
        {
            emailSender.Send(emails,theme,body);
        }

        [WebMethod]
        public void AddEmail(string email, string description)
        {
            emailService.AddEmail(email,description);
        }

        [WebMethod]
        public void EditEmail(int emailId,string email, string description)
        {
            emailService.EditEmail(emailId, email, description);
        }

        [WebMethod]
        public void DeleteEmail(int emailId)
        {
            emailService.DeleteEmail(emailId);
        }

        [WebMethod]
        public void ChangeDynamicReportStatus(int idFlow,ReportStatus status) =>
           _reportDynamicFlowHandler.ChangeStatus(idFlow, status);

        [WebMethod]
        public Report_Dynamic SaveDynamicReport(ReportDynamicDto report) =>
           _dynamicReportHandler.SaveReport(report);

        [WebMethod]
        public List<ReportDynamicFlowDto> GetReportDynamicFlows(int year) =>
          _reportDynamicFlowHandler.GetReportFlows(year);


        [WebMethod]
        public List<ReportDynamicScanModel> GetScansDynamic(int idFlow)
        {
            return _reportDynamicFlowHandler.GetScans(idFlow);
        }

        [WebMethod]
        public void SaveDynamicScan(int idFlow, string fileName)
        {
            _reportDynamicFlowHandler.SaveScan(idFlow, fileName);
        }

        [WebMethod]
        public void DeleteDynamicScan(int idDynamicScan)
        {
            _reportDynamicFlowHandler.DeleteScan(idDynamicScan);
        }

        [WebMethod]
        public ReportDynamicDto GetReportDynamicById(int reportId) =>
          _dynamicReportHandler.GetReportDynamic(reportId);

        [WebMethod]
        public ReportDynamicFlowDto GetReportDynamicFlowById(int flowId) =>
        _reportDynamicFlowHandler.GetReportFlow(flowId);

        [WebMethod]
        public void WebReportOped(ReportOped report)
        {
            
        }

        [WebMethod]
        public void WebReportOpedU(ReportOpedU report)
        {

        }

        [WebMethod]
        public void WebReportInfrormationResponse(ReportInfrormationResponse report)
        {

        }


        [WebMethod]
        public void WebReportVaccination(ReportVaccination report)
        {

        }

        [WebMethod]
        public void WebReportFSSMonitroing(ReportFSSMonitroing report)
        {

        }

        [WebMethod]
        public void WebReportMonitoringVCR(ReportMonitoringVCR report)
        {

        }


        [WebMethod]
        public void WebReportProposal(ReportProposal report)
        {

        }

        [WebMethod]
        public void WebReportDoff(ReportDoff report)
        {

        }

        [WebMethod]
        public void WebReportOpedFinance(ReportOpedFinance report)
        {

        }

        [WebMethod]
        public void WebReportOpedFinance3(ReportOpedFinance3 report)
        {

        }

        [WebMethod]
        public void WebReportInfomaterial(ReportInfomaterial report)
        {

        }

        [WebMethod]
        public void WebReportCadre(ReportCadre report)
        {

        }

        [WebMethod]
        public void WebReportViolations(ReportViolations report)
        {

        }

        [WebMethod]
        public void WebReportQuantity (ReportQuantity report)
        {

        }

        [WebMethod]
        public void WebReportTargetedAllowances(ReportTargetedAllowances report)
        {

        }

        [WebMethod]
        public void WebReportPVPLoad(ReportPVPLoad report)
        {

        }

        [WebMethod]
        public void WebReportReqVCR(ReportReqVCR report)
        {

        }


        [WebMethod]
        public List<DynamicDataDto> GetReportRegionData(int idFlow) =>
       _dynamicReportHandler.GetReportRegionData(idFlow);

        [WebMethod]
        public List<ReportDynamicDto> GetDynamicReports() =>
         _dynamicReportHandler.GetReportDynamic();

        [WebMethod]
        public GetDynamicReportResponse GetDynamicReportXml(int reportId) =>
            _dynamicReportHandler.GetXmlReport(reportId);

        [WebMethod]
        public int SaveDynamicFlowData(List<DynamicDataDto> data, int IdReportDynamic, int idUser, string fillialCode) =>
          _dynamicReportHandler.SaveReportInDb(data, IdReportDynamic, idUser, fillialCode);


        [WebMethod]
        public List<ReportFlowDto> GetFlows(string filialCode, string yymmStart, string yymmEnd) =>
            _flowHandler.GetReportFlows(filialCode, yymmStart, yymmEnd);

        [WebMethod]
        public void SaveScan(int idReport, int idUser, string uri, int num) =>
            _flowHandler.SaveScanToDb(idReport, idUser, uri, num);


        [WebMethod]
        public void DeleteScan(int idReport, int idUser,  int num) =>
            _flowHandler.DeleteScan(idReport, idUser, num);


        [WebMethod]
        public List<ConsolidateDisp> CreateReportDisp(string yymm)
        {
            var consolidate = new ConsolidateDispCollector();
            return consolidate.Collect(yymm);
        }

        [WebMethod]
        public void ChangeStatus(int idReport, int idUser, ReportStatus status) =>
            _flowHandler.ChangeStatus(idReport, idUser, status);

        [WebMethod]
        public void ChangeDataSource(int idReport, int idUser, DataSource datasource) =>
    _flowHandler.ChangeDataSource(idReport, idUser, datasource);

        [WebMethod]
        public AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status, ReportType reportType)
        {
            var collector = _reportCollectorFactory.GetCollector(reportType);
            return collector.CollectSummaryReport(filials, yymmStart, yymmEnd, status);
        }

        [WebMethod]
        public AbstractReport GetReport(string filialCode, string yymm, ReportType reportType)
        {
            var handler = _reportHandlerFactory.GetHandler(reportType);
            return handler.GetReport(filialCode, yymm);
        }

        [WebMethod]
        public AbstractReport SaveReport(AbstractReport report, string yymm, int idUser, string filialCode,
            ReportType reportType)
        {
            var handler = _reportHandlerFactory.GetHandler(reportType);
            return handler.SaveReportToDb(report, yymm, idUser, filialCode);
        }

        [WebMethod]
        public AbstractReport SaveReportDataSourceHandle(AbstractReport report, string yymm, int idUser, string filialCode,
        ReportType reportType)
        {
            var handler = _reportHandlerFactory.GetHandler(reportType);
            return handler.SaveReportDataSourceHandle(report, yymm, idUser, filialCode);
        }
            
        [WebMethod]
        public AbstractReport SaveReportDataSourceExcel(AbstractReport report, string yymm, int idUser, string filialCode,
        ReportType reportType)
        {
            var handler = _reportHandlerFactory.GetHandler(reportType);
            return handler.SaveReportDataSourceExcel(report, yymm, idUser, filialCode);
        }

        [WebMethod]
        public string SendNotification(NotificationRequest request) =>
            _notificationService.SendNotification(request);

        // comment form 
        [WebMethod]
        public List<ReportComment> GetComments(int idReport, string filialCode) =>
            _commentService.GetComments(idReport, filialCode);

        [WebMethod]
        public bool IsReportHasComments(int idReport) =>
           _commentService.IsReportHasComments(idReport);

        [WebMethod]
        public void AddComment(int idReport, int idEmp, string comment) =>
            _commentService.AddComment(idReport, idEmp, comment);

        [WebMethod]
        public List<UnreadComment> GetReportsWithUnreadComments(string filialCode) =>
            _commentService.GetReportsWithUnreadComments(filialCode);
        // comment form

        // using in autorization Form
        [WebMethod]
        public KmsReportDictionary CheckPasswordNew(string id, string password) =>
            _clientService.CheckPassword(id, password);

        [WebMethod]
        public ClientContext CollectClientContext() =>
            _clientService.CollectContext();
        // using in autorization Form

        // using in seeting form
        [WebMethod]
        public void SaveUser(string filialCode, string fio, string email, string phone, bool isAddUser, int idUser) =>
            _filialService.SaveUser(filialCode, fio, email, phone, isAddUser, idUser);

        [WebMethod]
        public void EditFilial(string filialCode, string filialName, string fio, string position, string phone) =>
            _filialService.EditFilial(filialCode, filialName, fio, position, phone);
        //using in setting form

        [WebMethod]
        public void UploadFile(byte[] bytes, string fileName, string filial) =>
            _fileProcessor.UploadFile(bytes, fileName, filial);

        [WebMethod]
        public byte[] DownloadFile(string fileName, string filial) =>
            _fileProcessor.DownloadFile(fileName, filial);

        [WebMethod]
        public byte[] DownloadDllFile(string fileName) =>
         _fileProcessor.DownloadDllFile(fileName);

        [WebMethod]
        public List<string> GetDllFileNames() =>
           _fileProcessor.GetDllFileNames();


        

        [WebMethod]
        public void UploadXmlDynamicFile(byte[] bytes, string fileName) =>
           _fileProcessor.UploadXmlDynamicFile(bytes, fileName);



        [WebMethod]
        public void MethodForSendingChildModel(Report262 f262, Report294 f294, ReportIizl iilz, ReportPg pg, ReportZpz zpz, ReportZpz2025 zpz2025, ReportEffectiveness effective, ReportViolations violations, ReportMonthlyVol monthlyVol)
        {
        }


        [WebMethod]
        public List<ConsolidateCpnp> CreateReportCpnp(string yymm)
        {
            var consolidate = new ConsolidateCnpnCollector();
            return consolidate.ConsolidateReportCpnps(yymm);
        }


        [WebMethod]
        public List<ConsolidateOped> CreateReportCOped(string yymmStart,string yymmEnd,List<string> regions)
        {
            var consolidate = new ConsolidateOpedCollector();
            return consolidate.Collect(yymmStart,yymmEnd, regions);
        }

        [WebMethod]
        public List<ConsolidateVSS> CreateReportVSS(string yymm)
        {
            var consolidate = new ConsolidateVSSReportCollector();
            return consolidate.Collect(yymm);
        }

        [WebMethod]
        public List<ConsolidateVCR> CreateReportVCR(string yymm)
        {
            var consolidate = new ConsolidateVCRReportCollector();
            return consolidate.Collect(yymm);
        }


        [WebMethod]
        public List<ConsolidateCpnpM> CreateReportCpnpM(string yymm)
        {
            var consolidate = new ConsolidateCnpnMonthCollector();
            return consolidate.ConsolidateReportCpnpM(yymm);
        }

        [WebMethod]
        public List<CReportVCRFilial> CreateReportVCRFilial(string yymm)
        {
            var consolidate = new ConsolidateVCRReportFilialCollector();
            return consolidate.CreateReportConsolidateVCRFilial(yymm);
        }

        [WebMethod]
        public List<ConsolidateCardio> CreateReportCardio(string yymm)
        {
            var consolidate = new ConsolidateCardioCollector();
            return consolidate.Collect(yymm);
        }

        [WebMethod]
        public List<CReportOpedUnplanned> CreateReportOpedUnplanned(string yymm)
        {
            var consolidate = new ConsolidateOpedUnplannedCollector();
            return consolidate.CreateReportOpedUnplanned(yymm);
        }

        [WebMethod]
        public List<CReportQuantityFilial> CreateReportConsQuantityFilial(string yymm)
        {
            var consolidate = new ConsolidateQuantityFilialCollector();
            return consolidate.CreateReportConsQuantityFilial(yymm);
        }

        [WebMethod]
        public List<CReportCadreTable1> CreateReportCadreTable1(string yymm)
        {
            var consolidate = new ConsolidateCadreCollector();
            return consolidate.CreateReportCadreTable1(yymm);
        }

        [WebMethod]
        public List<CReportCadreTable2> CreateReportCadreTable2(string yymm)
        {
            var consolidate = new ConsolidateCadreCollector();
            return consolidate.CreateReportCadreTable2(yymm);
        }


        //перенес в ConsolidateEndpoint позже удалить
        [WebMethod]
        public List<CReport262Table1> CreateReport262T1(int year)
        {
            var consolidate = new Consolidate262Collector();
            return consolidate.CreateReport262T1(year);
        }

        [WebMethod]
        public List<CReport262Table2> CreateReport262T2(string yymmSt, string yymmEnd)
        {
            var consolidate = new Consolidate262Collector();
            return consolidate.CreateReport262T2(yymmSt, yymmEnd);
        }


        [WebMethod]
        public List<CReport262Table3> CreateReport262T3(string yymm)
        {
            var consolidate = new Consolidate262Collector();
            return consolidate.CreateReport262T3(yymm);
        }

        [WebMethod]
        public List<CReportPg> CreateReportControlPgZpz(string yymm, bool isMonthly)
        {
            var consolidate = new ControlZpzCollector();
            return consolidate.Collect(yymm, isMonthly);
        }

        [WebMethod]
        public List<CReportZpz2023> CreateReportControlZpz2023(string yymm, bool isMonthly)
        {
            var consolidate = new ControlZpz2023Collector();
            return consolidate.Collect(yymm, isMonthly);
        }


        [WebMethod]
        public List<CReportZpz2025> CreateReportControlZpz2025(string yymm, bool isMonthly)
        {
            var consolidate = new ControlZpz2025Collector();
            return consolidate.Collect(yymm, isMonthly);
        }


        [WebMethod]
        public List<ConsolidateOnko> CreateConsolidateOnko(string yymm, bool isMonthly)
        {
            var consolidate = new ConsolidateOnkoCollector();
            return consolidate.Collect(yymm, isMonthly);
        }


        [WebMethod]
        public List<ConsolidateLetal> CreateConsolidateLetal(string yymm)
        {
            var consolidate = new ConsolidateLetalCollector();
            return consolidate.Collect(yymm);
        }

        [WebMethod]
        public List<ConsolidateLetal> CreateConsolidateLetal2023(string yymm)
        {
            var consolidate = new ConsolidateLetal2023Collector();
            return consolidate.Collect(yymm);
        }

        [WebMethod]
        public Report_Dynamic CreateDynamicReport(ReportDynamicDto report) =>
            _dynamicReportHandler.SaveReport(report);



        [WebMethod]
        public Consolidate294 CreateConsolidate294(string yymm)
        {
            var consolidate = new Consolidate294Collector();
            return consolidate.Collect(yymm);
        }


        [WebMethod]
        public List<ZpzForWebSite> CreateZpzForWebSite(string yymm)
        {
            var consolidate = new ZpzForWebSiteCollector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<ZpzForWebSite2023> CreateZpzForWebSite2023(string yymm)
        {
            var consolidate = new ZpzForWebSite2023Collector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<ViolationsOfAppeals> CreateViolationsOfAppeals(string yymm)
        {
            var consolidate = new ViolationsOfAppealsCollector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<CReportZpz2023Full> CreateReportControlZpz2023Full(string year)
        {
            var consolidate = new ControlZpz2023FullCollector();
            return consolidate.Collect(year);
        }

        [WebMethod]
        public List<CReportZpz2023Single> CreateReportControlZpz2023Single(string year, string filial)
        {
            var consolidate = new ControlZpz2023SingleCollector();
            return consolidate.Collect(year,filial);
        }



        [WebMethod]
        public List<ZpzForWebSite2025> CreateZpzForWebSite2025(string yymm)
        {
            var consolidate = new ZpzForWebSite2025Collector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<CReportZpz2025Full> CreateReportControlZpz2025Full(string year)
        {
            var consolidate = new ControlZpz2025FullCollector();
            return consolidate.Collect(year);
        }

        [WebMethod]
        public List<CReportZpz2025Single> CreateReportControlZpz2025Single(string year, string filial)
        {
            var consolidate = new ControlZpz2025SingleCollector();
            return consolidate.Collect(year, filial);
        }


        [WebMethod]
        public List<FFOMSTargetedExp> CreateFFOMSTargetedExp(string yymm)
        {
            var consolidate = new FFOMSTargetedExpCollector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<FFOMSOncoCT> CreateFFOMSOncoCT(string yymm) 
        {
            var consolidate = new FFOMSOncoCTCollector(yymm);
            return consolidate.Collect(yymm);
        }


        [WebMethod]
        public List<FFOMSPersonnel> CreateFFOMSPersonnel(string yymm)
        {
            var consolidate = new FFOMSPersonnelCollector(yymm);
            return consolidate.Collect().Result; // Синхронное ожидание
        }

        [WebMethod]
        public List<FFOMSViolMEE> CreateFFOMSViolMEE(string yymm)
        {
            var consolidate = new FFOMSViolMEECollector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<FFOMSViolEKMP> CreateFFOMSViolEKMP(string yymm)
        {
            var consolidate = new FFOMSViolEKMPCollector(yymm);
            return consolidate.Collect();
        }

        [WebMethod]
        public List<FFOMSVerifyPlan> CreateFFOMSVerifyPlan(string yymm)
        {
            var consolidate = new FFOMSVerifyPlanCollector(yymm);
            return consolidate.Collect();
        }
    }
}