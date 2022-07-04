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
    public class ConsolidateLetalCollector
    {
        private readonly string[] _themes =
         { "Таблица 1Л"};



        public List<ConsolidateLetal> Collect(string yymm)
        {
            string reportType = "PG_Q";

            var pgData = CollectSummaryData(yymm, reportType);

            var reports = new List<ConsolidateLetal>();

            var filials = pgData.Select(x => x.Filial).Distinct().OrderBy(x => x);

            foreach (var filial in filials)
            {
                var pgFilialData = pgData.Where(x => x.Filial == filial);
                var data = MapLetal(pgFilialData);
               
                var report = new ConsolidateLetal
                {
                    Filial = filial,
                    Data = data


                };
                reports.Add(report);
            }

            return reports;

        }



        private LetalData MapLetal(IEnumerable<SummaryPg> pgFilialData)
        {
            var tableData = pgFilialData.Where(x => x.Theme == "Таблица 1Л");
            return new LetalData
            {
                r1 = tableData.Where(x => x.RowNum == "1").Sum(x => x.SumSmo),
                r1_1 = tableData.Where(x => x.RowNum == "1.1").Sum(x => x.SumSmo),
                r1_2 = tableData.Where(x => x.RowNum == "1.2").Sum(x => x.SumSmo),
                r121 = tableData.Where(x => x.RowNum == "1.2.1").Sum(x => x.SumSmo),
                r2 = tableData.Where(x => x.RowNum == "2").Sum(x => x.SumSmo),
                r3 = tableData.Where(x => x.RowNum == "3").Sum(x => x.SumSmo),
                r31 = tableData.Where(x => x.RowNum == "3.1.1").Sum(x => x.SumSmo),
                r311 = tableData.Where(x => x.RowNum == "3.1.1").Sum(x => x.SumSmo),
                r3111 = tableData.Where(x => x.RowNum == "3.1.1.1").Sum(x => x.SumSmo),
                r3112 = tableData.Where(x => x.RowNum == "3.1.1.2").Sum(x => x.SumSmo),
                r3113 = tableData.Where(x => x.RowNum == "3.1.1.3").Sum(x => x.SumSmo),
                r3114 = tableData.Where(x => x.RowNum == "3.1.1.4").Sum(x => x.SumSmo),
                r32 = tableData.Where(x => x.RowNum == "3.2").Sum(x => x.SumSmo),
                r33 = tableData.Where(x => x.RowNum == "3.3").Sum(x => x.SumSmo),
                r4 = tableData.Where(x => x.RowNum == "4").Sum(x => x.SumSmo),
                r5 = tableData.Where(x => x.RowNum == "5").Sum(x => x.SumSmo),
                r6 = tableData.Where(x => x.RowNum == "6").Sum(x => x.SumSmo),
                r7 = tableData.Where(x => x.RowNum == "7").Sum(x => x.SumSmo),
                r8 = tableData.Where(x => x.RowNum == "8").Sum(x => x.SumSmo),
                r9 = tableData.Where(x => x.RowNum == "9").Sum(x => x.SumSmo),
                r10 = tableData.Where(x => x.RowNum == "10").Sum(x => x.SumSmo),
                r11 = tableData.Where(x => x.RowNum == "11").Sum(x => x.SumSmo),
                r12 = tableData.Where(x => x.RowNum == "12").Sum(x => x.SumSmo),
                r13 = tableData.Where(x => x.RowNum == "13").Sum(x => x.SumSmo),

            };
        }


        private List<SummaryPg> CollectSummaryData(string yymm, string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join reg in db.Region on flow.Id_Region equals reg.id
                    join table in db.Report_Pg on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                          && flow.Status != ReportStatus.Refuse.GetDescription()
                          && flow.Id_Report_Type == reportType
                          && _themes.Contains(rData.Theme)
                    group new { flow, rData, table } by new { flow.Id_Region, reg.name, rData.Theme, table.RowNum }
                          into gr
                    select new SummaryPg
                    {
                        Filial = gr.Key.name,
                        Theme = gr.Key.Theme,
                        RowNum = gr.Key.RowNum,
                        SumSmo = gr.Sum(x => x.table.CountSmo) ?? 0,
                        SumSmoAnother = gr.Sum(x => x.table.CountSmoAnother) ?? 0,
                        SumOutOfSmo = gr.Sum(x => x.table.CountOutOfSmo) ?? 0,
                        SumAmbulatory = gr.Sum(x => x.table.CountAmbulatory) ?? 0,
                        SumDs = gr.Sum(x => x.table.CountDs) ?? 0,
                        SumStac = gr.Sum(x => x.table.CountStac),
                        SumOutOfSmoAnother = gr.Sum(x => x.table.CountOutOfSmoAnother) ?? 0,
                        SumAmbulatoryAnother = gr.Sum(x => x.table.CountAmbulatoryAnother) ?? 0,
                        SumDsAnother = gr.Sum(x => x.table.CountDsAnother) ?? 0,
                        SumStacAnother = gr.Sum(x => x.table.CountStacAnother) ?? 0,
                        SumInsured = gr.Sum(x => x.table.CountInsured) ?? 0,
                        SumInsuredRepresentative = gr.Sum(x => x.table.CountInsuredRepresentative) ?? 0,
                        SumProsecutor = gr.Sum(x => x.table.CountProsecutor) ?? 0,
                        SumTfoms = gr.Sum(x => x.table.CountTfoms) ?? 0,
                        SumVmp = gr.Sum(x => x.table.CountStacVmp) ?? 0
                    }).ToList();
        }

    }



}