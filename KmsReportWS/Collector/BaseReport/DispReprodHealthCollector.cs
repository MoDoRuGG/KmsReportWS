using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class DispReprodHealthCollector : BaseReportCollector
    {
        public DispReprodHealthCollector(ReportType reportType) : base (reportType) { }

        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status)
        {
            try
            {
                // Создаем контекст базы данных
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                db.CommandTimeout = 1000;

                // Фильтруем отчетные потоки по филиалам и дате
                var flows = GetFilteredReportFlows(db, filials, yymmStart, yymmEnd, status);

                // Группируем потоки по тематике
                var groupTheme = from f in flows
                                 group f by f.Theme into fgr
                                 select new ReportDispReprodHealthDto { Theme = fgr.Key, Data = new List<ReportDispReprodHealthDataDto>() };

                // Инициализируем выходной отчет
                var outReport = new ReportDispReprodHealth { ReportDataList = new List<ReportDispReprodHealthDto>() };

                // Для каждой темы собираем данные отчета
                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportDispReprodHealthDto = new ReportDispReprodHealthDto { Theme = theme.Theme, Data = data.ToList() };
                    outReport.ReportDataList.Add(reportDispReprodHealthDto);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary DispReprodHealth");
                throw;
            }
        }

        private IQueryable<ReportDispReprodHealthDataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_DispReprodHealth)
            group f by f.RowNum into fgr
            select new ReportDispReprodHealthDataDto
            {
                Code = fgr.Key,
                YearlySum = fgr.Sum(x => x.YearlySum ?? 0),
                ForPeriod = fgr.Sum(x => x.ForPeriod ?? 0)
            };
    }
}
