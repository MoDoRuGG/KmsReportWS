using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.BaseReport
{
    public class MonthlyVolCollector : BaseReportCollector
    {
        public MonthlyVolCollector() : base(ReportType.MonthlyVol)
        {
        }
        private readonly string _connStr = Settings.Default.ConnStr;
        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status)
        {
            try
            {

                var db = new LinqToSqlKmsReportDataContext(_connStr);
                var table = db.MonthlyVol(yymmStart, yymmEnd, filials.First());

                var data = from t in table

                           select new ReportMonthlyVolDto
                           {
                               RowNum = t.RowNum,
                               CountSluch = t.CountSluch ?? 0,
                               CountAppliedSluch = t.CountAppliedSluch ?? 0,
                               CountSluchMEE = t.CountSluchMEE ?? 0,
                               CountSluchEKMP = t.CountSluchEKMP ?? 0
                           };

                var outReport = new ReportMonthlyVol { ReportDataList = new List<ReportMonthlyVolDto>() };
                foreach (var theme in data)
                {
                    var reportMonthlyVolDto = new ReportMonthlyVolDto
                    {
                        RowNum = theme.RowNum,
                        CountSluch = theme.CountSluch,
                        CountAppliedSluch = theme.CountAppliedSluch,
                        CountSluchMEE = theme.CountSluchMEE,
                        CountSluchEKMP = theme.CountSluchEKMP
                    };
                    outReport.ReportDataList.Add(reportMonthlyVolDto);

                }
                return outReport;


            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary MonthlyVol");
                throw;
            }
        }
    }
}