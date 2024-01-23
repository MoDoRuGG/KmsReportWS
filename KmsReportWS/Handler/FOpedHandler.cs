using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class FOpedHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private string _themeName = "oped";

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }

        public FOpedHandler(ReportType reportType)
            : base(reportType)
        {
            if (reportType == ReportType.Oped)
            {
                _themeName = "oped";
            } else
            {
                _themeName = "";

            }
        }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportOped ??
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

            db.Report_Oped.InsertAllOnSubmit(MapReportFromPersist(report, themeData.Id));
            db.SubmitChanges();
          

        }

        protected List<Report_Oped> MapReportFromPersist(ReportOped rep, int idReportData)
        {
            var result = new List<Report_Oped>();

            foreach (var row in rep.ReportDataList)
            {
                result.Add(new Report_Oped
                {
                    Id_Report_Data = idReportData,
                    RowNum = row.RowNum,
                    App = row.App,
                    Ks = row.Ks,
                    Ds = row.Ds,
                    Smp = row.Smp,
                    Notes = row.Notes

                });

            }

            return result;

        }

        private Report_Oped MapReportFromPersist(ReportOpedDto data, int idReportData)
        {
            return new Report_Oped
            {
                Id_Report_Data = idReportData,
                RowNum = data.RowNum,
                App = data.App,
                Ks = data.Ks,
                Ds = data.Ds,
                Smp = data.Smp,
                Notes = data.Notes
            };
        }


        private ReportOpedDto MapReportFromPersist(Report_Oped data)
        {
            return new ReportOpedDto
            {
                RowNum = data.RowNum,
                App = data.App,
                Ks = data.Ks,
                Ds = data.Ds,
                Smp = data.Smp,
                Notes = data.Notes
            };
        }
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportOped ??
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
                var oped = db.Report_Oped.SingleOrDefault(x => x.RowNum == row.RowNum && x.Id_Report_Data == idTheme);
                if (oped != null)
                {
                    oped.App = row.App;
                    oped.Ks = row.Ks;
                    oped.Ds = row.Ds;
                    oped.Smp = row.Smp;
                    oped.Notes = row.Notes;
                }
                else
                {
                    var opedIns = MapReportFromPersist(row, idTheme);
                    db.Report_Oped.InsertOnSubmit(opedIns);

                }
            }

            db.SubmitChanges();
        }


        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportOped { ReportDataList = new List<ReportOpedDto>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                var dataList = themeData.Report_Oped.Select(MapReportFromPersist).ToList();
                outReport.ReportDataList.AddRange(dataList);
            }

            return outReport;
        }


    }
}