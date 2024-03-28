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
    public class OpedCollector : BaseReportCollector
    {
        public OpedCollector() : base(ReportType.Oped)
        {
        }
        private readonly string _connStr = Settings.Default.ConnStr;
        public override AbstractReport CollectSummaryReport(string[] filials, string yymmStart, string yymmEnd,
            ReportStatus status)
        {
            try
            {

                var db = new LinqToSqlKmsReportDataContext(_connStr);
                var table = db.OpedNorm(yymmStart, yymmEnd, filials.First());

                var data = from t in table

                           select new ReportOpedDto
                           {
                               RowNum = t.RowNum,
                               App = t.App ?? 0,
                               Ks = t.Ks ?? 0,
                               Ds = t.Ds ?? 0,
                               Smp = t.Smp ?? 0
                           };

                var outReport = new ReportOped { ReportDataList = new List<ReportOpedDto>() };
                foreach (var theme in data)
                {
                    var reportOpedDto = new ReportOpedDto
                    {
                        RowNum = theme.RowNum,
                        App = theme.App,
                        Ks = theme.Ks,
                        Ds = theme.Ds,
                        Smp = theme.Smp
                    };
                    outReport.ReportDataList.Add(reportOpedDto);

                }
                return outReport;


            }
            catch (Exception e)
            {
                Log.Error(e, "Error collecting summary Oped");
                throw;
            }
        }
    }
}