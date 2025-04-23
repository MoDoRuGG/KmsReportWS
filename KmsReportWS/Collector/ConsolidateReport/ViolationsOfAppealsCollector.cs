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
    public class ViolationsOfAppealsCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt(), ReportStatus.Saved.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public ViolationsOfAppealsCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<ViolationsOfAppeals> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);

            IEnumerable<Task<ViolationsOfAppeals>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<ViolationsOfAppeals> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var T1VOATask = CollectForT1VOA(db, filial);
            var T2VOATask = CollectForT2VOA(db, filial);
            var T3VOATask = CollectForT3VOA(db, filial);


            var T1VOA = await T1VOATask;
            var T2VOA = await T2VOATask;
            var T3VOA = await T3VOATask;


            return new ViolationsOfAppeals
            {
                Filial = filial,
                Yymm = _yymm,
                T1 = T1VOA,
                T2 = T2VOA,
                T3 = T3VOA
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

        private IQueryable<Report_Zpz2025> CollectZpz(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Zpz2025 on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz2025"
            select f;


        private async Task<List<ForT1VOA>> CollectForT1VOA(LinqToSqlKmsReportDataContext db, string region)
        {
            // Собираем данные из таблицы 1
            var table1 = CollectZpz(db, "Таблица 1", region);

            // Список всех строк (RowNum), которые нужно обработать
            var rows = new List<string>
            {
                "3", "3.1", "3.1.1", "3.1.2", "3.1.3", "3.1.4", "3.1.5", "3.1.5.1", "3.1.5.2", "3.1.5.3", "3.1.5.4", "3.1.5.5",
                "3.1.5.6", "3.1.5.7", "3.1.5.8", "3.1.5.9", "3.1.5.10", "3.1.5.11", "3.1.6", "3.1.6.1", "3.1.6.2", "3.1.6.3",
                "3.1.6.4", "3.1.6.5", "3.1.6.6", "3.1.6.7", "3.1.6.8", "3.1.6.9", "3.1.6.10", "3.1.6.11", "3.1.6.12", "3.1.6.13",
                "3.1.7", "3.1.7.1", "3.1.7.2", "3.1.7.3", "3.1.7.4", "3.1.7.5", "3.1.8", "3.1.9", "3.1.10", "3.1.11", "3.1.12",
                "3.1.13", "3.1.14", "3.1.15"
            };

            // Генерация списка TreatmentVOA
            var T1VOA = rows.Select(row => new ForT1VOA
            {
                Row = row,
                Oral = Convert.ToInt32(table1.Where(x => x.RowNum == row).Sum(x => x.CountSmo)),
                Written = Convert.ToInt32(table1.Where(x => x.RowNum == row).Sum(x => x.CountSmoAnother)),
                Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == row).Sum(x => x.CountAssignment)),
            }).ToList();

            return T1VOA;
        }



        private async Task<List<ForT2VOA>> CollectForT2VOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var table2 = CollectZpzQ(db, "Таблица 7", region);

            var rows = new List<string>
            {
                "1.9", "1.9.1", "1.9.2", "1.9.3", "1.9.4", "1.9.5", "1.9.6", "1.9.7", "1.9.8"
            };

            // Генерация списка TreatmentVOA
            var T2VOA = rows.Select(row => new ForT2VOA
            {
                Row = row,
                Plan = Convert.ToInt32(table2.Where(x => x.RowNum == row).Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother +
                              x.CountStacAnother)),
                Target = Convert.ToInt32(table2.Where(x => x.RowNum == row).Sum(x => x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
                Violation = Convert.ToInt32(table2.Where(x => x.RowNum == row).Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother + x.CountStacAnother
                              + x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
            }).ToList();

            return T2VOA;
        }


        private async Task<List<ForT3VOA>> CollectForT3VOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var table3 = CollectZpzQ(db, "Таблица 6", region);

            var rows = new List<string>
            {
                "1.8", "1.8.1", "1.8.2", "1.8.3", "1.8.4", "1.8.5", "1.8.6", "1.8.7"
            };

            // Генерация списка TreatmentVOA
            var T3VOA = rows.Select(row => new ForT3VOA
            {
                Row = row,
                Plan = Convert.ToInt32(table3.Where(x => x.RowNum == row).Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother +
                              x.CountStacAnother)),
                Target = Convert.ToInt32(table3.Where(x => x.RowNum == row).Sum(x => x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
                Violation = Convert.ToInt32(table3.Where(x => x.RowNum == row).Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother + x.CountStacAnother
                              + x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
            }).ToList();

            return T3VOA;
        }

    }
}