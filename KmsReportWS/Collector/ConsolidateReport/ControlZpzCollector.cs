using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ControlZpzCollector
    {
        private readonly string[] _themes = { "Таблица 5", "Таблица 6", "Таблица 8", "Таблица 10", "Таблица 11" };

        private readonly string[] _rowNumsExpertiseTable5 = { "4.1", "4.2", "4.3", "4.4", "4.5", "4.6" };
        private readonly string[] _rowNumsExpertiseTable6 = { "5.1", "5.2", "5.3", "5.4", "5.5", "5.6", "5.7", "5.8" };
        private readonly string[] _rowNumsExpertiseTable8 =
            {"6.1", "6.2", "6.3", "6.4", "6.5", "6.6", "6.7", "6.8", "6.9", "6.10"};

        public List<CReportPg> Collect(string yymm, bool isMonthly)
        {
            string reportType = isMonthly ? "PG" : "PG_Q";

            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            var pgData = CollectSummaryData(yymm, reportType);

            var reports = new List<CReportPg>();
            var filials = pgData.Select(x => x.Filial).Distinct().OrderBy(x => x);
            foreach (var filial in filials)
            {
                var pgFilialData = pgData.Where(x => x.Filial == filial);
                var expertise = MapExpertise(pgFilialData);
                var finance = MapFinance(pgFilialData);
                var personnel = MapPersonnel(pgFilialData);
                var normative = MapNormative(pgFilialData);

                var report = new CReportPg
                {
                    Filial = filial,
                    Expertise = expertise,
                    Normative = normative,
                    Finance = finance,
                    Personnel = personnel
                };
                reports.Add(report);
            }

            return reports;
        }

        private PgNormative MapNormative(IEnumerable<SummaryPg> pgFilialData)
        {
            var table5Data = pgFilialData.Where(x => x.Theme == "Таблица 5");
            var table6Data = pgFilialData.Where(x => x.Theme == "Таблица 6");
            var table8Data = pgFilialData.Where(x => x.Theme == "Таблица 8");
            return new PgNormative
            {
                BillsOutMo = table5Data.Where(x => x.RowNum == "5").Sum(x => x.SumOutOfSmo),
                MeeOutMoPlan = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumOutOfSmo),
                MeeOutMoTarget = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumOutOfSmoAnother),
                BillsApp = table5Data.Where(x => x.RowNum == "5").Sum(x => x.SumAmbulatory),
                MeeAppPlan = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumAmbulatory),
                MeeAppTarget = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumAmbulatoryAnother),
                BillsDayHosp = table5Data.Where(x => x.RowNum == "5").Sum(x => x.SumDs),
                MeeDayHospPlan = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumDs),
                MeeDayHospTarget = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumDsAnother),
                BillsHosp = table5Data.Where(x => x.RowNum == "5").Sum(x => x.SumStac),
                MeeHospPlan = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumStac),
                MeeHospTarget = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumStacAnother),
                EkmpOutMoPlan = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumOutOfSmo),
                EkmpOutMoTarget = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumOutOfSmoAnother),
                EkmpAppPlan = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumAmbulatory),
                EkmpAppTarget = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumAmbulatoryAnother),
                EkmpDayHospPlan = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumDs),
                EkmpDayHospTarget = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumDsAnother),
                EkmpHospPlan = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumStac),
                EkmpHospTarget = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumStacAnother),
            };
        }

        private PgPersonnel MapPersonnel(IEnumerable<SummaryPg> pgFilialData)
        {
            var pgTable11 = pgFilialData.Where(x => x.Theme == "Таблица 11");
            return new PgPersonnel
            {
                Specialist = pgTable11.Where(x => x.RowNum == "1").Sum(x => x.SumSmo + x.SumSmoAnother),
                MekFullTime = pgTable11.Where(x => x.RowNum == "1.1.1").Sum(x => x.SumSmo),
                MekRemote = pgTable11.Where(x => x.RowNum == "1.1.1").Sum(x => x.SumSmoAnother),
                ExpertsFullTime = pgTable11.Where(x => x.RowNum == "1.1.2").Sum(x => x.SumSmo),
                ExpertsRemote = pgTable11.Where(x => x.RowNum == "1.1.2").Sum(x => x.SumSmoAnother),
                ExpertsEkmpRegion = pgTable11.Where(x => x.RowNum == "1.1.3.1").Sum(x => x.SumSmo),
                ExpertsEkmpRemote = pgTable11.Where(x => x.RowNum == "1.1.3.1").Sum(x => x.SumSmoAnother),
                ExpertsEkmpRegionOnko = pgTable11.Where(x => x.RowNum == "1.1.3.1.1").Sum(x => x.SumSmo),
                ExpertsEkmpRemoteOnko = pgTable11.Where(x => x.RowNum == "1.1.3.1.1").Sum(x => x.SumSmoAnother),
                ExpertsEkmpRegister = pgTable11.Where(x => x.RowNum == "1.1.3.2").Sum(x => x.SumSmo),
                ExpertsEkmpRegisterRemote = pgTable11.Where(x => x.RowNum == "1.1.3.2").Sum(x => x.SumSmoAnother),
                ExpertsEkmpRegisterOnko = pgTable11.Where(x => x.RowNum == "1.1.3.2.1").Sum(x => x.SumSmo),
                ExpertsEkmpRegisterRemoteOnko = pgTable11.Where(x => x.RowNum == "1.1.3.2.1").Sum(x => x.SumSmoAnother),
                ExpertsOmsFullTime = pgTable11.Where(x => x.RowNum == "2").Sum(x => x.SumSmo),
                ExpertsOmsRemote = pgTable11.Where(x => x.RowNum == "2").Sum(x => x.SumSmoAnother),
                ExpertsOmsEkmpFullTime = pgTable11.Where(x => x.RowNum == "2.1").Sum(x => x.SumSmo),
                ExpertsOmsEkmpRemote = pgTable11.Where(x => x.RowNum == "2.1").Sum(x => x.SumSmoAnother)
            };
        }

        private PgFinance MapFinance(IEnumerable<SummaryPg> pgFilialData)
        {
            var pgTable10 = pgFilialData.Where(x => x.Theme == "Таблица 10");
            return new PgFinance
            {
                SumPayment = pgTable10.Where(x => x.RowNum == "1").Sum(x => x.SumSmo),
                SumNotPayment = pgTable10.Where(x => x.RowNum == "2").Sum(x => x.SumSmo),
                SumMek = pgTable10.Where(x => x.RowNum == "3").Sum(x => x.SumSmo),
                SumMee = pgTable10.Where(x => x.RowNum == "4").Sum(x => x.SumSmo),
                SumEkmp = pgTable10.Where(x => x.RowNum == "5").Sum(x => x.SumSmo)
            };
        }

        private PgExpertise MapExpertise(IEnumerable<SummaryPg> pgFilialData)
        {
            var table5Data = pgFilialData.Where(x => x.Theme == "Таблица 5");
            var table6Data = pgFilialData.Where(x => x.Theme == "Таблица 6");
            var table8Data = pgFilialData.Where(x => x.Theme == "Таблица 8");
            return new PgExpertise
            {
                Bills = table5Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom),
                BillsOnco = table5Data.Where(x => x.RowNum == "1.1").Sum(x => x.SumVidpom),
                BillsVioletion = table5Data.Where(x => _rowNumsExpertiseTable5.Contains(x.RowNum)).Sum(x => x.SumVidpom),
                PaymentBills = table5Data.Where(x => x.RowNum == "5").Sum(x => x.SumVidpom),
                PaymentBillsOnco = table5Data.Where(x => x.RowNum == "5.1").Sum(x => x.SumVidpom),
                MeeTarget = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom),
                MeePlan = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpomAnother),
                CaseMeeTarget = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumVidpom),
                CaseMeePlan = table6Data.Where(x => x.RowNum == "2").Sum(x => x.SumVidpomAnother),
                DefectMeeTarget = table6Data.Where(x => _rowNumsExpertiseTable6.Contains(x.RowNum)).Sum(x => x.SumVidpom),
                DefectMeePlan = table6Data.Where(x => _rowNumsExpertiseTable6.Contains(x.RowNum)).Sum(x => x.SumVidpomAnother),
                EkmpTarget = table8Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom),
                EkmpPlan = table8Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpomAnother),
                ThemeCaseEkmpPlan = table8Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpomAnother),
                CaseEkmpTarget = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumVidpom),
                CaseEkmpPlan = table8Data.Where(x => x.RowNum == "2").Sum(x => x.SumVidpomAnother),
                DefectEkmpTarget = table8Data.Where(x => _rowNumsExpertiseTable8.Contains(x.RowNum)).Sum(x => x.SumVidpom),
                DefectEkmpPlan = table8Data.Where(x => _rowNumsExpertiseTable8.Contains(x.RowNum)).Sum(x => x.SumVidpomAnother)
            };
        }

        private List<SummaryPg> CollectSummaryData(string yymm, string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Pg on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                          //&& flow.Status != ReportStatus.Refuse.GetDescriptionSt()
                          && flow.Id_Report_Type == reportType
                          //&& _themes.Contains(rData.Theme)
                    group new { flow, rData, table } by new { flow.Id_Region, rData.Theme, table.RowNum }
                          into gr
                    select new SummaryPg
                    {
                        Filial = gr.Key.Id_Region,
                        Theme = gr.Key.Theme,
                        RowNum = gr.Key.RowNum,
                        SumSmo = gr.Sum(x => x.table.CountSmo) ?? 0,
                        SumSmoAnother = gr.Sum(x => x.table.CountSmoAnother) ?? 0,
                        SumOutOfSmo = gr.Sum(x => x.table.CountOutOfSmo) ?? 0,
                        SumAmbulatory = gr.Sum(x => x.table.CountAmbulatory) ?? 0,
                        SumDs = gr.Sum(x => x.table.CountDs) ?? 0,
                        SumStac = gr.Sum(x => x.table.CountStac),
                        SumOutOfSmoAnother = gr.Sum(x => x.table.CountOutOfSmoAnother) ?? 0,
                        SumAmbulatoryAnother = gr.Sum(x => x.table.CountAmbulatoryAnother) ?? 0,
                        SumDsAnother = gr.Sum(x => x.table.CountDsAnother) ?? 0,
                        SumStacAnother = gr.Sum(x => x.table.CountStacAnother) ?? 0,
                    }).ToList();
        }
    }
}
