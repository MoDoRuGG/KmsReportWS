using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateDispCollector
    {
        private readonly string[] _themes =
          { "Таблица 1", "Таблица 3", "Таблица 6", "Таблица 8", "Таблица 10" };

        public List<ConsolidateDisp> Collect(string yymm)
        {
            string reportType = "PG_Q";

            var pgData = CollectSummaryData(yymm, reportType);

            var reports = new List<ConsolidateDisp>();

            var filials = pgData.Select(x => x.Filial).Distinct().OrderBy(x => x);

            foreach (var filial in filials)
            {
                var pgFilialData = pgData.Where(x => x.Filial == filial);
                var complaint = MapComplaint(pgFilialData);
                var protection = MapProtection(pgFilialData);
                var finance = MapFinance(pgFilialData);
                var mee = MapMee(pgFilialData);
                var mek = MapMek(pgFilialData);
                var ekmp = MapEkmp(pgFilialData);


                var report = new ConsolidateDisp
                {
                    Filial = filial,
                    Complaint = complaint,
                    Protection = protection,
                    Finance = finance,
                    Mee = mee,
                    Mek = mek,
                    Ekmp = ekmp

                };
                reports.Add(report);
            }

            return reports;

        }

        private DispComplaint MapComplaint(IEnumerable<SummaryPg> pgFilialData)
        {

            var table1Data = pgFilialData.Where(x => x.Theme == "Таблица 1");

            return new DispComplaint
            {
                Row361Gr7 = table1Data.Where(x => x.RowNum == "3.6.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row365Gr7 = table1Data.Where(x => x.RowNum == "3.6.5").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row37Gr7 = table1Data.Where(x => x.RowNum == "3.7").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row371Gr7 = table1Data.Where(x => x.RowNum == "3.7.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row372Gr7 = table1Data.Where(x => x.RowNum == "3.7.2").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row3721Gr7 = table1Data.Where(x => x.RowNum == "3.7.2.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row373Gr7 = table1Data.Where(x => x.RowNum == "3.7.3").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row3731Gr7 = table1Data.Where(x => x.RowNum == "3.7.3.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row462Gr7 = table1Data.Where(x => x.RowNum == "4.6.2").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row47Gr7 = table1Data.Where(x => x.RowNum == "4.7").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row471Gr7 = table1Data.Where(x => x.RowNum == "4.7.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row472Gr7 = table1Data.Where(x => x.RowNum == "4.7.2").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row4721Gr7 = table1Data.Where(x => x.RowNum == "4.7.2.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row473Gr7 = table1Data.Where(x => x.RowNum == "4.7.3").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row4731Gr7 = table1Data.Where(x => x.RowNum == "4.7.3.1").Sum(x => x.SumSmo + x.SumSmoAnother),
                Row49Gr7 = table1Data.Where(x => x.RowNum == "4.9").Sum(x => x.SumSmo + x.SumSmoAnother),

            };

        }

        private DispProtection MapProtection(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 3");

            return new DispProtection
            {
                Row161Gr3 = table3Data.Where(x => x.RowNum == "1.6.1").Sum(x => x.SumConflict),
                Row165Gr3 = table3Data.Where(x => x.RowNum == "1.6.5").Sum(x => x.SumConflict),
                Row17Gr3 = table3Data.Where(x => x.RowNum == "1.7").Sum(x => x.SumConflict),
                Row171Gr3 = table3Data.Where(x => x.RowNum == "1.7.1").Sum(x => x.SumConflict),
                Row172Gr3 = table3Data.Where(x => x.RowNum == "1.7.2").Sum(x => x.SumConflict),
                Row1721Gr3 = table3Data.Where(x => x.RowNum == "1.7.2.1").Sum(x => x.SumConflict),
                Row173Gr3 = table3Data.Where(x => x.RowNum == "1.7.3").Sum(x => x.SumConflict),
                Row1731Gr3 = table3Data.Where(x => x.RowNum == "1.7.3.1").Sum(x => x.SumConflict),

                Row161Gr6 = table3Data.Where(x => x.RowNum == "1.6.1").Sum(x => x.SumSmo),
                Row165Gr6 = table3Data.Where(x => x.RowNum == "1.6.5").Sum(x => x.SumSmo),
                Row17Gr6 = table3Data.Where(x => x.RowNum == "1.7").Sum(x => x.SumSmo),
                Row171Gr6 = table3Data.Where(x => x.RowNum == "1.7.1").Sum(x => x.SumSmo),
                Row172Gr6 = table3Data.Where(x => x.RowNum == "1.7.2").Sum(x => x.SumSmo),
                Row1721Gr6 = table3Data.Where(x => x.RowNum == "1.7.2.1").Sum(x => x.SumSmo),
                Row173Gr6 = table3Data.Where(x => x.RowNum == "1.7.3").Sum(x => x.SumSmo),
                Row1731Gr6 = table3Data.Where(x => x.RowNum == "1.7.3.1").Sum(x => x.SumSmo),

            };

        }

        private DispMek MapMek(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 5");

            return new DispMek
            {
                Row1Gr3 = table3Data.Where(x => x.RowNum == "1").Sum(x => x.SumVidpomGr3),
                Row12Gr3 = table3Data.Where(x => x.RowNum == "1.2").Sum(x => x.SumVidpomGr3),
                Row4Gr3 = table3Data.Where(x => x.RowNum == "4").Sum(x => x.SumVidpomGr3),
                Row41Gr3 = table3Data.Where(x => x.RowNum == "4.1").Sum(x => x.SumVidpomGr3),
                Row411Gr3 = table3Data.Where(x => x.RowNum == "4.1.1").Sum(x => x.SumVidpomGr3),
                Row42Gr3 = table3Data.Where(x => x.RowNum == "4.2.1").Sum(x => x.SumVidpomGr3),
                Row421Gr3 = table3Data.Where(x => x.RowNum == "4.2.1").Sum(x => x.SumVidpomGr3),
                Row43Gr3 = table3Data.Where(x => x.RowNum == "4.3").Sum(x => x.SumVidpomGr3),
                Row431Gr3 = table3Data.Where(x => x.RowNum == "4.3.1").Sum(x => x.SumVidpomGr3),
                Row44Gr3 = table3Data.Where(x => x.RowNum == "4.4").Sum(x => x.SumVidpomGr3),
                Row441Gr3 = table3Data.Where(x => x.RowNum == "4.4.1").Sum(x => x.SumVidpomGr3),
                Row45Gr3 = table3Data.Where(x => x.RowNum == "4.5").Sum(x => x.SumVidpomGr3),
                Row451Gr3 = table3Data.Where(x => x.RowNum == "4.5.1").Sum(x => x.SumVidpomGr3),
                Row5Gr3 = table3Data.Where(x => x.RowNum == "5").Sum(x => x.SumVidpomGr3),
                Row51Gr3 = table3Data.Where(x => x.RowNum == "5.1").Sum(x => x.SumVidpomGr3),
                Row52Gr3 = table3Data.Where(x => x.RowNum == "5.2").Sum(x => x.SumVidpomGr3),


            };

        }

        private DispMee MapMee(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 6");

            return new DispMee
            {
                Row21Gr3 = table3Data.Where(x => x.RowNum == "2.1").Sum(x => x.SumVidpom),
                Row22Gr3 = table3Data.Where(x => x.RowNum == "2.2").Sum(x => x.SumVidpom),
                Row221Gr3 = table3Data.Where(x => x.RowNum == "2.2.1").Sum(x => x.SumVidpom),
                Row222Gr3 = table3Data.Where(x => x.RowNum == "2.2.2").Sum(x => x.SumVidpom),
                Row223Gr3 = table3Data.Where(x => x.RowNum == "2.2.3").Sum(x => x.SumVidpom),
                Row24Gr3 = table3Data.Where(x => x.RowNum == "2.4").Sum(x => x.SumVidpom),
                Row241Gr3 = table3Data.Where(x => x.RowNum == "2.4.1").Sum(x => x.SumVidpom),
                Row26Gr10 = table3Data.Where(x => x.RowNum == "2.6").Sum(x => x.SumVidpomAnother),

                Row531Gr3 = table3Data.Where(x => x.RowNum == "5.3.1").Sum(x => x.SumVidpom),
                Row5311Gr3 = table3Data.Where(x => x.RowNum == "5.3.1.1").Sum(x => x.SumVidpom),
                Row54Gr3 = table3Data.Where(x => x.RowNum == "5.4").Sum(x => x.SumVidpom),
                Row55Gr3 = table3Data.Where(x => x.RowNum == "5.5").Sum(x => x.SumVidpom),
                Row56Gr3 = table3Data.Where(x => x.RowNum == "5.6").Sum(x => x.SumVidpom),
                Row561Gr3 = table3Data.Where(x => x.RowNum == "5.6.1").Sum(x => x.SumVidpom),

                Row531Gr10 = table3Data.Where(x => x.RowNum == "5.3.1").Sum(x => x.SumVidpomAnother),
                Row5311Gr10 = table3Data.Where(x => x.RowNum == "5.3.1.1").Sum(x => x.SumVidpomAnother),
                Row54Gr10 = table3Data.Where(x => x.RowNum == "5.4").Sum(x => x.SumVidpomAnother),
                Row55Gr10 = table3Data.Where(x => x.RowNum == "5.5").Sum(x => x.SumVidpomAnother),
                Row56Gr10 = table3Data.Where(x => x.RowNum == "5.6").Sum(x => x.SumVidpomAnother),
                Row561Gr10 = table3Data.Where(x => x.RowNum == "5.6.1").Sum(x => x.SumVidpomAnother),

            };

        }

        private DispEkmp MapEkmp(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 8");

            return new DispEkmp
            {
                Row21Gr3 = table3Data.Where(x => x.RowNum == "2.1").Sum(x => x.SumVidpom),
                Row223Gr3 = table3Data.Where(x => x.RowNum == "2.2.3").Sum(x => x.SumVidpom),

                Row25Gr3 = table3Data.Where(x => x.RowNum == "2.5").Sum(x => x.SumVidpom),
                Row251Gr3 = table3Data.Where(x => x.RowNum == "2.5.1").Sum(x => x.SumVidpom),
                Row611Gr3 = table3Data.Where(x => x.RowNum == "6.1.1").Sum(x => x.SumVidpom),
                Row6111Gr3 = table3Data.Where(x => x.RowNum == "6.1.1.1").Sum(x => x.SumVidpom),
                Row62Gr3 = table3Data.Where(x => x.RowNum == "6.2").Sum(x => x.SumVidpom),
                Row621Gr3 = table3Data.Where(x => x.RowNum == "6.2.1").Sum(x => x.SumVidpom),
                Row63Gr3 = table3Data.Where(x => x.RowNum == "6.3").Sum(x => x.SumVidpom),
                Row631Gr3 = table3Data.Where(x => x.RowNum == "6.3.1").Sum(x => x.SumVidpom),
                Row632Gr3 = table3Data.Where(x => x.RowNum == "6.3.2").Sum(x => x.SumVidpom),
                Row633Gr3 = table3Data.Where(x => x.RowNum == "6.3.3").Sum(x => x.SumVidpom),
                Row64Gr3 = table3Data.Where(x => x.RowNum == "6.4").Sum(x => x.SumVidpom),
                Row641Gr3 = table3Data.Where(x => x.RowNum == "6.4.1").Sum(x => x.SumVidpom),
                Row642Gr3 = table3Data.Where(x => x.RowNum == "6.4.2").Sum(x => x.SumVidpom),
                Row643Gr3 = table3Data.Where(x => x.RowNum == "6.4.3").Sum(x => x.SumVidpom),
                Row644Gr3 = table3Data.Where(x => x.RowNum == "6.4.4").Sum(x => x.SumVidpom),
                Row645Gr3 = table3Data.Where(x => x.RowNum == "6.4.5").Sum(x => x.SumVidpom),
                Row651Gr3 = table3Data.Where(x => x.RowNum == "6.5.1").Sum(x => x.SumVidpom),
                Row652Gr3 = table3Data.Where(x => x.RowNum == "6.5.2").Sum(x => x.SumVidpom),
                Row653Gr3 = table3Data.Where(x => x.RowNum == "6.5.3").Sum(x => x.SumVidpom),

                Row25Gr10 = table3Data.Where(x => x.RowNum == "2.5").Sum(x => x.SumVidpomAnother),
                Row251Gr10 = table3Data.Where(x => x.RowNum == "2.5.1").Sum(x => x.SumVidpomAnother),
                Row611Gr10 = table3Data.Where(x => x.RowNum == "6.1.1").Sum(x => x.SumVidpomAnother),
                Row6111Gr10 = table3Data.Where(x => x.RowNum == "6.1.1.1").Sum(x => x.SumVidpomAnother),
                Row62Gr10 = table3Data.Where(x => x.RowNum == "6.2").Sum(x => x.SumVidpomAnother),
                Row621Gr10 = table3Data.Where(x => x.RowNum == "6.2.1").Sum(x => x.SumVidpomAnother),
                Row63Gr10 = table3Data.Where(x => x.RowNum == "6.3").Sum(x => x.SumVidpomAnother),
                Row631Gr10 = table3Data.Where(x => x.RowNum == "6.3.1").Sum(x => x.SumVidpomAnother),
                Row632Gr10 = table3Data.Where(x => x.RowNum == "6.3.2").Sum(x => x.SumVidpomAnother),
                Row633Gr10 = table3Data.Where(x => x.RowNum == "6.3.3").Sum(x => x.SumVidpomAnother),
                Row64Gr10 = table3Data.Where(x => x.RowNum == "6.4").Sum(x => x.SumVidpomAnother),
                Row641Gr10 = table3Data.Where(x => x.RowNum == "6.4.1").Sum(x => x.SumVidpomAnother),
                Row642Gr10 = table3Data.Where(x => x.RowNum == "6.4.2").Sum(x => x.SumVidpomAnother),
                Row643Gr10 = table3Data.Where(x => x.RowNum == "6.4.3").Sum(x => x.SumVidpomAnother),
                Row644Gr10 = table3Data.Where(x => x.RowNum == "6.4.4").Sum(x => x.SumVidpomAnother),
                Row645Gr10 = table3Data.Where(x => x.RowNum == "6.4.5").Sum(x => x.SumVidpomAnother),
                Row651Gr10 = table3Data.Where(x => x.RowNum == "6.5.1").Sum(x => x.SumVidpomAnother),
                Row652Gr10 = table3Data.Where(x => x.RowNum == "6.5.2").Sum(x => x.SumVidpomAnother),
                Row653Gr10 = table3Data.Where(x => x.RowNum == "6.5.3").Sum(x => x.SumVidpomAnother),


            };

        }


        private DispFinance MapFinance(IEnumerable<SummaryPg> pgFilialData)
        {
            var table3Data = pgFilialData.Where(x => x.Theme == "Таблица 10");

            return new DispFinance
            {
                Row4Gr3 = table3Data.Where(x => x.RowNum == "4").Sum(x => x.SumSmo),
                Row41Gr3 = table3Data.Where(x => x.RowNum == "4.1").Sum(x => x.SumSmo),
                Row412Gr3 = table3Data.Where(x => x.RowNum == "4.1.2").Sum(x => x.SumSmo),
                Row413Gr3 = table3Data.Where(x => x.RowNum == "4.1.3").Sum(x => x.SumSmo),
                Row414Gr3 = table3Data.Where(x => x.RowNum == "4.1.4").Sum(x => x.SumSmo),
                Row51Gr3 = table3Data.Where(x => x.RowNum == "5.1").Sum(x => x.SumSmo),
                Row511Gr3 = table3Data.Where(x => x.RowNum == "5.1.1").Sum(x => x.SumSmo),
                Row512Gr3 = table3Data.Where(x => x.RowNum == "5.1.2").Sum(x => x.SumSmo),
                Row513Gr3 = table3Data.Where(x => x.RowNum == "5.1.3").Sum(x => x.SumSmo),
                Row514Gr3 = table3Data.Where(x => x.RowNum == "5.1.4").Sum(x => x.SumSmo),
                Row52Gr3 = table3Data.Where(x => x.RowNum == "5.2").Sum(x => x.SumSmo),
                Row521Gr3 = table3Data.Where(x => x.RowNum == "5.2.1").Sum(x => x.SumSmo),
                Row522Gr3 = table3Data.Where(x => x.RowNum == "5.2.2").Sum(x => x.SumSmo),
                Row523Gr3 = table3Data.Where(x => x.RowNum == "5.2.3").Sum(x => x.SumSmo),
                Row53Gr3 = table3Data.Where(x => x.RowNum == "5.3").Sum(x => x.SumSmo),
                Row531Gr3 = table3Data.Where(x => x.RowNum == "5.3.1").Sum(x => x.SumSmo),
                Row532Gr3 = table3Data.Where(x => x.RowNum == "5.3.2").Sum(x => x.SumSmo),
                Row533Gr3 = table3Data.Where(x => x.RowNum == "5.3.3").Sum(x => x.SumSmo),
                Row54Gr3 = table3Data.Where(x => x.RowNum == "5.4").Sum(x => x.SumSmo),
                Row541Gr3 = table3Data.Where(x => x.RowNum == "5.4.1").Sum(x => x.SumSmo),
                Row542Gr3 = table3Data.Where(x => x.RowNum == "5.4.2").Sum(x => x.SumSmo),
                Row55Gr3 = table3Data.Where(x => x.RowNum == "5.5").Sum(x => x.SumSmo),
                Row551Gr3 = table3Data.Where(x => x.RowNum == "5.5.1").Sum(x => x.SumSmo),
                Row552Gr3 = table3Data.Where(x => x.RowNum == "5.5.2").Sum(x => x.SumSmo),
                Row553Gr3 = table3Data.Where(x => x.RowNum == "5.5.3").Sum(x => x.SumSmo),
                Row56Gr3 = table3Data.Where(x => x.RowNum == "5.6").Sum(x => x.SumSmo),
                Row561Gr3 = table3Data.Where(x => x.RowNum == "5.6.1").Sum(x => x.SumSmo),
                Row562Gr3 = table3Data.Where(x => x.RowNum == "5.6.2").Sum(x => x.SumSmo),
                Row563Gr3 = table3Data.Where(x => x.RowNum == "5.6.3").Sum(x => x.SumSmo),

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
                    group new { flow, rData, table } by new { flow.Id_Region, reg.name, rData.Theme, table.RowNum }
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
                        SumVmp = gr.Sum(x => x.table.CountStacVmp) ?? 0
                    }).ToList();
        }

    }
}