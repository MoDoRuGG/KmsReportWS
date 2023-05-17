using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class IizlCollector : BaseReportCollector
    {
        public IizlCollector() : base(ReportType.Iizl)
        {
        }

        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flows = GetFilteredReportFlows(db, filials, yymmStart, yymmEnd, status);
                var groupTheme = from f in flows
                    group f by f.Theme
                    into fgr
                    select new ReportIizlDto {
                        Theme = fgr.Key,
                        TotalPersFirst = fgr.Sum(x => Convert.ToInt32(x.General_field_1 ?? 0)),
                        TotalPersRepeat = fgr.Sum(x => Convert.ToInt32(x.General_field_2 ?? 0)),
                        Data = new List<ReportIizlDataDto>()
                    };

                var outReport = new ReportIizl {ReportDataList = new List<ReportIizlDto>()};

                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportPgDto = new ReportIizlDto {
                        Theme = theme.Theme,
                        Data = data.ToList(),
                        TotalPersFirst = theme.TotalPersFirst,
                        TotalPersRepeat = theme.TotalPersRepeat
                    };
                    outReport.ReportDataList.Add(reportPgDto);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary Iizl");
                throw;
            }
        }

        private IQueryable<ReportIizlDataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_Iilz)
            group f by f.Code
            into fgr
            select new ReportIizlDataDto {
                Code = fgr.Key,
                AccountingDocument = "",
                CountPersRepeat = fgr.Sum(x => Convert.ToInt32(x.Count_Pers_Repeat ?? 0)),
                CountMessages = fgr.Sum(x => Convert.ToInt32(x.Count_Messages ?? 0)),
                TotalCost = fgr.Sum(x => Convert.ToInt32(x.Total_Cost ?? 0)),
                CountPersFirst = fgr.Sum(x => Convert.ToInt32(x.Count_Pers_First ?? 0))
            };
    }
}