using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ControlZpz2023FullCollector
    {
        private readonly string[] _themes = { "Таблица 5А", "Таблица 6", "Таблица 7", "Таблица 8", "Таблица 9" };

        private readonly string[] _rowNumsExpertiseTable5 = { "4.1", "4.2", "4.3", "4.4", "4.5", "4.6" };
        private readonly string[] _rowNumsExpertiseTable6 =
                { "4" };

        private readonly string[] _rowNumsExpertiseTable7 =
                {"6.8", "6.9", "6.9.12", "6.10", "6.11", "6.12", "6.13", "6.14", "6.15", "6.16", "6.17", "6.18", "6.19", "6.20", "6.22", "6.23.8", "6.23.10"};

        public List<CReportZpz2023Full> Collect(string year)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            var zpzData1Q = CollectSummaryData1Q(year);
            var zpzData2Q = CollectSummaryData2Q(year);
            var zpzData3Q = CollectSummaryData3Q(year);
            var zpzData4Q = CollectSummaryData4Q(year);

            var reports = new List<CReportZpz2023Full>();

            var filials1Q = zpzData1Q.Select(x => x.Filial).Distinct().OrderBy(x => x);
            foreach (var filial in filials1Q)
            {
                var zpzFilialData1Q = zpzData1Q.Where(x => x.Filial == filial);
                var zpzFilialData2Q = zpzData2Q.Where(x => x.Filial == filial);
                var zpzFilialData3Q = zpzData3Q.Where(x => x.Filial == filial);
                var zpzFilialData4Q = zpzData4Q.Where(x => x.Filial == filial);
                var expertise1Q = MapExpertise(zpzFilialData1Q);
                var finance1Q = MapFinance(zpzFilialData1Q);
                var expertise2Q = MapExpertise(zpzFilialData2Q);
                var finance2Q = MapFinance(zpzFilialData2Q);
                var expertise3Q = MapExpertise(zpzFilialData3Q);
                var finance3Q = MapFinance(zpzFilialData3Q);
                var expertise4Q = MapExpertise(zpzFilialData4Q);
                var finance4Q = MapFinance(zpzFilialData4Q);
                var report = new CReportZpz2023Full
                {
                    Filial = filial,
                    Expertise1Q = expertise1Q,
                    Finance1Q = finance1Q,
                    Expertise2Q = expertise2Q,
                    Finance2Q = finance2Q,
                    Expertise3Q = expertise3Q,
                    Finance3Q = finance3Q,
                    Expertise4Q = expertise4Q,
                    Finance4Q = finance4Q,
                };
                reports.Add(report);
            }
            return reports;
        }

        private ZpzFinance2023Full MapFinance(IEnumerable<SummaryZpz2023> zpzFilialData)
        {
            var zpzTable8 = zpzFilialData.Where(x => x.Theme == "Таблица 8");
            return new ZpzFinance2023Full
            {
                SumPayment = zpzTable8.Where(x => x.RowNum == "1").Sum(x => x.SumSmo),
                SumNotPayment = zpzTable8.Where(x => x.RowNum == "2").Sum(x => x.SumSmo),
                SumMek = zpzTable8.Where(x => x.RowNum == "3").Sum(x => x.SumSmo),
                SumMee = zpzTable8.Where(x => x.RowNum == "4").Sum(x => x.SumSmo),
                SumEkmp = zpzTable8.Where(x => x.RowNum == "5").Sum(x => x.SumSmo)

            };
        }

        private ZpzExpertise2023Full MapExpertise(IEnumerable<SummaryZpz2023> zpzFilialData)
        {
            var table5Data = zpzFilialData.Where(x => x.Theme == "Таблица 5А");
            var table6Data = zpzFilialData.Where(x => x.Theme == "Таблица 6");
            var table7Data = zpzFilialData.Where(x => x.Theme == "Таблица 7");
            return new ZpzExpertise2023Full
            {
                Bills = table5Data.Sum(x => x.SumSmo),
                CountMeeTarget = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom),
                CountMeePlan = table6Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpomAnother),
                CountMeeComplaintTarget = table6Data.Where(x => x.RowNum == "1.8").Sum(x => x.SumVidpom),
                CountMeeComplaintPlan = table6Data.Where(x => x.RowNum == "1.8").Sum(x => x.SumVidpomAnother),
                CountMeeRepeat = table6Data.Where(x => x.RowNum == "1.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeOnco = table6Data.Where(x => x.RowNum == "1.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDs = table6Data.Where(x => x.RowNum == "1.11").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeLeth = table6Data.Where(x => x.RowNum == "1.12").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeInjured = table6Data.Where(x => x.RowNum == "1.13").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountMeeDefectedCaseTarget = table6Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpom),
                CountMeeDefectedCasePlan = table6Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpomAnother),
                CountMeeDefectsTarget = table6Data.Where(x => x.RowNum == "4").Sum(x => x.SumVidpom),
                CountMeeDefectsPlan = table6Data.Where(x => x.RowNum == "4").Sum(x => x.SumVidpomAnother),
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
                CountCaseEkmpTarget = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpom),
                CountCaseEkmpPlan = table7Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpomAnother),
                CountCaseEkmpComplaint = table7Data.Where(x => x.RowNum == "1.8").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpLeth = table7Data.Where(x => x.RowNum == "1.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpByMek = table7Data.Where(x => x.RowNum == "1.10").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpByMee = table7Data.Where(x => x.RowNum == "1.11").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpUTheme = table7Data.Where(x => x.RowNum == "2").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpMultiTarget = table7Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpom),
                CountCaseEkmpMultiPlan = table7Data.Where(x => x.RowNum == "3").Sum(x => x.SumVidpomAnother),
                CountCaseEkmpMultiLeth = table7Data.Where(x => x.RowNum == "3.9").Sum(x => x.SumVidpom+x.SumVidpomAnother),
                CountCaseEkmpMultiUthemeTarget = table7Data.Where(x => x.RowNum == "3.10").Sum(x => x.SumVidpom),
                CountCaseEkmpMultiUthemePlan = table7Data.Where(x => x.RowNum == "3.10").Sum(x => x.SumVidpomAnother),
                CountCaseDefectedBySmoTarget = table7Data.Where(x => x.RowNum == "5").Sum(x => x.SumVidpom),
                CountCaseDefectedBySmoPlan = table7Data.Where(x => x.RowNum == "5").Sum(x => x.SumVidpomAnother),
                CountEkmpDefectedCaseTarget = table7Data.Where(x => x.RowNum == "6").Sum(x => x.SumVidpom),
                CountEkmpDefectedCasePlan = table7Data.Where(x => x.RowNum == "6").Sum(x => x.SumVidpomAnother),
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

        private List<SummaryZpz2023> CollectSummaryData1Q(string year)
        {
            var period = Convert.ToString(Convert.ToInt32(year) - 2000)+"03";
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Zpz on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == period && 
                          flow.Id_Report_Type == "Zpz_Q"
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

        private List<SummaryZpz2023> CollectSummaryData2Q(string year)
        {
            var period = Convert.ToString(Convert.ToInt32(year) - 2000) + "06";
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Zpz on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == period &&
                          flow.Id_Report_Type == "Zpz_Q"
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

        private List<SummaryZpz2023> CollectSummaryData3Q(string year)
        {
            var period = Convert.ToString(Convert.ToInt32(year) - 2000) + "09";
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Zpz on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == period &&
                          flow.Id_Report_Type == "Zpz_Q"
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

        private List<SummaryZpz2023> CollectSummaryData4Q(string year)
        {
            var period = Convert.ToString(Convert.ToInt32(year) - 2000) + "12";
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_Zpz on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == period &&
                          flow.Id_Report_Type == "Zpz_Q"
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
