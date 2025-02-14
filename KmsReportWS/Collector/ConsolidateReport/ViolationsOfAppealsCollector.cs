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
    public class ViolationsOfAppealsCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string[] T1Rows = { "3.1", "3.1.1", "3.1.2", "3.1.3", "3.1.4", "3.1.5", "3.1.5.1", "3.1.5.2", "3.1.6", "3.1.7", "3.1.8", "3.1.9", "3.1.10", "3.1.11", "3.1.16", };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public ViolationsOfAppealsCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<ViolationsOfAppeals> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);

            IEnumerable<Task<ViolationsOfAppeals>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<ViolationsOfAppeals> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var treatmentTask = CollectTreatmentsVOA(db, filial);
            var complaintsTask = CollectComplaintsVOA(db, filial);
            var protectionsTask = CollectProtectionsVOA(db, filial);
            var expertiesesTask = CollectExpertisesVOA(db, filial);
            var specialistsTask = CollectSpecialistsVOA(db, filial);
            var informationsTask = CollectInformationsVOA(db, filial);

            var treatmentsVOA = await treatmentTask;
            var complaintsVOA = await complaintsTask;
            var protectionsVOA = await protectionsTask;
            var expertisesVOA = await expertiesesTask;
            var specialistsVOA = await specialistsTask;
            var informationsVOA = await informationsTask;

            return new ViolationsOfAppeals
            {
                Filial = filial,
                Treatments = treatmentsVOA,
                Complaints = complaintsVOA,
                Expertises = expertisesVOA,
                Protections = protectionsVOA,
                Specialists = specialistsVOA,
                Informations = informationsVOA,
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
            where Convert.ToInt32(flow.Yymm) <= Convert.ToInt32(_yymm) && Convert.ToInt32(flow.Yymm) >= Convert.ToInt32(_yymm.Substring(0, 2) + "01")
                  && data.Theme == theme
                  && flow.Id_Region == region
                  && Statuses.Contains(flow.Status)
                  && flow.Id_Report_Type == "Zpz10_2025"
            select f;


        private async Task<List<TreatmentVOA>> CollectTreatmentsVOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);
            return new List<TreatmentVOA> {
                new TreatmentVOA {
                    Row = "1",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "1").Sum(x => x.CountAssignment)),
                },
                new TreatmentVOA {
                    Row = "3",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "3").Sum(x => x.CountAssignment)),
                },
                new TreatmentVOA {
                    Row = "4",
                    Oral = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1
                        .Where(x => x.RowNum == "4").Sum(x => x.CountAssignment)),
                },
                new TreatmentVOA {
                    Row = "5",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "5").Sum(x => x.CountAssignment)),
                },
                new TreatmentVOA {
                    Row = "6",
                    Oral = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmo)),
                    Written = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountSmoAnother)),
                    Assignment = Convert.ToInt32(table1.Where(x => x.RowNum == "6").Sum(x => x.CountAssignment)),
                }

            };
        }

        private async Task<List<TreatmentVOA>> CollectComplaintsVOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);

            var complaints = new List<TreatmentVOA>();
            foreach (string rown in T1Rows)
            {
                var data = table1.Where(x => x.RowNum == rown);
                var complaint = new TreatmentVOA
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

        private async Task<List<StatisticsVOA>> CollectProtectionsVOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var table1 = CollectZpz(db, "Таблица 1", region);
            var table2 = CollectZpz(db, "Таблица 2", region);
            return new List<StatisticsVOA>() {
                new StatisticsVOA {
                    Row = "3.1", Count = Convert.ToInt32(table1.Where(x => x.RowNum == "3.1").Sum(x => x.CountSmoAnother))+Convert.ToInt32(table1.Where(x => x.RowNum == "3.1").Sum(x => x.CountSmo))
                },
                new StatisticsVOA {
                    Row = "1", Count = Convert.ToInt32(table2.Where(x => x.RowNum == "1").Sum(x => x.CountSmo))
                },
                new StatisticsVOA {
                    Row = "2", Count = Convert.ToDecimal(table2.Where(x => x.RowNum == "2").Sum(x => x.CountSmo))
                }
            };

        }

        private async Task<List<ExpertiseVOA>> CollectExpertisesVOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var meeTable = CollectZpzQ(db, "Таблица 6", region);
            var mee = new ExpertiseVOA
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
            var ekmp = new ExpertiseVOA
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

            var expertises = new List<ExpertiseVOA>() { mee, ekmp };

            return expertises;
        }

        private async Task<List<StatisticsVOA>> CollectSpecialistsVOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var specialistsTable = CollectZpzQ(db, "Таблица 9", region);
            return new List<StatisticsVOA>() {
                new StatisticsVOA {
                    Row = "1",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new StatisticsVOA {
                    Row = "1.1.2",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.2")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new StatisticsVOA {
                    Row = "1.1.3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "1.1.3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new StatisticsVOA {
                    Row = "3",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "3")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                },
                new StatisticsVOA {
                    Row = "4",
                    Count = Convert.ToInt32(specialistsTable.Where(x => x.RowNum == "4")
                        .Sum(x => x.CountSmo + x.CountSmoAnother)),
                }
            };
        }

        private async Task<List<StatisticsVOA>> CollectInformationsVOA(LinqToSqlKmsReportDataContext db, string region)
        {
            var informTable = CollectZpz10(db, "Таблица 10", region);

            var informations = new List<StatisticsVOA>();

            var inform2 = new StatisticsVOA
            {
                Row = "2",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "2").Sum(x => x.CountSmo)),
            };
            informations.Add(inform2);

            for (int i = 1; i <= 6; i++)
            {
                string rowNum = $"2.{i}";
                var inform = new StatisticsVOA
                {
                    Row = rowNum,
                    Count = Convert.ToInt32(informTable.Where(x => x.RowNum == rowNum).Sum(x => x.CountSmo)),
                };
                informations.Add(inform);
            }

            var inform4 = new StatisticsVOA
            {
                Row = "4",
                Count = Convert.ToInt32(
                informTable.Where(x => x.RowNum == "4").Sum(x => x.CountSmo)),
            };
            informations.Add(inform4);

            for (int i = 1; i <= 8; i++)
            {
                string rowNum = $"4.{i}";
                var inform = new StatisticsVOA
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