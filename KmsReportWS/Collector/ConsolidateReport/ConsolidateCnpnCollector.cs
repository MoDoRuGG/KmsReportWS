using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateCnpnCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<ConsolidateCpnp> ConsolidateReportCpnps(string yymm)
        {
            string reportType =  "PG_Q";

            int year = YymmUtils.ConvertYymmToYear(yymm);

            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };

            var reports = new List<ConsolidateCpnp>();

            reports = (from flow in db.Report_Flow
                       join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                       join table in db.Report_Pg on rData.Id equals table.Id_Report_Data
                       join r in db.Region on flow.Id_Region equals r.id
                       join cpnpRegion in db.NormativCpnpRegion on r.id equals cpnpRegion.id_region
                       join cpnpFederal in db.NormativCpnpFederal on year equals cpnpFederal.year
                       where flow.Yymm == yymm
                       && flow.Id_Report_Type ==reportType
                       //&& flow.Status != ReportStatus.Refuse.GetDescription()
                       && (rData.Theme == "Таблица 2" || rData.Theme == "Таблица 1")
                       && cpnpRegion.year==year
                       group new { flow, table,r } by new { flow.Id_Region,r.name, FedValue = cpnpFederal.value, RegValue = cpnpRegion.value }
                       into flowGr
                       select new ConsolidateCpnp
                       {
                           Filial = flowGr.Key.name,
                           CountPretrial = flowGr.Where(x => x.table.RowNum == "1.1" && x.table.Report_Data.Theme == "Таблица 2").Sum(x => x.table.CountSmo) ?? 0,
                           CountAll = flowGr.Where(x => x.table.RowNum == "2" && x.table.Report_Data.Theme == "Таблица 1").Sum(x => x.table.CountSmo + x.table.CountSmoAnother) ?? 0,
                           NormativRegionCpnp = flowGr.Key.RegValue,
                           NormativFederalCpnp = flowGr.Key.FedValue
                       }).ToList();

            return reports;



        }




    }
}