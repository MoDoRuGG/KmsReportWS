using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class FFOMSViolMEECollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public FFOMSViolMEECollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSViolMEE> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);
            IEnumerable<Task<FFOMSViolMEE>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
            
            
        }

        private async Task<FFOMSViolMEE> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var T1FFOMSMEETask = CollectFFOMSMEE(db, filial);

            var ViolMEE = await T1FFOMSMEETask;


            return new FFOMSViolMEE
            {
                Filial = filial,
                DataViolMEE = ViolMEE,
            };
        }

        private IQueryable<Report_Violations> CollectViolMEE(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Violations on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == "Нарушения МЭЭ"
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "ViolMEE"
            select f;


        private async Task<List<FFOMSViolMEEdata>> CollectFFOMSMEE(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectViolMEE(db, "Нарушения МЭЭ", region);

            var rows = new List<string>
            {
                "2.1", "2.2", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12", "2.13", "2.14", "2.15", "2.16.1", "2.16.2", "2.16.3", "2.17", "2.18",
            };

            // Генерация списка 
            var T1FFOMSMEE = rows.Select(row => new FFOMSViolMEEdata
            {
                RowNum = row,
                Count = Convert.ToInt32(table1.Where(x => x.RowNum == row).Sum(x => x.Count)),
            }).ToList();

            return T1FFOMSMEE;
        }
    }
}