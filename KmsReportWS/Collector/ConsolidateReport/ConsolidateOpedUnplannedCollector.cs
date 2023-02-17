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
    public class ConsolidateOpedUnplannedCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReportOpedUnplanned> CreateReportOpedUnplanned(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.opedU_report(yymm)         //  функция вывода табличного значения в SQL
                    group new { table } by new { table.id, table.RowNum, table.App, table.Ks, table.Ds, table.Smp, table.Notes, table.NotesGoodReason }
                into x
                    select new CReportOpedUnplanned
                    {
                        Filial = x.Key.id,
                        RowNum = x.Key.RowNum,
                        App = x.Sum(g => g.table.App ?? 0),
                        Ks = x.Sum(g => g.table.Ks ?? 0),
                        Ds = x.Sum(g => g.table.Ds ?? 0),
                        Smp = x.Sum(g => g.table.Smp ?? 0),
                        Notes = x.Key.Notes,
                        NotesGoodReason = x.Key.NotesGoodReason,
                        //Data = new ReportOpedUDto
                        //{
                        //    App = x.Sum(g => g.table.App ?? 0),
                        //    Ks = x.Sum(g => g.table.Ks ?? 0),
                        //    Ds = x.Sum(g => g.table.Ds ?? 0),
                        //    Smp = x.Sum(g => g.table.Smp ?? 0),
                        //}

                    }).ToList();
        }
    }
}