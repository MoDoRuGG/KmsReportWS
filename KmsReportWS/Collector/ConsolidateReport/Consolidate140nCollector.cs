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
    public class Consolidate140nCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<Cons140nTable1> CreateCons140nTable1(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.cons140n_filials(yymm, "Таблица 1")
                    where table.Id_Region != "RU-KHA" && table.Id_Region != "RU"
                    group table by new { table.Id_Region } into x
                    select new Cons140nTable1
                    {
                        Filial = x.Key.Id_Region,
                        Data = new Report140nDataDto
                        {
                            CZLdost = x.Sum(g => g.CZLdost ?? 0),
                            CZLsmo = x.Sum(g => g.CZLsmo ?? 0),
                            KSErez = x.Sum(g => g.KSErez ?? 0),
                            KSE = x.Sum(g => g.KSE ?? 0),
                            PPMinadvn = x.Sum(g => g.PPMinadvn ?? 0),
                            Iidvn = x.Sum(g => g.Iidvn ?? 0),
                            PPMinfdn = x.Sum(g => g.PPMinfdn ?? 0),
                            Iidn = x.Sum(g => g.Iidn ?? 0),
                            KOJdosud = x.Sum(g => g.KOJdosud ?? 0),
                            KOJsud = x.Sum(g => g.KOJsud ?? 0),
                            KOJzl = x.Sum(g => g.KOJzl ?? 0),
                            KOJzlsmo = x.Sum(g => g.KOJzlsmo ?? 0),
                            KZAsobl = x.Sum(g => g.KZAsobl ?? 0),
                            KZAvsego = x.Sum(g => g.KZAvsego ?? 0),
                            DT = x.Sum(g => g.DT ?? 0),
                            Scpo = x.Sum(g => g.Scpo ?? 0),
                            KEKMPpodtv = x.Sum(g => g.KEKMPpodtv ?? 0),
                            KEKMPtfoms = x.Sum(g => g.KEKMPtfoms ?? 0),
                            KZSMOpodtv = x.Sum(g => g.KZSMOpodtv ?? 0),
                            KPMOtfoms = x.Sum(g => g.KPMOtfoms ?? 0)
                        }
                    }).ToList();
        }

        public List<Cons140nTable2> CreateCons140nTable2(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var data = (from table in db.cons140n_filials(yymm, "Таблица 1")
                        where table.Id_Region != "RU-KHA" && table.Id_Region != "RU-LEN"
                        select table).ToList();

            if (!data.Any())
                return new List<Cons140nTable2> { new Cons140nTable2 { Data = new Report140nDataDto() } };

            return new List<Cons140nTable2>
            {
                new Cons140nTable2
                {
                    Data = new Report140nDataDto
                    {
                        CZLdost = data.Sum(x => x.CZLdost ?? 0),
                        CZLsmo = data.Sum(x => x.CZLsmo ?? 0),
                        KSErez = data.Sum(x => x.KSErez ?? 0),
                        KSE = data.Sum(x => x.KSE ?? 0),
                        PPMinadvn = data.Sum(x => x.PPMinadvn ?? 0),
                        Iidvn = data.Sum(x => x.Iidvn ?? 0),
                        PPMinfdn = data.Sum(x => x.PPMinfdn ?? 0),
                        Iidn = data.Sum(x => x.Iidn ?? 0),
                        KOJdosud = data.Sum(x => x.KOJdosud ?? 0),
                        KOJsud = data.Sum(x => x.KOJsud ?? 0),
                        KOJzl = data.Sum(x => x.KOJzl ?? 0),
                        KOJzlsmo = data.Sum(x => x.KOJzlsmo ?? 0),
                        KZAsobl = data.Sum(x => x.KZAsobl ?? 0),
                        KZAvsego = data.Sum(x => x.KZAvsego ?? 0),
                        DT = data.Sum(x => x.DT ?? 0),
                        Scpo = data.Sum(x => x.Scpo ?? 0),
                        KEKMPpodtv = data.Sum(x => x.KEKMPpodtv ?? 0),
                        KEKMPtfoms = data.Sum(x => x.KEKMPtfoms ?? 0),
                        KZSMOpodtv = data.Sum(x => x.KZSMOpodtv ?? 0),
                        KPMOtfoms = data.Sum(x => x.KPMOtfoms ?? 0)
                    }
                }
            };
        }
    }
}