using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities.Collections;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateOpedUCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReportOpedU> CollectOpedUData(string yymm, string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from regi in db.Region
                    join flow in db.Report_Flow on regi.id equals flow.Id_Region
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_OpedU on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                          && flow.Status != ReportStatus.Refuse.GetDescriptionSt()
                          && flow.Id_Report_Type == reportType
                          && table.RowNum != null
                    group new { regi, flow, rData, table } by new { regi.name, table.RowNum, table.App, 
                                                                    table.Ks, table.Ds, table.Smp, table.Notes, }

                          into gr


                    select new CReportOpedU

                    {
                        Filial = gr.Key.name,
                        Data = new ReportOpedUDataDto
                        {
                            RowNum = gr.Key.RowNum,
                            App = gr.Key.App,
                            Ks = gr.Key.Ks,
                            Ds = gr.Key.Ds,
                            Smp = gr.Key.Smp,
                            Notes = gr.Key.Notes,
                        }
                        
                    }).ToList();
        }
    }
}