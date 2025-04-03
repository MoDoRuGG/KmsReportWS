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
    public class FFOMSTargetedExpCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt(), ReportStatus.Saved.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public FFOMSTargetedExpCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSTargetedExp> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);

            IEnumerable<Task<FFOMSTargetedExp>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
        }

        private async Task<FFOMSTargetedExp> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var MEETask = CollectMEE(db, filial);
            var EKMPTask = CollectEKMP(db, filial);
            var MDTask = CollectMD_EKMP(db, filial);


            var T1MEE = await MEETask;
            var T2EKMP = await EKMPTask;
            var T3MD_EKMP = await MDTask;


            return new FFOMSTargetedExp
            {
                Filial = filial,
                MEE = T1MEE,
                EKMP = T2EKMP,
                MD_EKMP = T3MD_EKMP
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


        private async Task<List<MEE>> CollectMEE(LinqToSqlKmsReportDataContext db, string region)
        {
            var meeTable = CollectZpzQ(db, "Таблица 6", region);
            return new List<MEE>() {
                new MEE {
                    Row = "1.8", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.8").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.8").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.8").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.8").Sum(x => x.CountStac))
                },
                new MEE {
                    Row = "1.9", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountStac))
                },
                new MEE {
                    Row = "1.9amb", Target =
                                          Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountAmbulatory))
                },
                new MEE {
                    Row = "1.9stac", Target =
                                          Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountStac))
                },
                new MEE {
                    Row = "1.10", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountOutOfSmo)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountAmbulatory)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountDs)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountStac))
                },
                new MEE {
                    Row = "1.11", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountOutOfSmo)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountAmbulatory)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountDs)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountStac))
                },
                new MEE {
                    Row = "1.12", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountOutOfSmo)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountAmbulatory)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountDs)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountStac))
                },
                new MEE {
                    Row = "1.13", Target = Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.13").Sum(x => x.CountOutOfSmo)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.13").Sum(x => x.CountAmbulatory)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.13").Sum(x => x.CountDs)) +
                                           Convert.ToInt32(meeTable.Where(x => x.RowNum == "1.13").Sum(x => x.CountStac))
                },
            };
        }



        private async Task<List<EKMP>> CollectEKMP(LinqToSqlKmsReportDataContext db, string region)
        {
            var ekmpTable = CollectZpzQ(db, "Таблица 7", region);
            return new List<EKMP>() {
                new EKMP {
                    Row = "1.9", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.9").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.10", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.10.1", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.1").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.1").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.1").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.1").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.10.2", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.2").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.2").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.2").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.2").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.10.3", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.3").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.3").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.3").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.3").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.10.4", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.4").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.4").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.4").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.10.4").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.11", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.11").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "1.12", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "1.12").Sum(x => x.CountStac))
                },
                new EKMP {
                    Row = "2", Target = Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "2").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "2").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "2").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(ekmpTable.Where(x => x.RowNum == "2").Sum(x => x.CountStac))
                },

            };
        }


        private async Task<List<MD_EKMP>> CollectMD_EKMP(LinqToSqlKmsReportDataContext db, string region)
        {
            var md_ekmpTable = CollectZpzQ(db, "Таблица 7", region);
            return new List<MD_EKMP>() {
                new MD_EKMP {
                    Row = "3.9", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.9").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.9").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.9").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.9").Sum(x => x.CountStac))
                },
                new MD_EKMP {
                    Row = "3.10.1", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.1").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.1").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.1").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.1").Sum(x => x.CountStac))
                },
                                new MD_EKMP {
                    Row = "3.10.2", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2").Sum(x => x.CountStac))
                },
                                                new MD_EKMP {
                    Row = "3.10.2.1", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.1").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.1").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.1").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.1").Sum(x => x.CountStac))
                },
                                                                new MD_EKMP {
                    Row = "3.10.2.2", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.2").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.2").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.2").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.2.2").Sum(x => x.CountStac))
                },
                                                                new MD_EKMP {
                    Row = "3.10.3", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.3").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.3").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.3").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.3").Sum(x => x.CountStac))
                },
                                                                                new MD_EKMP {
                    Row = "3.10.4", Target = Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.4").Sum(x => x.CountOutOfSmo)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.4").Sum(x => x.CountAmbulatory)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.4").Sum(x => x.CountDs)) +
                                          Convert.ToInt32(md_ekmpTable.Where(x => x.RowNum == "3.10.4").Sum(x => x.CountStac))
                },
            };    
        }

    }
}