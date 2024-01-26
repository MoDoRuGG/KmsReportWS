using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class Consolidate294Collector
    {
        private static readonly string[] statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        public Consolidate294 Collect(string yymm)
        {
            var connStr = Settings.Default.ConnStr;
            using var db = new LinqToSqlKmsReportDataContext(connStr);
            int yymmStart = Convert.ToInt32(yymm.Substring(0, 2) + "01");
            int yymmEnd = Convert.ToInt32(yymm);

            return new Consolidate294 {
                Disp13List = CollectDisp13(db, yymmStart, yymmEnd),
                Disp12List = CollectDisp12(db, yymmStart, yymmEnd),
                Disp2List = CollectDisp2(db, yymmStart, yymmEnd),
                DispensaryObservationList = CollectDispensaryObservation(db, yymmStart, yymmEnd),
                EfficiencyList = CollectEfficiency(db, yymmStart, yymmEnd),
                InsuranceRepresentativeList = CollectInsuranceRepresentative(db, yymmStart, yymmEnd),
                PhoneQuestionaryList = CollectPhoneQuestionary(db, yymmStart, yymmEnd),
                TreatmentList = CollectTreatment(db, yymmStart, yymmEnd)
            };
        }

        public List<Dispanserization> CollectDisp13(LinqToSqlKmsReportDataContext db, int yymmStart, int yymmEnd)
        {
            string[] themeList = {"Таблица 2", "Таблица 3"};
            var dispList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && themeList.Contains(data.Theme)
                      && statuses.Contains(flow.Status)
                      && flow.Id_Region != "RU-KHA"
                group new {flow, data, f, r} by new {r.name, flow.Yymm}
                into gr
                select new Dispanserization {
                    Filial = gr.Key.name,
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    CountPeoplelMo =
                        gr.Where(x => x.data.Theme == "Таблица 2" && x.f.RowNum.StartsWith("01."))
                            .Sum(x => x.f.CountPpl) ?? 0,
                    Sms = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountSms) ?? 0,
                    Post = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountPost) ?? 0,
                    Phone = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountPhone) ?? 0,
                    Messangers = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountMessangers) ?? 0,
                    EMail = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountEmail) ?? 0,
                    Address = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountAddress) ?? 0,
                    AnotherType = gr.Where(x => x.data.Theme == "Таблица 3").Sum(x => x.f.CountAnother) ?? 0,
                    CountPeopleInform = gr.Where(x => x.data.Theme == "Таблица 3" && x.f.RowNum.StartsWith("01."))
                                            .Sum(x => x.f.CountSms + x.f.CountPost + x.f.CountPhone +
                                                      x.f.CountMessangers +
                                                      x.f.CountEmail + x.f.CountAddress + x.f.CountAnother) ?? 0,
                    CountPeopleRepeatInform = gr.Where(x => x.data.Theme == "Таблица 3" && x.f.RowNum.StartsWith("02."))
                                                  .Sum(x => x.f.CountSms + x.f.CountPost + x.f.CountPhone +
                                                            x.f.CountMessangers +
                                                            x.f.CountEmail + x.f.CountAddress + x.f.CountAnother) ?? 0,
                };
            return dispList.ToList();
        }

        public List<Dispanserization> CollectDisp12(LinqToSqlKmsReportDataContext db, int yymmStart, int yymmEnd)
        {
            string[] themeList = {"Таблица 2", "Таблица 4"};
            var dispList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && themeList.Contains(data.Theme)
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new Dispanserization {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    CountPeoplelMo =
                        gr.Where(x => x.data.Theme == "Таблица 2" && x.f.RowNum.StartsWith("02."))
                            .Sum(x => x.f.CountPpl) ?? 0,
                    Sms = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountSms) ?? 0,
                    Post = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountPost) ?? 0,
                    Phone = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountPhone) ?? 0,
                    Messangers = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountMessangers) ?? 0,
                    EMail = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountEmail) ?? 0,
                    Address = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountAddress) ?? 0,
                    AnotherType = gr.Where(x => x.data.Theme == "Таблица 4").Sum(x => x.f.CountAnother) ?? 0,
                    CountPeopleInform = gr.Where(x => x.data.Theme == "Таблица 4" && x.f.RowNum.StartsWith("01."))
                                            .Sum(x => x.f.CountSms + x.f.CountPost + x.f.CountPhone +
                                                      x.f.CountMessangers +
                                                      x.f.CountEmail + x.f.CountAddress + x.f.CountAnother) ?? 0,
                    CountPeopleRepeatInform = gr.Where(x => x.data.Theme == "Таблица 4" && x.f.RowNum.StartsWith("02."))
                                                  .Sum(x => x.f.CountSms + x.f.CountPost + x.f.CountPhone +
                                                            x.f.CountMessangers +
                                                            x.f.CountEmail + x.f.CountAddress + x.f.CountAnother) ?? 0,
                };
            return dispList.ToList();
        }

        public List<Dispanserization> CollectDisp2(LinqToSqlKmsReportDataContext db, int yymmStart, int yymmEnd)
        {
            var dispList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && data.Theme == "Таблица 5"
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new Dispanserization {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    Sms = gr.Sum(x => x.f.CountSms) ?? 0,
                    Post = gr.Sum(x => x.f.CountPost) ?? 0,
                    Phone = gr.Sum(x => x.f.CountPhone) ?? 0,
                    Messangers = gr.Sum(x => x.f.CountMessangers) ?? 0,
                    EMail = gr.Sum(x => x.f.CountEmail) ?? 0,
                    Address = gr.Sum(x => x.f.CountAddress) ?? 0,
                    AnotherType = gr.Sum(x => x.f.CountAnother) ?? 0,
                    CountPeoplelMo = gr.Sum(x => x.f.CountSms + x.f.CountPost + x.f.CountPhone +
                                                 x.f.CountMessangers + x.f.CountEmail + x.f.CountAddress +
                                                 x.f.CountAnother) ?? 0
                };
            return dispList.ToList();
        }

        public List<DispensaryObservation> CollectDispensaryObservation(LinqToSqlKmsReportDataContext db, int yymmStart,
            int yymmEnd)
        {
            var dispensaryObservationList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && data.Theme == "Таблица 6"
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new DispensaryObservation {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    Onco = gr.Sum(x => x.f.CountOncologicalDisease) ?? 0,
                    Endo = gr.Sum(x => x.f.CountEndocrineDisease) ?? 0,
                    Broncho = gr.Sum(x => x.f.CountBronchoDisease) ?? 0,
                    BloodCirculatory = gr.Sum(x => x.f.CountBloodDisease) ?? 0,
                    NotInfection = gr.Sum(x => x.f.CountAnotherDisease) ?? 0,
                    CountPeopleInform = gr.Sum(x => x.f.CountOncologicalDisease + x.f.CountEndocrineDisease +
                                                    x.f.CountBloodDisease + x.f.CountBronchoDisease +
                                                    x.f.CountAnotherDisease) ?? 0
                };
            return dispensaryObservationList.ToList();
        }

        public List<PhoneQuestionary> CollectPhoneQuestionary(LinqToSqlKmsReportDataContext db, int yymmStart,
            int yymmEnd)
        {
            var phoneQuestionaryList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && data.Theme == "Таблица 7"
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new PhoneQuestionary {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    Prof = gr.Where(x => x.f.RowNum.StartsWith("01.")).Sum(x => x.f.CountPpl) ?? 0,
                    Disp = gr.Where(x => x.f.RowNum.StartsWith("02.")).Sum(x => x.f.CountPpl) ?? 0,
                };
            return phoneQuestionaryList.ToList();
        }

        public List<InsuranceRepresentative> CollectInsuranceRepresentative(LinqToSqlKmsReportDataContext db,
            int yymmStart, int yymmEnd)
        {
            var insuranceRepresentativeList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && data.Theme == "Таблица 8"
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new InsuranceRepresentative {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    FirstLevel = gr.Where(x => x.f.RowNum == "01.3").Sum(x => x.f.CountPpl) ?? 0,
                    FirstLevelTraining = gr.Where(x => x.f.RowNum == "01.4").Sum(x => x.f.CountPpl) ?? 0,
                    SecondLevel = gr.Where(x => x.f.RowNum == "02").Sum(x => x.f.CountPpl) ?? 0,
                    SecondLevelTraining = gr.Where(x => x.f.RowNum == "02.1").Sum(x => x.f.CountPpl) ?? 0,
                    ThirdLevel = gr.Where(x => x.f.RowNum == "03").Sum(x => x.f.CountPpl) ?? 0,
                    ThirdLevelTraining = gr.Where(x => x.f.RowNum == "03.1").Sum(x => x.f.CountPpl) ?? 0,
                };
            return insuranceRepresentativeList.ToList();
        }

        public List<Treatment> CollectTreatment(LinqToSqlKmsReportDataContext db, int yymmStart, int yymmEnd)
        {
            var treatmentList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && data.Theme == "Таблица 9"
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new Treatment {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    Oral = gr.Where(x => x.f.RowNum == "01").Sum(x => x.f.CountPpl) ?? 0,
                    OralSecond = gr.Where(x => x.f.RowNum == "01.1").Sum(x => x.f.CountPpl) ?? 0,
                    OralThird = gr.Where(x => x.f.RowNum == "01.2").Sum(x => x.f.CountPpl) ?? 0,
                    Written = gr.Where(x => x.f.RowNum.StartsWith("02.")).Sum(x => x.f.CountPpl) ?? 0,
                    WrittenSecond =
                        gr.Where(x => x.f.RowNum == "02.2" || x.f.RowNum == "02.3").Sum(x => x.f.CountPpl) ?? 0,
                    WrittenThird =
                        gr.Where(x => x.f.RowNum == "02.5" || x.f.RowNum == "02.6").Sum(x => x.f.CountPpl) ?? 0,
                    WrittenDoctor = gr.Where(x => x.f.RowNum == "03").Sum(x => x.f.CountPpl) ?? 0,
                };
            return treatmentList.ToList();
        }

        public List<Efficiency> CollectEfficiency(LinqToSqlKmsReportDataContext db, int yymmStart, int yymmEnd)
        {
            var efficiencyList = from flow in db.Report_Flow
                join data in db.Report_Data on flow.Id equals data.Id_Flow
                join f in db.Report_f294 on data.Id equals f.Id_Report_Data
                join r in db.Region on flow.Id_Region equals r.id
                where Convert.ToInt32(flow.Yymm) >= yymmStart
                      && Convert.ToInt32(flow.Yymm) <= yymmEnd
                      && data.Theme == "Эффективность"
                      && statuses.Contains(flow.Status)
                group new {flow, data, f} by new {r.name, flow.Yymm}
                into gr
                select new Efficiency {
                    Filial = gr.Key.name.Trim(),
                    Month = YymmUtils.ConvertYymmToMonth(gr.Key.Yymm),
                    Disp13Inform = gr.Where(x => x.f.RowNum == "Д13.8").Sum(x => x.f.CountPpl) ?? 0,
                    Disp12Inform = gr.Where(x => x.f.RowNum == "Д12.8").Sum(x => x.f.CountPpl) ?? 0,
                    AimedDisp2 = gr.Where(x => x.f.RowNum == "Д2.1").Sum(x => x.f.CountPpl) ?? 0,
                    Disp2 = gr.Where(x => x.f.RowNum == "Д2.3").Sum(x => x.f.CountPpl) ?? 0,
                    SubjectToDisp = gr.Where(x => x.f.RowNum == "ДН.1").Sum(x => x.f.CountPpl) ?? 0
                };
            return efficiencyList.ToList();
        }
    }
}