using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Description;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class FFOMSOncoCTCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt(),  ReportStatus.Saved.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public FFOMSOncoCTCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSOncoCT> Collect(string yymm)
        {

            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            var zpzData = CollectSummaryData(yymm);

            var reports = new List<FFOMSOncoCT>();
            var filials = zpzData.Select(x => x.Filial).Distinct().OrderBy(x => x);
            foreach (var filial in filials)
            {
                var zpzFilialData = zpzData.Where(x => x.Filial == filial);
                var expertise = MapOncoCT(zpzFilialData);

                var report = new FFOMSOncoCT
                {
                    Filial = filial,
                    OncoCT_MEE = expertise,
                };
                reports.Add(report);
            }

            return reports;
        }



        private FFOMSOncoCT_MEE MapOncoCT(IEnumerable<SummaryZpz2025> zpzFilialData)
        {
            var table6Data = zpzFilialData.Where(x => x.Theme == "Таблица 6");
            return new FFOMSOncoCT_MEE
            {
                Target = table6Data.Where(x => x.RowNum == "1.10").Sum(x => x.SumOutOfSmo+x.SumAmbulatory+x.SumDs+x.SumStac)
            };
        }

        private List<SummaryZpz2025> CollectSummaryData(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Zpz2025 on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                        //&& flow.Status != ReportStatus.Refuse.GetDescriptionSt()
                          && flow.Id_Region != "RU-KHA"
                    //&& _themes.Contains(rData.Theme)
                    group new { flow, rData, table } by new { flow.Id_Region, rData.Theme, table.RowNum }
                          into gr
                    select new SummaryZpz2025
                    {
                        Filial = gr.Key.Id_Region,
                        Theme = gr.Key.Theme,
                        RowNum = gr.Key.RowNum,
                        SumSmo = gr.Sum(x => x.table.CountSmo) ?? 0,
                        SumSmoAnother = gr.Sum(x => x.table.CountSmoAnother) ?? 0,
                        SumOutOfSmo = gr.Sum(x => x.table.CountOutOfSmo) ?? 0,
                        SumAmbulatory = gr.Sum(x => x.table.CountAmbulatory) ?? 0,
                        SumDs = gr.Sum(x => x.table.CountDs) ?? 0,
                        SumStac = gr.Sum(x => x.table.CountStac) ?? 0,
                        SumOutOfSmoAnother = gr.Sum(x => x.table.CountOutOfSmoAnother) ?? 0,
                        SumAmbulatoryAnother = gr.Sum(x => x.table.CountAmbulatoryAnother) ?? 0,
                        SumDsAnother = gr.Sum(x => x.table.CountDsAnother) ?? 0,
                        SumStacAnother = gr.Sum(x => x.table.CountStacAnother) ?? 0,
                        SumInsured = gr.Sum(x => x.table.CountInsured) ?? 0,
                        SumInsuredRepresentative = gr.Sum(x => x.table.CountInsuredRepresentative) ?? 0,
                        SumProsecutor = gr.Sum(x => x.table.CountProsecutor) ?? 0,
                    }).ToList();
        }
    }
}