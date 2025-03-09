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
    public class FFOMSOncoCTCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public FFOMSOncoCTCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSOncoCT> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);

            IEnumerable<Task<FFOMSOncoCT>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<FFOMSOncoCT> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var MEETask = CollectMEE(db, filial);


            var T1MEE = await MEETask;


            return new FFOMSOncoCT
            {
                Filial = filial,
                OncoCT_MEE = T1MEE,
            };
        }

        private IQueryable<Report_Zpz2025> CollectZpzQ(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Zpz2025 on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz_Q2025"
            select f;


        private async Task<List<OncoCT_MEE>> CollectMEE(LinqToSqlKmsReportDataContext db, string region)
        {
            var meeTable = CollectZpzQ(db, "Таблица 6", region);
            return new List<OncoCT_MEE>() {

                new OncoCT_MEE {
                    Row = "1.10", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountOutOfSmo)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountAmbulatory)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountDs)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountStac))
                },
            };
        }
    }
}