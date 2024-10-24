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
    public class ZpzForWebSite2025Collector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string[] T1Rows = { "3.1", "3.1.1", "3.1.2", "3.1.3", "3.1.4", "3.1.5", "3.1.6", "3.1.7", "3.1.8", "3.1.9", "3.1.10", "3.1.11", "3.1.16", };

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
            var treatmentTask = CollectTreatments2025(db, filial);
            var complaintsTask = CollectComplaints2025(db, filial);
            var protectionsTask = CollectProtections2025(db, filial);
            var expertiesesTask = CollectExpertises2025(db, filial);
            var specialistsTask = CollectSpecialists2025(db, filial);
            var informationsTask = CollectInformations2025(db, filial);

            var treatments2025 = await treatmentTask;
            var complaints2025 = await complaintsTask;
            var protections2025 = await protectionsTask;
            var expertises2025 = await expertiesesTask;
            var specialists2025 = await specialistsTask;
            var informations2025 = await informationsTask;

            return new ZpzForWebSite2025
            {
                Filial = filial,
                Treatments = treatments2025,
                Complaints = complaints2025,
                Expertises = expertises2025,
                Protections = protections2025,
                Specialists = specialists2025,
                Informations = informations2025,
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

        private IQueryable<Report_Zpz2025> CollectZpz10(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Zpz2025 on data.Id equals f.Id_Report_Data
            where Convert.ToInt32(flow.Yymm) <= Convert.ToInt32(_yymm) && Convert.ToInt32(flow.Yymm)>= Convert.ToInt32(_yymm.Substring(0,2)+"01")
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz10_2025"
            select f;
             

        private async Task<List<ZpzTreatment2025>> CollectTreatments2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);
            return new List<ZpzTreatment2025> {
                new ZpzTreatment2025 {
                    Row = "1",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2025 {
                    Row = "3",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2025 {
                    Row = "4",
                    Oral = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2025 {
                    Row = "5",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2025 {
                    Row = "6",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountAssignment)),
                }

            };
        }

        private async Task<List<ZpzTreatment2025>> CollectComplaints2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);

            var complaints = new List<ZpzTreatment2025>();
            foreach (string rown in T1Rows)
            {
                var data = table1.Where(x => x.RowNum == rown);
                var complaint = new ZpzTreatment2025
                {
                    Row = rown,
                    Oral = Convert.ToInt32(data.Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(data.Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(data.Sum(x => x.CountAssignment)),
                };
                complaints.Add(complaint);
            }
            
            return complaints;
        }

        private async Task<List<ZpzStatistics2025>> CollectProtections2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);
            var table2 = CollectZpz(db, "Таблица 2", region);
            return new List<ZpzStatistics2025>() {
                new ZpzStatistics2025 {
                    Row = "3.1", Count = Convert.ToInt32(table1.Where(x => x.RowNum == "3.1").Sum(x => x.CountSmoAnother))+Convert.ToInt32(table1.Where(x => x.RowNum == "3.1").Sum(x => x.CountSmo))
                },
                new ZpzStatistics2025 {
                    Row = "1", Count = Convert.ToInt32(table2.Where(x => x.RowNum == "1").Sum(x => x.CountSmo))
                },
                new ZpzStatistics2025 {
                    Row = "2", Count = Convert.ToDecimal(table2.Where(x => x.RowNum == "2").Sum(x => x.CountSmo))
                }
            };

        }

        private async Task<List<Expertise2025>> CollectExpertises2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var meeTable = CollectZpzQ(db, "Таблица 6", region);
            var mee = new Expertise2025
            {
                Row = "1",
                Target = Convert.ToInt32(meeTable
                    .Where(x => x.RowNum == "1")
                    .Sum(x => x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
                Plan = Convert.ToInt32(meeTable
                    .Where(x => x.RowNum == "1")
                    .Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother +
                              x.CountStacAnother)),
                Violation = Convert.ToInt32(meeTable
                    .Where(x => x.RowNum == "3")
                    .Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother + x.CountStacAnother
                              + x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
            };

            var ekmpTable = CollectZpzQ(db, "Таблица 7", region);
            var ekmp = new Expertise2025
            {
                Row = "2",
                Target = Convert.ToInt32(ekmpTable
                    .Where(x => x.RowNum == "1")
                    .Sum(x => x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
                Plan = Convert.ToInt32(ekmpTable
                    .Where(x => x.RowNum == "1")
                    .Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother +
                              x.CountStacAnother)),
                Violation = Convert.ToInt32(ekmpTable
                    .Where(x => x.RowNum == "5")
                    .Sum(x => x.CountOutOfSmoAnother + x.CountAmbulatoryAnother + x.CountDsAnother + x.CountStacAnother
                              + x.CountOutOfSmo + x.CountAmbulatory + x.CountDs + x.CountStac)),
            };

            var expertises = new List<Expertise2025>() { mee, ekmp };

            return expertises;
        }

        private async Task<List<ZpzStatistics2025>> CollectSpecialists2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var specialistsTable = CollectZpzQ(db, "Таблица 9", region);
            return new List<ZpzStatistics2025>() {
                new ZpzStatistics2025 {
                    Row = "1",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2025 {
                    Row = "1.1.2",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.2")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2025 {
                    Row = "1.1.3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2025 {
                    Row = "3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2025 {
                    Row = "4",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "4")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                }
            };
        }

        private async Task<List<ZpzStatistics2025>> CollectInformations2025(LinqToSqlKmsReportDataContext db, string region)
        {
            var informTable = CollectZpz10(db, "Таблица 10", region);

            var informations = new List<ZpzStatistics2025>();

            var inform2 = new ZpzStatistics2025
            {
                Row = "2",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "2").Sum(x => x.CountSmo)),
            };
            informations.Add(inform2);

            for (int i = 1; i <= 6; i++)
            {
                string rowNum = $"2.{i}";
                var inform = new ZpzStatistics2025
                {
                    Row = rowNum,
                    Count = Convert.ToInt32(informTable.Where(x => x.RowNum == rowNum).Sum(x => x.CountSmo)),
                };
                informations.Add(inform);
            }

            var inform4 = new ZpzStatistics2025
            {
                Row = "4",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
            };
            informations.Add(inform4);

            for (int i = 1; i <= 8; i++)
            {
                string rowNum = $"4.{i}";
                var inform = new ZpzStatistics2025
                {
                    Row = rowNum,
                    Count = Convert.ToInt32(informTable.Where(x => x.RowNum == rowNum).Sum(x => x.CountSmo)),
                };
                informations.Add(inform);
            }

            return informations;
        }
    }
}