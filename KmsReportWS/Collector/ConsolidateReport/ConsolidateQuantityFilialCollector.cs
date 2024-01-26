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
    public class ConsolidateQuantityFilialCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReportQuantityFilial> CreateReportConsQuantityFilial(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.Cons_Quantity_Filials(yymm)         //  функция вывода табличного значения в SQL
            where table.Id_Region != "RU-KHA" //&& table.Id_Region != "RU-LEN"
                    group new { table } by new { table.Id_Region }
                into x
                    select new CReportQuantityFilial
                    {
                        Filial = x.Key.Id_Region,
                        Data = new ReportQuantity
                        {
                            Col_1 = x.Sum(g => g.table.Col_1 ?? 0),
                            Col_2 = x.Sum(g => g.table.Col_2 ?? 0),
                            Col_3 = x.Sum(g => g.table.Col_3 ?? 0),
                            Col_4 = x.Sum(g => g.table.Col_4 ?? 0),
                            Col_5 = x.Sum(g => g.table.Col_5 ?? 0),
                            Col_6 = x.Sum(g => g.table.Col_6 ?? 0),
                            Col_7 = x.Sum(g => g.table.Col_7 ?? 0),
                            Col_8 = x.Sum(g => g.table.Col_8 ?? 0),
                            Col_9 = x.Sum(g => g.table.Col_9 ?? 0),
                            Col_10 = x.Sum(g => g.table.Col_10 ?? 0),
                            Col_11 = x.Sum(g => g.table.Col_11 ?? 0),
                            Col_12 = x.Sum(g => g.table.Col_12 ?? 0),
                            Col_13 = x.Sum(g => g.table.Col_13 ?? 0),
                            Col_14 = x.Sum(g => g.table.Col_14 ?? 0),
                            Col_15 = x.Sum(g => g.table.Col_15 ?? 0),
                            Col_16 = x.Sum(g => g.table.Col_16 ?? 0)

                        }
                    }).ToList();
        }
    }
}