using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using static KmsReportWS.Model.ConcolidateReport.FFOMSLethalEKMP;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class FFOMSLethalEKMPCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;
        public List<FFOMSLethalEKMP> CreateFFOMSLethalEKMP(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.FFOMSLethalEKMP(yymm)         //  функция вывода табличного значения в SQL
                    where table.Id_Region != "RU-KHA" && table.Id_Region != "RU"

                    select new FFOMSLethalEKMP
                    {
                        Code = table.Id_Region,
                        Filial = table.name,
                        Row1 = table.Row1 ?? 0,
                        Row12 = table.Row12 ?? 0,
                        Row121 = table.Row121 ?? 0,
                        Row11 = table.Row11 ?? 0,

                    }).ToList();
        }
    }
}