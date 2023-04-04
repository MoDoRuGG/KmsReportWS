using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class ReportOpedFinanceHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private string _themeName = "oped_finance";


        public ReportOpedFinanceHandler(ReportType reportType)
          : base(reportType)
        {

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportOpedFinance ??
                        throw new Exception("Error saving new report, because getting empty report");

            var themeData = new Report_Data
            {
                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = _themeName,
                General_field_1 = 0,
                General_field_2 = 0
            };
            db.Report_Data.InsertOnSubmit(themeData);


            db.SubmitChanges();

            db.Report_OpedFinance.InsertAllOnSubmit(MapReport(report, themeData.Id));
            db.SubmitChanges();
        }


        protected List<Report_OpedFinance> MapReport(ReportOpedFinance report, int idReportData)
        {
            List<Report_OpedFinance> result = new List<Report_OpedFinance>();

            foreach (var item in report.ReportDataList)
            {
                result.Add(new Report_OpedFinance
                {
                    id_ReportData = idReportData,
                    row_num = item.RowNum,
                    value_fact = item.ValueFact,
                    notes = item.Notes
                });
            }


            return result;
        }


        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportOpedFinance { ReportDataList = new List<ReportOpedFinanceData>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                if (outReport.IdReportData != 0)
                {
                    outReport.IdReportData = themeData.Id;
                }
                outReport.ReportDataList.AddRange(themeData.Report_OpedFinance.Select(x => new ReportOpedFinanceData
                {
                    RowNum = x.row_num,
                    ValueFact = x.value_fact,
                    Notes = x.notes
                }));

            }

            return outReport;
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportOpedFinance ??
                             throw new Exception("Error update report, because getting empty report");

            var idTheme = db.Report_Data
                   .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow)?.Id ?? 0;
            if (idTheme == 0)
            {
                Log.Error(
                    $"Error getting data. idTheme = 0; IdFlow = {inReport.IdFlow}");
                return;
            }

            foreach (var row in report.ReportDataList)
            {
                var oped = db.Report_OpedFinance.SingleOrDefault(x => x.row_num == row.RowNum && x.id_ReportData == idTheme);
                if (oped != null)
                {
                    oped.value_fact = row.ValueFact;
                    oped.notes = row.Notes;
                }
                else
                {
                    var rowIns = new ReportOpedFinanceData
                    {
                        RowNum = row.RowNum,
                        ValueFact = row.ValueFact,
                        Notes = row.Notes
                       
                    };

                }
            }

            db.SubmitChanges();
        }
    }


}