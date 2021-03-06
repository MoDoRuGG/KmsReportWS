using System.Collections.Generic;
using System.Web.Services;
using KmsReportWS.Collector.ConsolidateReport;
using KmsReportWS.Model.ConcolidateReport;

namespace KmsReportWS
{
    /// <summary>
    /// Сводное описание для ConsolidateEndpoint
    /// </summary>
    [WebService(Namespace = "http://kms-oms.ru/consolidate")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class ConsolidateEndpoint : WebService
    {

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
        public List<ConsolidateCpnp> CreateReportCpnp(string yymm)
        {
            var consolidate = new ConsolidateCnpnCollector();
            return consolidate.ConsolidateReportCpnps(yymm);
        }

        [WebMethod]
        public List<CReportPg> CreateReportControlPgZpz(string yymm, bool isMonthly)
        {
            var consolidate = new ControlZpzCollector();
            return consolidate.Collect(yymm, isMonthly);
        }

        [WebMethod]
        public Consolidate294 CreateConsolidate294(string yymm)
        {
            var consolidate = new Consolidate294Collector();
            return consolidate.Collect(yymm);
        }

        [WebMethod]
        public List<ZpzForWebSite> CreateZpzForWebSite(string yymmStart)
        {
            var consolidate = new ZpzForWebSiteCollector(yymmStart);
            return consolidate.Collect();
        }
    }
}
