using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class F262Collector : BaseReportCollector
    {
        public F262Collector() : base(ReportType.F262)
        {
        }

        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flows = GetFilteredReportFlows(db, filials, yymmStart, yymmEnd, status);
                var table3Data = CollectTable3Data(flows);
                var table1Data = CollectTable1Data(flows);
                var table2Data = CollectTable2Data(flows);

                var outReport = new Report262 {ReportDataList = new List<Report262Dto>()};

                var data1 = new Report262Dto {Theme = "Таблица 1", Data = table1Data.ToList()};
                var data2 = new Report262Dto {Theme = "Таблица 2", Data = table2Data.ToList()};
                var table3 = new Report262Dto {Theme = "Таблица 3", Table3 = table3Data.ToList()};

                outReport.ReportDataList.Add(data1);
                outReport.ReportDataList.Add(data2);
                outReport.ReportDataList.Add(table3);

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting Summary f262");
                throw;
            }
        }

        private IQueryable<Report262Table3Data> CollectTable3Data(IQueryable<Report_Data> flows) =>
            from t in flows.Where(x => x.Theme == "Таблица 3").SelectMany(x => x.Report262_Table3)
            group t by t.mo
            into tGroup
            select new Report262Table3Data {
                Mo = tGroup.Key,
                CountChannelAnother = tGroup.Sum(x => x.count_channel_another ?? 0),
                CountChannelAnotherChild = tGroup.Sum(x => x.count_channel_another_child ?? 0),
                CountChannelPhone = tGroup.Sum(x => x.count_channel_phone ?? 0),
                CountChannelPhoneChild = tGroup.Sum(x => x.count_channel_phone_child ?? 0),
                CountChannelSp = tGroup.Sum(x => x.count_channel_sp ?? 0),
                CountChannelSpChild = tGroup.Sum(x => x.count_channel_sp_child ?? 0),
                CountChannelTerminal = tGroup.Sum(x => x.count_channel_terminal ?? 0),
                CountChannelTerminalChild = tGroup.Sum(x => x.count_channel_terminal_child ?? 0),
                CountUnit = tGroup.Sum(x => x.count_unit ?? 0),
                CountUnitChild = tGroup.Sum(x => x.count_unit_child ?? 0),
                CountUnitWithSp = tGroup.Sum(x => x.count_unit_with_sp ?? 0),
                CountUnitWithSpChild = tGroup.Sum(x => x.count_unit_with_sp_child ?? 0)
            };

        private IQueryable<Report262DataDto> CollectTable1Data(IQueryable<Report_Data> flows) =>
            from t in flows.Where(x => x.Theme == "Таблица 1").SelectMany(x => x.Report_f262)
            group t by t.Row_Num
            into tGroup
            select new Report262DataDto {
                RowNum = tGroup.Key ?? 1,
                CountPpl = tGroup.Sum(x => x.Count_Ppl ?? 0),
                CountPplFull = tGroup.Sum(x => x.Count_Ppl_Full ?? 0)
            };

        private IQueryable<Report262DataDto> CollectTable2Data(IQueryable<Report_Data> flows) =>
            from t in flows.Where(x => x.Theme == "Таблица 2").SelectMany(x => x.Report_f262)
            group t by t.Row_Num
            into tGroup
            select new Report262DataDto {
                RowNum = tGroup.Key ?? 1,
                CountAddress = tGroup.Sum(x => x.Count_Address ?? 0),
                CountAnother = tGroup.Sum(x => x.Count_Another ?? 0),
                CountEmail = tGroup.Sum(x => x.Count_Email ?? 0),
                CountMessengers = tGroup.Sum(x => x.Count_Messangers ?? 0),
                CountPhone = tGroup.Sum(x => x.Count_Phone ?? 0),
                CountPost = tGroup.Sum(x => x.Count_Post ?? 0),
                CountSms = tGroup.Sum(x => x.Count_Sms ?? 0)
            };
    }
}