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
    public class ConsolidateCardiCollector
    {
        private readonly string[] _themes =
        {  "Таблица 11" };



        public List<ConsolidateCadri> Collect(string yymm)
        {
            string reportType = "PG_Q";

            var pgData = CollectSummaryData(yymm, reportType);

            var reports = new List<ConsolidateCadri>();

            var filials = pgData.Select(x => x.Filial).Distinct().OrderBy(x => x);

            foreach (var filial in filials)
            {
                var pgFilialData = pgData.Where(x => x.Filial == filial);
                var cardi = MapCardi(pgFilialData);

                var report = new ConsolidateCadri
                {
                    Filial = filial,
                    data = MapCardi(pgFilialData)

                };
                reports.Add(report);
            }

            return reports;

        }

        private ConsolidateCardiData MapCardi(IEnumerable<SummaryPg> pgFilialData)
        {
            var table11Data = pgFilialData.Where(x => x.Theme == "Таблица 11");

            return new ConsolidateCardiData
            {
                r1 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1").Sum(x => x.SumSmoAnother),
                },

                r11 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1").Sum(x => x.SumSmoAnother),
                },

                r111 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.1").Sum(x => x.SumSmoAnother),
                },
                r112 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.2").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.2").Sum(x => x.SumSmoAnother),
                },

                r113 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.3").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.3").Sum(x => x.SumSmoAnother),
                },

                r1131 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.3.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.3.1").Sum(x => x.SumSmoAnother),
                },
                r11311 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.3.1.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.3.1.1").Sum(x => x.SumSmoAnother),
                },
                r1132 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.3.2").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.3.2").Sum(x => x.SumSmoAnother),
                },

                r11321 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "1.1.3.2.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "1.1.3.2.1").Sum(x => x.SumSmoAnother),
                },
                r2 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "2").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "2").Sum(x => x.SumSmoAnother),
                },
                r21 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "2.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "2.1").Sum(x => x.SumSmoAnother),
                },

                r3 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "3").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "3").Sum(x => x.SumSmoAnother),
                },

                r31 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "3.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "3.1").Sum(x => x.SumSmoAnother),
                },

                r32 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "3.2").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "3.2").Sum(x => x.SumSmoAnother),
                },

                r33 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "3.3").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "3.3").Sum(x => x.SumSmoAnother),
                },

                r4 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "4").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "4").Sum(x => x.SumSmoAnother),
                },

                r41 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "4.1").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "4.1").Sum(x => x.SumSmoAnother),
                },

                r42 = new ConsolidateCardiDataF
                {
                    IzNih1 = table11Data.Where(x => x.RowNum == "4.2").Sum(x => x.SumSmo),
                    IzNih2 = table11Data.Where(x => x.RowNum == "4.2").Sum(x => x.SumSmoAnother),
                },



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