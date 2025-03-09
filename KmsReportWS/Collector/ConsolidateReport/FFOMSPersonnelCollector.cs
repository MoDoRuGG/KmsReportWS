using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using NLog;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class FFOMSPersonnelCollector
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string[] Statuses = { ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt() };
        private static readonly string ConnStr = Settings.Default.ConnStr;
        private readonly string _yymm;

        public FFOMSPersonnelCollector(string yymm)
        {
            _yymm = yymm;
        }

        public async Task<List<FFOMSPersonnel>> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region
                .Where(x => x.id != "RU" && x.id != "RU-KHA")
                .Select(x => x.id)
                .ToList();

            var tasks = filials.Select(filial => CollectFilialDataAsync(filial));
            // Исправлено:
            var results = await Task.WhenAll(tasks); // Получаем массив FFOMSPersonnel[]
            return results.ToList(); // Преобразуем массив в List<FFOMSPersonnel>
        }

        private async Task<FFOMSPersonnel> CollectFilialDataAsync(string filial)
        {
            try
            {
                using var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var personnel = CollectPersonnel(db, filial);
                return new FFOMSPersonnel { Filial = filial, PersonnelT9 = personnel };
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Ошибка при сборе данных для филиала {filial}");
                return new FFOMSPersonnel { Filial = filial };
            }
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

        // Добавьте класс:
        private class Table9DataItem
        {
            public decimal? SumCountSmo { get; set; }
            public decimal? SumCountSmoAnother { get; set; }
        }

        private List<PersonnelT9> CollectPersonnel(LinqToSqlKmsReportDataContext db, string region)
        {
            var rowsToProcess = new[] { "1", "1.1", "1.1.1", "1.1.2", "1.1.3", "2", "3", "3.1", "3.2", "3.3", "4", "4.1", "4.2" };

            var table9Data = CollectZpzQ(db, "Таблица 9", region)
                .GroupBy(f => f.RowNum)
                .ToDictionary(g => g.Key, g => new Table9DataItem
                {
                    SumCountSmo = g.Sum(x => x.CountSmo),
                    SumCountSmoAnother = g.Sum(x => x.CountSmoAnother)
                });

            return rowsToProcess.Select(row =>
            {
                if (table9Data.TryGetValue(row, out var data))
                {
                    return new PersonnelT9
                    {
                        Row = row,
                        FullTime = (int)(data.SumCountSmo ?? 0),
                        Contract = (int)(data.SumCountSmoAnother ?? 0)
                    };
                }
                else
                {
                    return new PersonnelT9 { Row = row, FullTime = 0, Contract = 0 };
                }
            }).ToList();
        }
    }
}