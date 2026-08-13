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

            // 1. Получаем корректные данные Иидвн и Иидн из ЗПЗ (Таблица 10) с группировкой по регионам.
            // .ToDictionary() выполнит запрос и создаст словарь уже в памяти C#.
            var zpzDataByRegion = (from r in db.Report_Zpz2025
                                   join rd in db.Report_Data on r.Id_Report_Data equals rd.Id
                                   join rf in db.Report_Flow on rd.Id_Flow equals rf.Id
                                   where rf.Yymm == yymm && rd.Theme == "Таблица 10"
                                   group r by rf.Id_Region into g
                                   select new
                                   {
                                       Region = g.Key,
                                       Iidvn = g.Sum(x => (x.RowNum == "2.1" || x.RowNum == "2.2" || x.RowNum == "2.3") ? (x.CountSmoAnother ?? 0m) : 0m),
                                       Iidn = g.Sum(x => x.RowNum == "1.4" ? (x.CountSmoAnother ?? 0m) : 0m)
                                   }).ToDictionary(x => x.Region, x => x);

            // 2. Получаем данные из функции БД, группируем их, но НЕ делаем финальную проекцию внутри LINQ to SQL.
            // Мы создаем промежуточный анонимный объект и вызываем .ToList(), чтобы переключиться на LINQ to Objects (память C#).
            var groupedFilialData = (from table in db.cons140n_filials(yymm, "Таблица 1")
                                     where table.Id_Region != "RU-KHA" && table.Id_Region != "RU"
                                     group table by table.Id_Region into x
                                     select new
                                     {
                                         Region = x.Key,
                                         CZLdost = x.Sum(g => g.CZLdost ?? 0),
                                         CZLsmo = x.Sum(g => g.CZLsmo ?? 0),
                                         KSErez = x.Sum(g => g.KSErez ?? 0),
                                         KSE = x.Sum(g => g.KSE ?? 0),
                                         PPMinadvn = x.Sum(g => g.PPMinadvn ?? 0),
                                         FallbackIidvn = x.Sum(g => g.Iidvn ?? 0), // Запасной вариант
                                         PPMinfdn = x.Sum(g => g.PPMinfdn ?? 0),
                                         FallbackIidn = x.Sum(g => g.Iidn ?? 0),   // Запасной вариант
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
                                     }).ToList(); // <--- КЛЮЧЕВОЙ МОМЕНТ: данные теперь в памяти C#

            // 3. Теперь, когда данные в памяти, мы можем БЕЗОПАСНО использовать TryGetValue и условный оператор (?:)
            return groupedFilialData.Select(x => new Cons140nTable1
            {
                Filial = x.Region,
                Data = new Report140nDataDto
                {
                    CZLdost = x.CZLdost,
                    CZLsmo = x.CZLsmo,
                    KSErez = x.KSErez,
                    KSE = x.KSE,
                    PPMinadvn = x.PPMinadvn,

                    // Безопасное обращение к словарю в C#
                    Iidvn = zpzDataByRegion.TryGetValue(x.Region, out var zpzVn) ? zpzVn.Iidvn : x.FallbackIidvn,

                    PPMinfdn = x.PPMinfdn,

                    // Безопасное обращение к словарю в C#
                    Iidn = zpzDataByRegion.TryGetValue(x.Region, out var zpzN) ? zpzN.Iidn : x.FallbackIidn,

                    KOJdosud = x.KOJdosud,
                    KOJsud = x.KOJsud,
                    KOJzl = x.KOJzl,
                    KOJzlsmo = x.KOJzlsmo,
                    KZAsobl = x.KZAsobl,
                    KZAvsego = x.KZAvsego,
                    DT = x.DT,
                    Scpo = x.Scpo,
                    KEKMPpodtv = x.KEKMPpodtv,
                    KEKMPtfoms = x.KEKMPtfoms,
                    KZSMOpodtv = x.KZSMOpodtv,
                    KPMOtfoms = x.KPMOtfoms
                }
            }).ToList();
        }

        public List<Cons140nTable2> CreateCons140nTable2(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);

            // 1. Получаем СУММАРНЫЕ корректные данные Иидвн и Иидн из ЗПЗ (Таблица 10) 
            // с учетом тех же исключений регионов
            var totalZpzData = (from r in db.Report_Zpz2025
                                join rd in db.Report_Data on r.Id_Report_Data equals rd.Id
                                join rf in db.Report_Flow on rd.Id_Flow equals rf.Id
                                where rf.Yymm == yymm
                                   && rd.Theme == "Таблица 10"
                                   && rf.Id_Region != "RU-KHA"
                                   && rf.Id_Region != "RU-LEN"
                                group r by 1 into g
                                select new
                                {
                                    Iidvn = g.Sum(x => (x.RowNum == "2.1" || x.RowNum == "2.2" || x.RowNum == "2.3") ? (x.CountSmoAnother ?? 0m) : 0m),
                                    Iidn = g.Sum(x => x.RowNum == "1.4" ? (x.CountSmoAnother ?? 0m) : 0m)
                                }).FirstOrDefault() ?? new { Iidvn = 0m, Iidn = 0m };

            // 2. Получаем данные из функции БД
            var data = (from table in db.cons140n_filials(yymm, "Таблица 1")
                        where table.Id_Region != "RU-KHA" && table.Id_Region != "RU-LEN"
                        select table).ToList();

            if (!data.Any())
                return new List<Cons140nTable2> { new Cons140nTable2 { Data = new Report140nDataDto() } };

            // 3. Формируем итоговый объект (это уже LINQ to Objects, ошибок не будет)
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

                        Iidvn = totalZpzData.Iidvn, // Подставляем корректную сумму из ЗПЗ
                        
                        PPMinfdn = data.Sum(x => x.PPMinfdn ?? 0),

                        Iidn = totalZpzData.Iidn,   // Подставляем корректную сумму из ЗПЗ
                        
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