using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateOpedUnplanned2026Collector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReportOpedUnplanned2026> CreateReportOpedUnplanned2026(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.opedUnplanned_report(yymm)         //  функция вывода табличного значения в SQL
                    group new { table } by new { table.id, table.RowNum, table.App, table.Ks, table.Ds, table.Smp, table.Notes}
                into x
                    select new CReportOpedUnplanned2026
                    {
                        Filial = x.Key.id,
                        RowNum = x.Key.RowNum,
                        App = x.Sum(g => g.table.App),
                        Ks = x.Sum(g => g.table.Ks),
                        Ds = x.Sum(g => g.table.Ds),
                        Smp = x.Sum(g => g.table.Smp),
                        Notes = x.Key.Notes

                    }).ToList();
        }
    }
}