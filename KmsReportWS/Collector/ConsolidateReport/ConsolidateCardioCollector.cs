using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateCardioCollector
    {

        private readonly string[] _themes =
           { "Таблица 1", "Таблица 3", "Таблица 6", "Таблица 8", "Таблица 10" };



        public List<ConsolidateCardio> Collect(string yymm)
        {
            string reportType = "PG_Q";

            var pgData = CollectSummaryData(yymm, reportType);

            var reports = new List<ConsolidateCardio>();

            var filials = pgData.Select(x => x.Filial).Distinct().OrderBy(x => x);

            foreach (var filial in filials)
            {
                var pgFilialData = pgData.Where(x => x.Filial == filial);
                var complaint = MapComplaint(pgFilialData);
                var protection = MapProtection(pgFilialData);
                var MeeEkmp = MapMeeEkmp(pgFilialData);
                var finance = MapFinance(pgFilialData);

                var report = new ConsolidateCardio
                {
                    Filial = filial,
                    Complaint = complaint,
                    Protection = protection,
                    Finance = finance,
                    MeeEkmp = MeeEkmp
                    
                    
                };
                reports.Add(report);
            }

            return reports;

        }



        private CardioComplaint MapComplaint(IEnumerable<SummaryPg> pgFilialData)
        {

            var table1Data = pgFilialData.Where(x => x.Theme == "Таблица 1");

            return new CardioComplaint
            {
                AskMedicalHelp = table1Data.Where(x => x.RowNum == "4.6.4").Sum(x => x.SumSmo + x.SumSmoAnother),
                MedicalHelp = table1Data.Where(x => x.RowNum == "3.6.3").Sum(x => x.SumSmo + x.SumSmoAnother),
                Underage = table1Data.Where(x => x.RowNum == "3.6.4").Sum(x => x.SumSmo + x.SumSmoAnother),
                UnderageAskMedicalHelp = table1Data.Where(x => x.RowNum == "4.6.5").Sum(x => x.SumSmo + x.SumSmoAnother)

            };

        }


        private CardioProtection MapProtection(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 3");

            return new CardioProtection
            {
                JudicalMedicalHelp = table3Data.Where(x => x.RowNum == "1.6.3").Sum(x => x.SumConflict),
                JudicalUnderage = table3Data.Where(x => x.RowNum == "1.6.4").Sum(x => x.SumConflict),
                PretrialMedicalHelp = table3Data.Where(x => x.RowNum == "1.6.3").Sum(x => x.SumSmo),
                PretrialUnderage = table3Data.Where(x => x.RowNum == "1.6.4").Sum(x => x.SumSmo),
            };

        }

        private CardioMeeEkmp MapMeeEkmp(IEnumerable<SummaryPg> pgFilialData)
        {
            string[] theme = {  "Таблица 6", "Таблица 8" };

            var table68Data = pgFilialData.Where(x => theme.Contains(x.Theme));
         

            return new CardioMeeEkmp
            {
                CelAllCardio = table68Data.Where(x => x.RowNum == "6.1.1.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelLetalKoronar = table68Data.Where(x => x.RowNum == "2.4.1" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelExpertiseMEE = table68Data.Where(x => x.RowNum == "5.3.1.3" && x.Theme == "Таблица 6").Sum(x => x.SumVidpom),
                CelLetalOnmk = table68Data.Where(x => x.RowNum == "2.4.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNeobosOtkaz = table68Data.Where(x => x.RowNum == "6.8.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNeobosOtkazUnderage = table68Data.Where(x => x.RowNum == "6.8.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNeprofilGospital = table68Data.Where(x => x.RowNum == "6.2.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNeprofilGospitalUnderage = table68Data.Where(x => x.RowNum == "6.2.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNevipolnenie = table68Data.Where(x => x.RowNum == "6.3.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNevipolnenieUnderage = table68Data.Where(x => x.RowNum == "6.3.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNotAddDispNab = table68Data.Where(x => x.RowNum == "6.4.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNotAddDispNabUnderage = table68Data.Where(x => x.RowNum == "6.4.4" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNotSobludClinicRecomendation = table68Data.Where(x => x.RowNum == "6.5.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelNotSobludClinicRecomendationUnderage = table68Data.Where(x => x.RowNum == "6.5.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelPrematureCloseHelpMerop = table68Data.Where(x => x.RowNum == "6.6.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelPrematureCloseHelpMeropUnderage = table68Data.Where(x => x.RowNum == "6.6.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelUnderage = table68Data.Where(x => x.RowNum == "6.1.1.4" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelViolationHospital = table68Data.Where(x => x.RowNum == "6.7.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                CelViolationHospitalUnderage = table68Data.Where(x => x.RowNum == "6.7.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                ComplaintsEKMP = table68Data.Where(x => x.RowNum == "2.2.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpom),
                ComplaintsMEE = table68Data.Where(x => x.RowNum == "2.2.2" && x.Theme == "Таблица 6").Sum(x => x.SumVidpom),
                HospitalizationMEE = table68Data.Where(x => x.RowNum == "2.5.2" && x.Theme == "Таблица 6").Sum(x => x.SumVidpom),
                PlanAllCardio = table68Data.Where(x => x.RowNum == "6.1.1.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanCardioUnderage = table68Data.Where(x => x.RowNum == "6.1.1.4" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanExpertiseMEE = table68Data.Where(x => x.RowNum == "5.3.1.3" && x.Theme == "Таблица 6").Sum(x => x.SumVidpomAnother),
                PlanNeobosOtkaz = table68Data.Where(x => x.RowNum == "6.8.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNeobosOtkazUnderage = table68Data.Where(x => x.RowNum == "6.8.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNeprofilGospital = table68Data.Where(x => x.RowNum == "6.2.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNeprofilGospitalUnderage = table68Data.Where(x => x.RowNum == "6.2.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNevipolnenie = table68Data.Where(x => x.RowNum == "6.3.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNevipolnenieUnderage = table68Data.Where(x => x.RowNum == "6.3.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNotAddDispNab = table68Data.Where(x => x.RowNum == "6.4.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNotAddDispNabUnderage = table68Data.Where(x => x.RowNum == "6.4.4" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNotSobludClinicRecomendation = table68Data.Where(x => x.RowNum == "6.5.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanNotSobludClinicRecomendationUnderage = table68Data.Where(x => x.RowNum == "6.5.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanPrematureCloseHelpMerop = table68Data.Where(x => x.RowNum == "6.6.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanPrematureCloseHelpMeropUnderage = table68Data.Where(x => x.RowNum == "6.6.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanViolationHospital = table68Data.Where(x => x.RowNum == "6.7.2" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother),
                PlanViolationHospitalUnderage = table68Data.Where(x => x.RowNum == "6.7.3" && x.Theme == "Таблица 8").Sum(x => x.SumVidpomAnother)


            };

        }


        private CardioFinance MapFinance(IEnumerable<SummaryPg> pgFilialData)
        {
            var table10Data = pgFilialData.Where(x => x.Theme == "Таблица 10");
            return new CardioFinance
            {
                SumCloseHelp = table10Data.Where(x => x.RowNum == "5.5.2" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumCloseHelpUnderage = table10Data.Where(x => x.RowNum == "5.5.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumEkmpNotTimeDispan = table10Data.Where(x => x.RowNum == "5.1.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumEkmpNotTimeDispanUnderage = table10Data.Where(x => x.RowNum == "5.1.4" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumMeeNotTimeDispan = table10Data.Where(x => x.RowNum == "4.1.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumMeeNotTimeDispanUnderage = table10Data.Where(x => x.RowNum == "4.1.4" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumNeprofilHelp = table10Data.Where(x => x.RowNum == "5.2.2" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumNeprofilHelpUnderage = table10Data.Where(x => x.RowNum == "5.2.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumNesobludRecomedation = table10Data.Where(x => x.RowNum == "5.4.2" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumNesobludRecomedationUnderage = table10Data.Where(x => x.RowNum == "5.4.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumNevipolnenie = table10Data.Where(x => x.RowNum == "5.3.2" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumNevipolnenieUnderage = table10Data.Where(x => x.RowNum == "5.3.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumViolation = table10Data.Where(x => x.RowNum == "5.6.2" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),
                SumViolationUnderage = table10Data.Where(x => x.RowNum == "5.6.3" && x.Theme == "Таблица 10").Sum(x => x.SumVidpom),

            };
        }


        private List<SummaryPg> CollectSummaryData(string yymm, string reportType)
        {
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join reg in db.Region on flow.Id_Region equals reg.id
                    join table in db.Report_Pg on rData.Id equals table.Id_Report_Data
                    where flow.Yymm == yymm
                          && flow.Status != ReportStatus.Refuse.GetDescription()
                          && flow.Id_Report_Type == reportType
                          && _themes.Contains(rData.Theme)
                    group new { flow, rData, table } by new { flow.Id_Region,reg.name, rData.Theme, table.RowNum }
                          into gr
                    select new SummaryPg
                    {
                        Filial = gr.Key.name,
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
                        SumInsured = gr.Sum(x => x.table.CountInsured) ?? 0,
                        SumInsuredRepresentative = gr.Sum(x => x.table.CountInsuredRepresentative) ?? 0,
                        SumProsecutor = gr.Sum(x => x.table.CountProsecutor) ?? 0,
                        SumTfoms = gr.Sum(x => x.table.CountTfoms) ?? 0,
                        SumVmp = gr.Sum(x=>x.table.CountStacVmp) ?? 0
                    }).ToList();
        }
    }
}