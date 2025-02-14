using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using KmsReportWS.Model.ConcolidateReport;
using System.Threading.Tasks;

namespace KmsReportWS.Collector.ConsolidateReport
{

    public class ZpzForWebSite2025Collector
    {
        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public ZpzForWebSite2025Collector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<ZpzForWebSite2025> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);

            IEnumerable<Task<ZpzForWebSite2025>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<ZpzForWebSite2025> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var wsTask = CollectWS2025(db, filial);


            var ws2025 = await wsTask;


            return new ZpzForWebSite2025
            {
                Filial = filial,
                WSData = ws2025,

            };
        }

        private IQueryable<WSData2025> CollectTable(LinqToSqlKmsReportDataContext db, string region) =>
    from table in db.ZpzWebSite2025(region, _yymm)
    where table.Id_Region == region
    select new WSData2025
    {
        Col1 = table.col1 ?? 0,
        Col2 = table.col2 ?? 0,
        Col3 = table.col3 ?? 0,
        Col4 = table.col4 ?? 0,
        Col5 = table.col5 ?? 0,
        Col6 = table.col6 ?? 0,
        Col8 = table.col8 ?? 0,
        Col9 = table.col9 ?? 0,
        Col10 = table.col10 ?? 0,
        Col11 = table.col11 ?? 0,
        Col12 = table.col12 ?? 0,
        Col13 = table.col13 ?? 0,
        Col14 = table.col14 ?? 0,
    };

        private Task<List<WSData2025>> CollectWS2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var table = CollectTable(db, region);

            // Выполняем запрос синхронно
            return Task.FromResult(table.Select(row => new WSData2025
            {
                Col1 = row.Col1,
                Col2 = row.Col2,
                Col3 = row.Col3,
                Col4 = row.Col4,
                Col5 = row.Col5,
                Col6 = row.Col6,
                Col8 = row.Col8,
                Col9 = row.Col9,
                Col10 = row.Col10,
                Col11 = row.Col11,
                Col12 = row.Col12,
                Col13 = row.Col13,
                Col14 = row.Col14,
            }).ToList());
        }
    }
}



