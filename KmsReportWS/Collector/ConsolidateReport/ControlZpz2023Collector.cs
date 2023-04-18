using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ControlZpz2023Collector
    {
        private readonly string[] _themes = { "Таблица 5А", "Таблица 6", "Таблица 7", "Таблица 8", "Таблица 9" };

        private readonly string[] _rowNumsExpertiseTable5 = { "4.1", "4.2", "4.3", "4.4", "4.5", "4.6" };
        private readonly string[] _rowNumsExpertiseTable6 =
                { "4" };
                //{ "5.1", "5.2", "5.3", "5.4", "5.5", "5.6", "5.7", "5.8" };

        private readonly string[] _rowNumsExpertiseTable7 =
                //{"6.1", "6.2", "6.3", "6.4", "6.5", "6.6", "6.7", "6.8", "6.9", "6.10"};
                {"6.8", "6.9", "6.9.12", "6.10", "6.11", "6.12", "6.13", "6.14", "6.15", "6.16", "6.17", "6.18", "6.19", "6.20", "6.22", "6.23.8", "6.23.10"};

        public List<CReportZpz2023> Collect(string yymm, bool isMonthly)
        {
            string reportType = isMonthly ? "Zpz" : "Zpz_Q";

            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            var zpzData = CollectSummaryData(yymm, reportType);

            var reports = new List<CReportZpz2023>();
            var filials = zpzData.Select(x => x.Filial).Distinct().OrderBy(x => x);
            foreach (var filial in filials)
            {
                var zpzFilialData = zpzData.Where(x => x.Filial == filial);
                var expertise = MapExpertise(zpzFilialData);
                var finance = MapFinance(zpzFilialData);
                var personnel = MapPersonnel(zpzFilialData);
                var normative = MapNormative(zpzFilialData);

                var report = new CReportZpz2023
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

        private ZpzNormative2023 MapNormative(IEnumerable<SummaryZpz2023> zpzFilialData)
        {
            var table5Data = zpzFilialData.Where(x => x.Theme == "Таблица 5А");
            var table6Data = zpzFilialData.Where(x => x.Theme == "Таблица 6");
            var table7Data = zpzFilialData.Where(x => x.Theme == "Таблица 7");
            return new ZpzNormative2023
            {
                BillsOutMo = table5Data.Sum(x => x.SumSmoAnother),
                MeeOutMoTarget = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumOutOfSmoAnother),
                MeeOutMoPlan = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumOutOfSmo),
                BillsApp = table5Data.Sum(x => x.SumInsured),
                MeeAppTarget = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumAmbulatoryAnother),
                MeeAppPlan = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumAmbulatory),
                BillsDayHosp = table5Data.Sum(x => x.SumInsuredRepresentative),
                MeeDayHospTarget = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumDsAnother),
                MeeDayHospTargetVmp = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumDsVmpAnother),
                MeeDayHospPlan = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumDs),
                MeeDayHospPlanVmp = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumDsVmp),
                BillsHosp = table5Data.Sum(x => x.SumProsecutor),
                MeeHospTarget = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumStacAnother),
                MeeHospTargetVmp = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumStacVmpAnother),
                MeeHospPlan = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumStac),
                MeeHospPlanVmp = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumStacVmp),
                EkmpOutMoTarget = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumOutOfSmoAnother),
                EkmpOutMoPlan = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumOutOfSmo),
                EkmpAppTarget = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumAmbulatoryAnother),
                EkmpAppPlan = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumAmbulatory),
                EkmpDayHospTarget = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumDsAnother),
                EkmpDayHospTargetVmp = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumDsVmpAnother),
                EkmpDayHospPlan = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumDs),
                EkmpDayHospPlanVmp = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumDsVmp),
                EkmpHospTarget = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumStacAnother),
                EkmpHospTargetVmp = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumStacVmpAnother),
                EkmpHospPlan = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumStac),
                EkmpHospPlanVmp = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumStacVmp), 
            };
        }

        private ZpzPersonnel2023 MapPersonnel(IEnumerable<SummaryZpz2023> zpzFilialData)
        {
            var zpzTable9 = zpzFilialData.Where(x => x.Theme == "Таблица 9");
            return new ZpzPersonnel2023
            {
                SpecialistFullTime = zpzTable9.Where(x => x.RowNum == "1").Sum(x => x.SumSmo),
                SpecialistRemote = zpzTable9.Where(x => x.RowNum == "1").Sum(x => x.SumSmoAnother),
                FullTime = zpzTable9.Where(x => x.RowNum == "1.1").Sum(x => x.SumSmo),
                Remote = zpzTable9.Where(x => x.RowNum == "1.1").Sum(x => x.SumSmoAnother),
                ExpertsFullTime = zpzTable9.Where(x => x.RowNum == "1.1.2").Sum(x => x.SumSmo),
                ExpertsRemote = zpzTable9.Where(x => x.RowNum == "1.1.2").Sum(x => x.SumSmoAnother),
                ExpertsEkmpRegion = zpzTable9.Where(x => x.RowNum == "1.1.3").Sum(x => x.SumSmo),
                ExpertsEkmpRemote = zpzTable9.Where(x => x.RowNum == "1.1.3").Sum(x => x.SumSmoAnother),
                ExpertisesFullTime = zpzTable9.Where(x => x.RowNum == "1.1.3.1").Sum(x => x.SumSmo),
                ExpertisesRemote = zpzTable9.Where(x => x.RowNum == "1.1.3.1").Sum(x => x.SumSmoAnother),
                ExpertisesPlanFullTime = zpzTable9.Where(x => x.RowNum == "1.1.3.1.1").Sum(x => x.SumSmo),
                ExpertisesPlanRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.1").Sum(x => x.SumSmoAnother),
                ExpertisesPlanAppealFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.1.1").Sum(x => x.SumSmo),
                ExpertisesPlanAppealRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.1.1").Sum(x => x.SumSmoAnother),
                ExpertisesPlanUnfoundedFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.1.2").Sum(x => x.SumSmo),
                ExpertisesPlanUnfoundedRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.1.2").Sum(x => x.SumSmoAnother),
                ExpertisesUnplannedFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.2").Sum(x => x.SumSmo),
                ExpertisesUnplannedRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.2").Sum(x => x.SumSmoAnother),
                ExpertisesUnplannedAppealFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.2.1").Sum(x => x.SumSmo),
                ExpertisesUnplannedAppealRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.2.1").Sum(x => x.SumSmoAnother),
                ExpertisesUnplannedUnfoundedFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.2.2").Sum(x => x.SumSmo),
                ExpertisesUnplannedUnfoundedRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.2.2").Sum(x => x.SumSmoAnother),
                ExpertisesThemeFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.3").Sum(x => x.SumSmo),
                ExpertisesThemeRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.3").Sum(x => x.SumSmoAnother),
                ExpertisesThemeAppealFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.3.1").Sum(x => x.SumSmo),
                ExpertisesThemeAppealRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.3.1").Sum(x => x.SumSmoAnother),
                ExpertisesThemeUnfoundedFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.3.2").Sum(x => x.SumSmo),
                ExpertisesThemeUnfoundedRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.3.2").Sum(x => x.SumSmoAnother),
                ExpertisesMultiFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.4").Sum(x => x.SumSmo),
                ExpertisesMultiRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.4").Sum(x => x.SumSmoAnother),
                ExpertisesMultiAppealFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.4.1").Sum(x => x.SumSmo),
                ExpertisesMultiAppealRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.4.1").Sum(x => x.SumSmoAnother),
                ExpertisesMultiUnfoundedFullTime = zpzTable9.Where(y => y.RowNum == "1.1.3.1.4.2").Sum(x => x.SumSmo),
                ExpertisesMultiUnfoundedRemote = zpzTable9.Where(y => y.RowNum == "1.1.3.1.4.2").Sum(x => x.SumSmoAnother),
                PreparedFullTime = zpzTable9.Where(y => y.RowNum == "2").Sum(x => x.SumSmo),
                PreparedRemote = zpzTable9.Where(y => y.RowNum == "2").Sum(x => x.SumSmoAnother),
                QualFullTime = zpzTable9.Where(y => y.RowNum == "3").Sum(x => x.SumSmo),
                QualRemote = zpzTable9.Where(y => y.RowNum == "3").Sum(x => x.SumSmoAnother),
                QualHigherFullTime = zpzTable9.Where(y => y.RowNum == "3.1").Sum(x => x.SumSmo),
                QualHigherRemote = zpzTable9.Where(y => y.RowNum =="3.1").Sum(x => x.SumSmoAnother),
                Qual1stFullTime = zpzTable9.Where(y => y.RowNum == "3.2").Sum(x => x.SumSmo),
                Qual1stRemote = zpzTable9.Where(y => y.RowNum == "3.2").Sum(x => x.SumSmoAnother),
                Qual2ndFullTime = zpzTable9.Where(y => y.RowNum == "3.3").Sum(x => x.SumSmo),
                Qual2ndRemote = zpzTable9.Where(y => y.RowNum == "3.3").Sum(x => x.SumSmoAnother),
                DegreeFullTime = zpzTable9.Where(y => y.RowNum == "4").Sum(x => x.SumSmo),
                DegreeRemote = zpzTable9.Where(y => y.RowNum == "4").Sum(x => x.SumSmoAnother),
                CandidateFullTime = zpzTable9.Where(y => y.RowNum == "4.1").Sum(x => x.SumSmo),
                CandidateRemote = zpzTable9.Where(y => y.RowNum == "4.1").Sum(x => x.SumSmoAnother),
                DoctorFullTime = zpzTable9.Where(y => y.RowNum == "4.2").Sum(x => x.SumSmo),
                DoctorRemote = zpzTable9.Where(y => y.RowNum == "4.2").Sum(x => x.SumSmoAnother),
                InsRepresFullTime = zpzTable9.Where(y => y.RowNum == "5").Sum(x => x.SumSmo),
                InsRepres1FullTime = zpzTable9.Where(y => y.RowNum == "5.1").Sum(x => x.SumSmo),
                InsRepres1spFullTime = zpzTable9.Where(y => y.RowNum == "5.1.1").Sum(x => x.SumSmo),
                InsRepres2FullTime = zpzTable9.Where(y => y.RowNum == "5.2").Sum(x => x.SumSmo),
                InsRepres2spFullTime = zpzTable9.Where(y => y.RowNum == "5.2.1").Sum(x => x.SumSmo),
                InsRepres3FullTime = zpzTable9.Where(y => y.RowNum == "5.3").Sum(x => x.SumSmo),
                InsRepres3spFullTime = zpzTable9.Where(y => y.RowNum == "5.3.1").Sum(x => x.SumSmo),
            };
        }

        private ZpzFinance2023 MapFinance(IEnumerable<SummaryZpz2023> zpzFilialData)
        {
            var zpzTable8 = zpzFilialData.Where(x => x.Theme == "Таблица 8");
            return new ZpzFinance2023
            {
                SumPayment = zpzTable8.Where(x => x.RowNum == "1").Sum(x => x.SumSmo),
                SumNotPayment = zpzTable8.Where(x => x.RowNum == "2").Sum(x => x.SumSmo),
                SumMek = zpzTable8.Where(x => x.RowNum == "3").Sum(x => x.SumSmo),
                SumMee = zpzTable8.Where(x => x.RowNum == "4").Sum(x => x.SumSmo),
                SumEkmp = zpzTable8.Where(x => x.RowNum == "5").Sum(x => x.SumSmo)

            };

        }



        private ZpzExpertise2023 MapExpertise(IEnumerable<SummaryZpz2023> zpzFilialData)
        {
            var table5Data = zpzFilialData.Where(x => x.Theme == "Таблица 5А");
            var table6Data = zpzFilialData.Where(x => x.Theme == "Таблица 6");
            var table7Data = zpzFilialData.Where(x => x.Theme == "Таблица 7");
            return new ZpzExpertise2023
            {
                Bills = table5Data.Sum(x => x.SumSmo),
                CountMee = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeComplaint = table6Data.Where(x => x.RowNum == "1.8").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeRepeat = table6Data.Where(x => x.RowNum == "1.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeOnco = table6Data.Where(x => x.RowNum == "1.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDs = table6Data.Where(x => x.RowNum == "1.11").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeLeth = table6Data.Where(x => x.RowNum == "1.12").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeInjured = table6Data.Where(x => x.RowNum == "1.13").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectedCase = table6Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefects = table6Data.Where(x => x.RowNum == "4").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsPeriod = table6Data.Where(x => x.RowNum == "4.8").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsCondition = table6Data.Where(x => x.RowNum == "4.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsRepeat = table6Data.Where(x => x.RowNum == "4.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsOutOfDocums = table6Data.Where(x => x.RowNum == "4.11").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsUnpayable = table6Data.Where(x => x.RowNum == "4.12").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsBuyMedicament = table6Data.Where(x => x.RowNum == "4.13").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsOutOfLeth = table6Data.Where(x => x.RowNum == "4.14").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsWithoutDocums = table6Data.Where(x => x.RowNum == "4.15").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsIncorrectDocums = table6Data.Where(x => x.RowNum == "4.16").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsBadDocums = table6Data.Where(x => x.RowNum == "4.17").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsBadDate = table6Data.Where(x => x.RowNum == "4.18").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsBadData = table6Data.Where(x => x.RowNum == "4.19").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectsOutOfProtocol = table6Data.Where(x => x.RowNum == "4.20").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmp = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpComplaint = table7Data.Where(x => x.RowNum == "1.8").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpLeth = table7Data.Where(x => x.RowNum == "1.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpByMek = table7Data.Where(x => x.RowNum == "1.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpByMee = table7Data.Where(x => x.RowNum == "1.11").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpUTheme = table7Data.Where(x => x.RowNum == "2").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpMulti = table7Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpMultiLeth = table7Data.Where(x => x.RowNum == "3.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpMultiUtheme = table7Data.Where(x => x.RowNum == "3.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseDefectedBySmo = table7Data.Where(x => x.RowNum == "5").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpDefectedCase = table7Data.Where(x => x.RowNum == "6").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadDs = table7Data.Where(x => x.RowNum == "6.8").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadDsNotAffected = table7Data.Where(x => x.RowNum == "6.8.5").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadDsProlonger = table7Data.Where(x => x.RowNum == "6.8.6").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadDsDecline = table7Data.Where(x => x.RowNum == "6.8.7").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadDsInjured = table7Data.Where(x => x.RowNum == "6.8.8").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadDsLeth= table7Data.Where(x => x.RowNum == "6.8.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpBadMed = table7Data.Where(x => x.RowNum == "6.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpUnreglamentedMed = table7Data.Where(x => x.RowNum == "6.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpStopMed = table7Data.Where(x => x.RowNum == "6.11").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpContinuity = table7Data.Where(x => x.RowNum == "6.12").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpUnprofile = table7Data.Where(x => x.RowNum == "6.13").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpUnfounded = table7Data.Where(x => x.RowNum == "6.14").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpRepeat = table7Data.Where(x => x.RowNum == "6.15").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpDifference = table7Data.Where(x => x.RowNum == "6.16").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpUnfoundedMedicaments = table7Data.Where(x => x.RowNum == "6.17").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpUnfoundedReject = table7Data.Where(x => x.RowNum == "6.18").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpDisp = table7Data.Where(x => x.RowNum == "6.19").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpRepeat2weeks = table7Data.Where(x => x.RowNum == "6.20").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpOutOfResults = table7Data.Where(x => x.RowNum == "6.21").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountEkmpDoubleHospital = table7Data.Where(x => x.RowNum == "6.22").Sum(x => x.SumVidpom+x.SumVidpomAnother),

            };
        }

        private List<SummaryZpz2023> CollectSummaryData(string yymm, string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Zpz on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                          //&& flow.Status != ReportStatus.Refuse.GetDescriptionSt()
                          && flow.Id_Report_Type == reportType
                    //&& _themes.Contains(rData.Theme)
                    group new { flow, rData, table } by new { flow.Id_Region, rData.Theme, table.RowNum }
                          into gr
                    select new SummaryZpz2023
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
                    }).ToList();
        }
    }
}
