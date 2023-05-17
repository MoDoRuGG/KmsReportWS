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
    public class ZpzForWebSite2023Collector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string[] T1Rows = { "3.1", "3.1.1", "3.1.2", "3.1.3", "3.1.4", "3.1.5", "3.1.6", "3.1.7", "3.1.8", "3.1.9", "3.1.10", "3.1.11", "3.1.16", };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public ZpzForWebSite2023Collector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<ZpzForWebSite2023> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU").Select(x => x.id);

            IEnumerable<Task<ZpzForWebSite2023>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<ZpzForWebSite2023> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var treatmentTask = CollectTreatments2023(db, filial);
            var complaintsTask = CollectComplaints2023(db, filial);
            var protectionsTask = CollectProtections2023(db, filial);
            var expertiesesTask = CollectExpertises2023(db, filial);
            var specialistsTask = CollectSpecialists2023(db, filial);
            var informationsTask = CollectInformations2023(db, filial);

            var treatments2023 = await treatmentTask;
            var complaints2023 = await complaintsTask;
            var protections2023 = await protectionsTask;
            var expertises2023 = await expertiesesTask;
            var specialists2023 = await specialistsTask;
            var informations2023 = await informationsTask;

            return new ZpzForWebSite2023
            {
                Filial = filial,
                Treatments = treatments2023,
                Complaints = complaints2023,
                Expertises = expertises2023,
                Protections = protections2023,
                Specialists = specialists2023,
                Informations = informations2023,
            };
        }

        private IQueryable<Report_Zpz> CollectZpzQ(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Zpz on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz_Q"
            select f;

        private IQueryable<Report_Zpz> CollectZpz(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Zpz on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz"
            select f;

        private IQueryable<Report_Zpz> CollectZpz10(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Zpz on data.Id equals f.Id_Report_Data
            where Convert.ToInt32(flow.Yymm) <= Convert.ToInt32(_yymm) && Convert.ToInt32(flow.Yymm)>= Convert.ToInt32(_yymm.Substring(0,2)+"01")
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz10"
            select f;
             

        private async Task<List<ZpzTreatment2023>> CollectTreatments2023(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);
            return new List<ZpzTreatment2023> {
                new ZpzTreatment2023 {
                    Row = "1",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2023 {
                    Row = "3",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2023 {
                    Row = "4",
                    Oral = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2023 {
                    Row = "5",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountAssignment)),
                },
                new ZpzTreatment2023 {
                    Row = "6",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountAssignment)),
                }

            };
        }

        private async Task<List<ZpzTreatment2023>> CollectComplaints2023(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);

            var complaints = new List<ZpzTreatment2023>();
            foreach (string rown in T1Rows)
            {
                var data = table1.Where(x => x.RowNum == rown);
                var complaint = new ZpzTreatment2023
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

        private async Task<List<ZpzStatistics2023>> CollectProtections2023(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 3", region);
            return new List<ZpzStatistics2023>() {
                new ZpzStatistics2023 {
                    Row = "1", Count = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmo))
                },
                new ZpzStatistics2023 {
                    Row = "1.1", Count = Convert.ToInt32(table1.Where(x => x.RowNum == "1.1").Sum(x => x.CountSmo))
                },
                new ZpzStatistics2023 {
                    Row = "2", Count = Convert.ToDecimal(table1.Where(x => x.RowNum == "2").Sum(x => x.CountSmo))
                }
            };
        }

        private async Task<List<Expertise2023>> CollectExpertises2023(LinqToSqlKmsReportDataContext db, string region)
        {
            var meeTable = CollectZpzQ(db, "Таблица 6", region);
            var mee = new Expertise2023
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
            var ekmp = new Expertise2023
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

            var expertises = new List<Expertise2023>() { mee, ekmp };

            return expertises;
        }

        private async Task<List<ZpzStatistics2023>> CollectSpecialists2023(LinqToSqlKmsReportDataContext db, string region)
        {
            var specialistsTable = CollectZpzQ(db, "Таблица 9", region);
            return new List<ZpzStatistics2023>() {
                new ZpzStatistics2023 {
                    Row = "1",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2023 {
                    Row = "1.1.2",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.2")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2023 {
                    Row = "1.1.3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2023 {
                    Row = "3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics2023 {
                    Row = "4",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "4")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                }
            };
        }

        private async Task<List<ZpzStatistics2023>> CollectInformations2023(LinqToSqlKmsReportDataContext db, string region)
        {
            var informTable = CollectZpz10(db, "Таблица 10", region);

            var informations = new List<ZpzStatistics2023>();

            var inform2 = new ZpzStatistics2023
            {
                Row = "2",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "2").Sum(x => x.CountSmo)),
            };
            informations.Add(inform2);

            for (int i = 1; i <= 6; i++)
            {
                string rowNum = $"2.{i}";
                var inform = new ZpzStatistics2023
                {
                    Row = rowNum,
                    Count = Convert.ToInt32(informTable.Where(x => x.RowNum == rowNum).Sum(x => x.CountSmo)),
                };
                informations.Add(inform);
            }

            var inform4 = new ZpzStatistics2023
            {
                Row = "4",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
            };
            informations.Add(inform4);

            for (int i = 1; i <= 8; i++)
            {
                string rowNum = $"4.{i}";
                var inform = new ZpzStatistics2023
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