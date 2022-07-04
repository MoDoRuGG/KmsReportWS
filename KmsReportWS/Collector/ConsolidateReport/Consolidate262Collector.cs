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
    public class Consolidate262Collector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReport262Table3> CreateReport262T3(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report262_Table3 on rData.Id equals table
                        .Id_Report_Data
                    where flow.Yymm == yymm
                          && flow.Status == ReportStatus.Done.GetDescription()
                          && flow.Id_Report_Type == "f262"
                          && rData.Theme == "Таблица 3"
                    group new { flow, table } by new { flow.Id_Region }
                into flowGr
                    select new CReport262Table3
                    {
                        Filial = flowGr.Key.Id_Region,
                        Data = new Report262Table3Data
                        {
                            CountChannelAnother = flowGr.Sum(x => x.table.count_channel_another) ?? 0,
                            CountChannelAnotherChild = flowGr.Sum(x => x.table.count_channel_another_child) ?? 0,
                            CountChannelPhone = flowGr.Sum(x => x.table.count_channel_phone) ?? 0,
                            CountChannelPhoneChild = flowGr.Sum(x => x.table.count_channel_phone_child) ?? 0,
                            CountChannelSp = flowGr.Sum(x => x.table.count_channel_sp) ?? 0,
                            CountChannelSpChild = flowGr.Sum(x => x.table.count_channel_sp_child) ?? 0,
                            CountChannelTerminal = flowGr.Sum(x => x.table.count_channel_terminal) ?? 0,
                            CountChannelTerminalChild = flowGr.Sum(x => x.table.count_channel_terminal_child) ?? 0,
                            CountUnit = flowGr.Sum(x => x.table.count_unit) ?? 0,
                            CountUnitChild = flowGr.Sum(x => x.table.count_unit_child) ?? 0,
                            CountUnitWithSp = flowGr.Sum(x => x.table.count_unit_with_sp) ?? 0,
                            CountUnitWithSpChild = flowGr.Sum(x => x.table.count_unit_with_sp_child) ?? 0
                        }
                    }).ToList();
        }

        public List<CReport262Table2> CreateReport262T2(string yymmStart, string yymmEnd)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            int start = Convert.ToInt32(yymmStart);
            int end = Convert.ToInt32(yymmEnd);
            return (from flow in db.Report_Flow
                    join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                    join table in db.Report_f262 on rData.Id equals table
                        .Id_Report_Data
                    where Convert.ToInt32(flow.Yymm) >= start
                          && Convert.ToInt32(flow.Yymm) <= end
                          && flow.Status == ReportStatus.Done.GetDescription()
                          && flow.Id_Report_Type == "f262"
                          && rData.Theme == "Таблица 2"
                    group new { flow, table } by new { flow.Id_Region }
                into gr
                    select new CReport262Table2
                    {
                        Filial = gr.Key.Id_Region,
                        Data = new Report262DataDto
                        {
                            CountAddress = gr.Sum(x => x.table.Count_Address) ?? 0,
                            CountEmail = gr.Sum(x => x.table.Count_Email) ?? 0,
                            CountAnother = gr.Sum(x => x.table.Count_Another) ?? 0,
                            CountMessengers =
                                gr.Sum(x => x.table.Count_Messangers) ?? 0,
                            CountPhone = gr.Sum(x => x.table.Count_Phone) ?? 0,
                            CountPost = gr.Sum(x => x.table.Count_Post) ?? 0,
                            CountSms = gr.Sum(x => x.table.Count_Sms) ?? 0
                        }
                    }).ToList();
        }

        public List<CReport262Table1> CreateReport262T1(int year)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            int start = (year - 2000) * 100 + 1;
            int end = (year - 2000) * 100 + 12;
            var table1 = from flow in db.Report_Flow
                         join rData in db.Report_Data on flow.Id equals rData.Id_Flow
                         join table in db.Report_f262 on rData.Id equals table
                             .Id_Report_Data
                         where Convert.ToInt32(flow.Yymm) >= start
                               && Convert.ToInt32(flow.Yymm) <= end
                               && flow.Status == ReportStatus.Done.GetDescription()
                               && flow.Id_Report_Type == "f262"
                               && (rData.Theme == "Таблица 1" ||
                                   rData.Theme == "Таблица 2")
                         group new { flow, table } by new { flow.Id_Region, flow.Yymm }
                into flowGr
                         select new
                         {
                             filial = flowGr.Key.Id_Region,
                             yymm = flowGr.Key.Yymm,
                             countPpl = flowGr.Sum(x => x.table.Count_Ppl) ?? 0,
                             countInfo = flowGr.Sum(x =>
                                             x.table.Count_Sms + x.table.Count_Address +
                                             x.table.Count_Another +
                                             x.table.Count_Email +
                                             x.table.Count_Messangers +
                                             x.table.Count_Phone +
                                             x.table.Count_Post) ?? 0
                         };

            var reports = new List<CReport262Table1>();
            var currentReport = new CReport262Table1();

            var groups = table1
                .Select(x => new { x.filial}).Distinct();

            int[] months = new int[12];
            int month = 0;
            for (int i = start; i <= end; i++)
            {
                months[month] = i;
                month++;
            }

            foreach (var group in groups)
            {
                if (currentReport.Filial != group.filial.Trim())
                {
                    currentReport = new CReport262Table1
                    {
                        Filial = group.filial.Trim(),
                        ListOfCountPpl = new List<CReport262Table1Row>(),
                        ListOfCountInform = new List<CReport262Table1Row>()
                    };
                    reports.Add(currentReport);
                }


                foreach (var m in months)
                {
                    var data = table1.Where(x => x.filial == group.filial && x.yymm == m.ToString());
                    if (data.Any())
                    {
                        foreach (var d in data)
                        {
                            currentReport.ListOfCountPpl.Add(new CReport262Table1Row { Value = d.countPpl, yymm = m.ToString() });
                            currentReport.ListOfCountInform.Add(new CReport262Table1Row { Value = d.countInfo, yymm = m.ToString() });
                        }
                    }
                    else
                    {
                        currentReport.ListOfCountPpl.Add(new CReport262Table1Row { Value = 0, yymm = m.ToString() });
                        currentReport.ListOfCountInform.Add(new CReport262Table1Row { Value = 0, yymm = m.ToString() });
                    }
                }
           
            }

            return reports;
        }
    }
}