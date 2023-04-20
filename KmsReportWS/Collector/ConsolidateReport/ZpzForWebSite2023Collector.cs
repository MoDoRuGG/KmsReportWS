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

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public ZpzForWebSite2023Collector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<ZpzForWebSite> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU").Select(x => x.id);

            IEnumerable<Task<ZpzForWebSite>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<ZpzForWebSite> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var treatmentTask = CollectTreatments(db, filial);
            var complaintsTask = CollectComplaints(db, filial);
            var protectionsTask = CollectProtections(db, filial);
            var expertiesesTask = CollectExpertises(db, filial);
            var specialistsTask = CollectSpecialists(db, filial);
            var informationsTask = CollectInformations(db, filial);

            var treatments = await treatmentTask;
            var complaints = await complaintsTask;
            var protections = await protectionsTask;
            var expertises = await expertiesesTask;
            var specialists = await specialistsTask;
            var informations = await informationsTask;

            return new ZpzForWebSite
            {
                Filial = filial,
                Treatments = treatments,
                Complaints = complaints,
                Expertises = expertises,
                Protections = protections,
                Specialists = specialists,
                Informations = informations,
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
            where flow.Yymm == _yymm
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz10"
            select f;

        private async Task<List<ZpzTreatment>> CollectTreatments(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);
            return new List<ZpzTreatment> {
                new ZpzTreatment {
                    Row = "2",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmoAnother))
                },
                new ZpzTreatment {
                    Row = "3",
                    Oral = Convert.ToInt32(table1
                        .Where(x => x.RowNum.StartsWith("4") && x.RowNum.Length <= 4).Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1
                        .Where(x => x.RowNum.StartsWith("4") && x.RowNum.Length <= 4).Sum(x => x.CountSmoAnother))
                },
                new ZpzTreatment {
                    Row = "4",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmoAnother))
                },
                new ZpzTreatment {
                    Row = "5",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmoAnother))
                }

            };
        }

        private async Task<List<ZpzTreatment>> CollectComplaints(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);

            var complaints = new List<ZpzTreatment>();
            for (int i = 1; i <= 11; i++)
            {
                string rowNum = $"3.1.{i}";
                var data = table1.Where(x => x.RowNum == rowNum);

                var complaint = new ZpzTreatment
                {
                    Row = rowNum,
                    Oral = Convert.ToInt32(data.Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(data.Sum(x => x.CountSmoAnother))
                };

                complaints.Add(complaint);
            }

            string rowNum_ = "3.1.16";
            var data_ = table1.Where(x => x.RowNum == rowNum_);
            var complaint_ = new ZpzTreatment
            {
                Row = rowNum_,
                Oral = Convert.ToInt32(data_.Sum(x => x.CountSmo)),
                Written = Convert.ToInt32(data_.Sum(x => x.CountSmoAnother))
            };
            complaints.Add(complaint_);
            
            return complaints;
        }

        private async Task<List<ZpzStatistics>> CollectProtections(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 3", region);
            return new List<ZpzStatistics>() {
                new ZpzStatistics {
                    Row = "1", Count = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmo))
                },
                new ZpzStatistics {
                    Row = "1.1", Count = Convert.ToInt32(table1.Where(x => x.RowNum == "1.1").Sum(x => x.CountSmo))
                },
                new ZpzStatistics {
                    Row = "2", Count = Convert.ToDecimal(table1.Where(x => x.RowNum == "2").Sum(x => x.CountSmo))
                }
            };
        }

        private async Task<List<Expertise>> CollectExpertises(LinqToSqlKmsReportDataContext db, string region)
        {
            var meeTable = CollectZpzQ(db, "Таблица 6", region);
            var mee = new Expertise
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
            var ekmp = new Expertise
            {
                Row = "1",
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

            var expertises = new List<Expertise>() { mee, ekmp };

            return expertises;
        }

        private async Task<List<ZpzStatistics>> CollectSpecialists(LinqToSqlKmsReportDataContext db, string region)
        {
            var specialistsTable = CollectZpzQ(db, "Таблица 9", region);
            return new List<ZpzStatistics>() {
                new ZpzStatistics {
                    Row = "1",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics {
                    Row = "1.1.2",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.2")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics {
                    Row = "1.1.3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics {
                    Row = "3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new ZpzStatistics {
                    Row = "4",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "4")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                }
            };
        }

        private async Task<List<ZpzStatistics>> CollectInformations(LinqToSqlKmsReportDataContext db, string region)
        {
            var informTable = CollectZpz10(db, "Таблица 10", region);

            var informations = new List<ZpzStatistics>();

            var inform2 = new ZpzStatistics
            {
                Row = "2",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "2").Sum(x => x.CountSmo)),
            };
            informations.Add(inform2);

            for (int i = 1; i <= 6; i++)
            {
                string rowNum = $"2.{i}";
                var inform = new ZpzStatistics
                {
                    Row = rowNum,
                    Count = Convert.ToInt32(informTable.Where(x => x.RowNum == rowNum).Sum(x => x.CountSmo)),
                };
                informations.Add(inform);
            }

            var inform4 = new ZpzStatistics
            {
                Row = "4",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
            };
            informations.Add(inform4);

            for (int i = 1; i <= 8; i++)
            {
                string rowNum = $"4.{i}";
                var inform = new ZpzStatistics
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