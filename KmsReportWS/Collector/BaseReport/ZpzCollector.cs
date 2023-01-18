using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Collector.BaseReport
{
    public class ZpzCollector : BaseReportCollector
    {
        public ZpzCollector(ReportType reportType) : base(reportType)
        {
        }

        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status, DataSource datasource)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(ConnStr);
                var flows = GetFilteredReportFlows(db, filials, yymmStart, yymmEnd, status, datasource);
                var groupTheme = from f in flows
                    group f by f.Theme
                    into fgr
                    select new ReportZpzDto {Theme = fgr.Key, Data = new List<ReportZpzDataDto>()};

                var outReport = new ReportZpz {ReportDataList = new List<ReportZpzDto>()};

                foreach (var theme in groupTheme)
                {
                    var data = CollectReportData(flows, theme.Theme);
                    var reportZpzDto = new ReportZpzDto {Theme = theme.Theme, Data = data.ToList()};
                    outReport.ReportDataList.Add(reportZpzDto);
                }

                return outReport;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary Zpz");
                throw;
            }
        }

        private IQueryable<ReportZpzDataDto> CollectReportData(IQueryable<Report_Data> flows, string theme) =>
            from f in flows.Where(x => x.Theme == theme).SelectMany(x => x.Report_Zpz)
            group f by f.RowNum
            into fgr
            select new ReportZpzDataDto {
                Code = fgr.Key,
                CountSmo = fgr.Sum(x => x.CountSmo ?? 0),
                CountSmoAnother = fgr.Sum(x => x.CountSmoAnother ?? 0),
                CountInsured = fgr.Sum(x => x.CountInsured ?? 0),
                CountInsuredRepresentative = fgr.Sum(x => x.CountInsuredRepresentative ?? 0),
                CountTfoms = fgr.Sum(x => x.CountTfoms ?? 0),
                CountProsecutor = fgr.Sum(x => x.CountProsecutor ?? 0),
                CountOutOfSmo = fgr.Sum(x => x.CountOutOfSmo ?? 0),
                CountAmbulatory = fgr.Sum(x => x.CountAmbulatory ?? 0),
                CountDs = fgr.Sum(x => x.CountDs ?? 0),
                CountDsVmp = fgr.Sum(x => x.CountDsVmp ?? 0),
                CountStac = fgr.Sum(x => x.CountStac ?? 0),
                CountStacVmp = fgr.Sum(x => x.CountStacVmp ?? 0),
                CountOutOfSmoAnother = fgr.Sum(x => x.CountOutOfSmoAnother ?? 0),
                CountAmbulatoryAnother = fgr.Sum(x => x.CountAmbulatoryAnother ?? 0),
                CountDsAnother = fgr.Sum(x => x.CountDsAnother ?? 0),
                CountDsVmpAnother = fgr.Sum(x => x.CountDsVmpAnother ?? 0),
                CountStacAnother = fgr.Sum(x => x.CountStacAnother ?? 0),
                CountStacVmpAnother = fgr.Sum(x => x.CountStacVmpAnother ?? 0)
            };
    }
}