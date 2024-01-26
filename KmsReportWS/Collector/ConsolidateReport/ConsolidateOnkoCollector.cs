using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateOnkoCollector
    {
        private readonly string[] _themes =
            { "Таблица 1", "Таблица 3", "Таблица 5", "Таблица 6", "Таблица 8", "Таблица 10" };

        public List<ConsolidateOnko> Collect(string yymm, bool isMonthly)
        {
            string reportType = isMonthly ? "PG" : "PG_Q";

            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            var pgData = CollectSummaryData(yymm, reportType);

            var reports = new List<ConsolidateOnko>();
            var filials = pgData.Select(x => x.Filial).Distinct().OrderBy(x => x);
            foreach (var filial in filials)
            {
                var pgFilialData = pgData.Where(x => x.Filial == filial);
                var complaint = MapComplaint(pgFilialData);
                var protection = MapProtection(pgFilialData);
                var mek = MapMek(pgFilialData);
                var mee = MapMee(pgFilialData);
                var ekmp = MapEkmp(pgFilialData);
                var finance = MapFinance(pgFilialData);

                var report = new ConsolidateOnko
                {
                    Filial = filial,
                    Complaint = complaint,
                    Protection = protection,
                    Mek = mek,
                    Mee = mee,
                    Ekmp = ekmp,
                    Finance = finance
                };
                reports.Add(report);
            }

            return reports;
        }

        private OnkoComplaint MapComplaint(IEnumerable<SummaryPg> pgFilialData)
        {
            var table1Data = pgFilialData.Where(x => x.Theme == "Таблица 1");
            return new OnkoComplaint
            {
                MedicalHelp = table1Data.Where(x => x.RowNum == "3.6.2").Sum(x => x.SumSmo + x.SumSmoAnother),
                Dedlines = table1Data.Where(x => x.RowNum == "3.6.2.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                MedicineProvision = table1Data.Where(x => x.RowNum == "3.8.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                DedlineDrugMedicine = table1Data.Where(x => x.RowNum == "3.8.1.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                NoDrugMedicine = table1Data.Where(x => x.RowNum == "3.8.1.2").Sum(x => x.SumSmo + x.SumSmoAnother),
                AppealMedicalHelp = table1Data.Where(x => x.RowNum == "4.6.3").Sum(x => x.SumSmo + x.SumSmoAnother),
                AppealMedicineProvision = table1Data.Where(x => x.RowNum == "4.8.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                AppealDrugsMedicine = table1Data.Where(x => x.RowNum == "4.8.1.1").Sum(x => x.SumSmo + x.SumSmoAnother),
            };

        }

        private OnkoProtection MapProtection(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 3");
            return new OnkoProtection
            {
                PretrialMedicalHelp = table3Data.Where(x => x.RowNum == "1.6.2").Sum(x => x.SumSmo),
                PretrialDeadline = table3Data.Where(x => x.RowNum == "1.6.2.1").Sum(x => x.SumSmo),
                PretrialMedicineProvision = table3Data.Where(x => x.RowNum == "1.8.1").Sum(x => x.SumSmo),
                PretrialDedlineDrugMedicine = table3Data.Where(x => x.RowNum == "1.8.1.1").Sum(x => x.SumSmo),
                PretrialNoDrugMedicine = table3Data.Where(x => x.RowNum == "1.8.1.2").Sum(x => x.SumSmo),
                JudicalMedicalHelp = table3Data.Where(x => x.RowNum == "1.6.2").Sum(x => x.SumConflict),
                JudicalDeadline = table3Data.Where(x => x.RowNum == "1.6.2.1").Sum(x => x.SumConflict),
                JudicalMedicineProvision = table3Data.Where(x => x.RowNum == "1.8.1").Sum(x => x.SumConflict),
                JudicalDedlineDrugMedicine = table3Data.Where(x => x.RowNum == "1.8.1.1").Sum(x => x.SumConflict),
                JudicalNoDrugMedicine = table3Data.Where(x => x.RowNum == "1.8.1.2").Sum(x => x.SumConflict),
            };
        }

        private OnkoMek MapMek(IEnumerable<SummaryPg> pgFilialData)
        {
            var table5Data = pgFilialData.Where(x => x.Theme == "Таблица 5");
            return new OnkoMek
            {
                PresentedBills = table5Data.Where(x => x.RowNum == "1.1").Sum(x => x.SumVidpom),
                AcceptedBills = table5Data.Where(x => x.RowNum == "5.1").Sum(x => x.SumVidpom),
                RegistrationMek = table5Data.Where(x => x.RowNum == "4.1.1").Sum(x => x.SumVidpom),
                NotInProgramMek = table5Data.Where(x => x.RowNum == "4.2.1").Sum(x => x.SumVidpom),
                TarifMek = table5Data.Where(x => x.RowNum == "4.3.1").Sum(x => x.SumVidpom),
                LicenceMek = table5Data.Where(x => x.RowNum == "4.4.1").Sum(x => x.SumVidpom),
                RepeatMek = table5Data.Where(x => x.RowNum == "4.5.1").Sum(x => x.SumVidpom),
            };
        }

        private OnkoMee MapMee(IEnumerable<SummaryPg> pgFilialData)
        {
            var table6Data = pgFilialData.Where(x => x.Theme == "Таблица 6");
            return new OnkoMee
            {
                Complaint = table6Data.Where(x => x.RowNum == "2.2.1").Sum(x => x.SumVidpom),
                Antitumor = table6Data.Where(x => x.RowNum == "2.3").Sum(x => x.SumVidpom),
                PlanHosp = table6Data.Where(x => x.RowNum == "2.5.1").Sum(x => x.SumVidpom),
                ViolationCondition = table6Data.Where(x => x.RowNum == "5.3.1.2").Sum(x => x.SumVidpom + x.SumVidpomAnother),
                ViolationOnkoFirst = table6Data.Where(x => x.RowNum == "5.3.2").Sum(x => x.SumVidpom + x.SumVidpomAnother),
                ViolationHisto = table6Data.Where(x => x.RowNum == "5.3.3").Sum(x => x.SumVidpom + x.SumVidpomAnother),
                ViolationOnkoDiagnostic = table6Data.Where(x => x.RowNum == "5.3.4").Sum(x => x.SumVidpom + x.SumVidpomAnother),
            };
        }

        private OnkoEkmp MapEkmp(IEnumerable<SummaryPg> pgFilialData)
        {
            var table8Data = pgFilialData.Where(x => x.Theme == "Таблица 8");
            return new OnkoEkmp
            {
                EkmpComplaint = table8Data.Where(x => x.RowNum == "2.2.1").Sum(x => x.SumVidpom),
                EkmpFromMee = table8Data.Where(x => x.RowNum == "2.3").Sum(x => x.SumVidpom),
                DeathEkmp = table8Data.Where(x => x.RowNum == "2.4.3").Sum(x => x.SumVidpom),
                ThematicEkmp = table8Data.Where(x => x.RowNum == "3.1").Sum(x => x.SumVidpomAnother),
                CountOnko = table8Data.Where(x => x.RowNum == "6.1.1.2").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                NoProfilOnko = table8Data.Where(x => x.RowNum == "6.2.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                UnreasonEkmp = table8Data.Where(x => x.RowNum == "6.3.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                DispEkmp = table8Data.Where(x => x.RowNum == "6.4.2").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                RecommendationEkmp = table8Data.Where(x => x.RowNum == "6.5.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                Premature = table8Data.Where(x => x.RowNum == "6.6.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                ViolationMoEmkp = table8Data.Where(x => x.RowNum == "6.7.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                Failure = table8Data.Where(x => x.RowNum == "6.8.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                Payment = table8Data.Where(x => x.RowNum == "6.9.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
                OtherViolation = table8Data.Where(x => x.RowNum == "6.10.1").Sum(x => x.SumOutOfSmo + x.SumVidpomAnother),
            };
        }

        private OnkoFinance MapFinance(IEnumerable<SummaryPg> pgFilialData)
        {
            var table10Data = pgFilialData.Where(x => x.Theme == "Таблица 10");
            return new OnkoFinance
            {
                SumMek = table10Data.Where(x => x.RowNum == "3.1").Sum(x => x.SumSmo),
                SumDispMee = table10Data.Where(x => x.RowNum == "4.1.2").Sum(x => x.SumSmo),
                SumMee = table10Data.Where(x => x.RowNum == "4.2").Sum(x => x.SumSmo),
                SumDispEkmp = table10Data.Where(x => x.RowNum == "5.1.2").Sum(x => x.SumSmo),
                SumNoProfilEkmp = table10Data.Where(x => x.RowNum == "5.2.1").Sum(x => x.SumSmo),
                SumUnreasonEkmp = table10Data.Where(x => x.RowNum == "5.3.1").Sum(x => x.SumSmo),
                SumRecommendationEkmp = table10Data.Where(x => x.RowNum == "5.4.1").Sum(x => x.SumSmo),
                SumPrematureEkmp = table10Data.Where(x => x.RowNum == "5.5.1").Sum(x => x.SumSmo),
                SumFailureEkmp = table10Data.Where(x => x.RowNum == "5.6.1").Sum(x => x.SumSmo),
                SumPaymentEkmp = table10Data.Where(x => x.RowNum == "5.7.1").Sum(x => x.SumSmo),
                SumOtherEkmp = table10Data.Where(x => x.RowNum == "5.8.1").Sum(x => x.SumSmo),
            };
        }

        private List<SummaryPg> CollectSummaryData(string yymm, string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Pg on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                          && flow.Status != ReportStatus.Refuse.GetDescriptionSt()
                          && flow.Id_Report_Type == reportType
                          && _themes.Contains(rData.Theme)
                          && flow.Id_Region != "RU-KHA"
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
                        SumStac = gr.Sum(x => x.table.CountStac) ?? 0,
                        SumOutOfSmoAnother = gr.Sum(x => x.table.CountOutOfSmoAnother) ?? 0,
                        SumAmbulatoryAnother = gr.Sum(x => x.table.CountAmbulatoryAnother) ?? 0,
                        SumDsAnother = gr.Sum(x => x.table.CountDsAnother) ?? 0,
                        SumStacAnother = gr.Sum(x => x.table.CountStacAnother) ?? 0,
                        SumInsured = gr.Sum(x => x.table.CountInsured) ?? 0,
                        SumInsuredRepresentative = gr.Sum(x => x.table.CountInsuredRepresentative) ?? 0,
                        SumProsecutor = gr.Sum(x => x.table.CountProsecutor) ?? 0,
                        SumTfoms = gr.Sum(x => x.table.CountTfoms) ?? 0,
                    }).ToList();
        }

    }
}