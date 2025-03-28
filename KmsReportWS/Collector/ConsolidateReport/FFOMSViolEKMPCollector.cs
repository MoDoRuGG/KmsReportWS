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
    public class FFOMSViolEKMPCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public FFOMSViolEKMPCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSViolEKMP> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);
            IEnumerable<Task<FFOMSViolEKMP>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
            
            
        }

        private async Task<FFOMSViolEKMP> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var T2FFOMSEKMPTask = CollectFFOMSEKMP(db, filial);

            var ViolEKMP = await T2FFOMSEKMPTask;

            return new FFOMSViolEKMP
            {
                Filial = filial,
                DataViolEKMP = ViolEKMP,
            };
        }

        private IQueryable<Report_Violations> CollectViolEKMP(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Violations on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == "Нарушения ЭКМП"
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "ViolEKMP"
            select f;



        private async Task<List<FFOMSViolEKMPdata>> CollectFFOMSEKMP(LinqToSqlKmsReportDataContext db, string region)
        {
            var table2 = CollectViolEKMP(db, "Нарушения ЭКМП", region);

            var rows = new List<string>
            {
                "2.1","2.2","2.7","2.8","2.9","2.10","2.11","2.12","2.13",
                "2.14","2.15","2.16.1","2.16.2","2.16.3","2.17","2.18",
                "3.1.1","3.1.2","3.1.3","3.1.4","3.1.5","3.2.1","3.2.2",
                "3.2.3","3.2.4","3.2.5","3.2.6","3.3","3.4","3.5","3.6",
                "3.7","3.8","3.9","3.10","3.11","3.12","3.13","3.14.1",
                "3.14.2","3.14.3","3.15.1","3.15.2","3.15.3"
            };

            var T2FFOMSEKMP = rows.Select(row => new FFOMSViolEKMPdata
            {
                RowNum = row,
                Count = Convert.ToInt32(table2.Where(x => x.RowNum == row).Sum(x => x.Count)),
            }).ToList();

            return T2FFOMSEKMP;
        }
    }
}