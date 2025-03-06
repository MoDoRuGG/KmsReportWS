using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class MonthlyVolHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private string _themeName = "MonthlyVol";

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }

        public MonthlyVolHandler(ReportType reportType)
            : base(reportType)
        {
            if (reportType == ReportType.MonthlyVol)
            {
                _themeName = "MonthlyVol";
            } else
            {
                _themeName = "";

            }
        }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportMonthlyVol ??
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

            db.Report_MonthlyVol.InsertAllOnSubmit(MapReportFromPersist(report, themeData.Id));
            db.SubmitChanges();
          

        }

        protected List<Report_MonthlyVol> MapReportFromPersist(ReportMonthlyVol rep, int idReportData)
        {
            var result = new List<Report_MonthlyVol>();

            foreach (var row in rep.ReportDataList)
            {
                result.Add(new Report_MonthlyVol
                {
                    Id_Report_Data = idReportData,
                    RowNum = row.RowNum,
                    CountSluch = row.CountSluch,
                    CountAppliedSluch = row.CountAppliedSluch,
                    CountSluchMEE = row.CountSluchMEE,
                    CountSluchEKMP = row.CountSluchEKMP

                });

            }

            return result;

        }

        private Report_MonthlyVol MapReportFromPersist(ReportMonthlyVolDto data, int idReportData)
        {
            return new Report_MonthlyVol
            {
                Id_Report_Data = idReportData,
                RowNum = data.RowNum,
                CountSluch = data.CountSluch,
                CountAppliedSluch = data.CountAppliedSluch,
                CountSluchMEE = data.CountSluchMEE,
                CountSluchEKMP = data.CountSluchEKMP,
            };
        }


        private ReportMonthlyVolDto MapReportFromPersist(Report_MonthlyVol data)
        {
            return new ReportMonthlyVolDto
            {
                RowNum = data.RowNum,
                CountSluch = data.CountSluch,
                CountAppliedSluch = data.CountAppliedSluch,
                CountSluchMEE = data.CountSluchMEE,
                CountSluchEKMP = data.CountSluchEKMP
            };
        }
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportMonthlyVol ??
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
                var monthlyVol = db.Report_MonthlyVol.SingleOrDefault(x => x.RowNum == row.RowNum && x.Id_Report_Data == idTheme);
                if (monthlyVol != null)
                {
                    monthlyVol.CountSluch = row.CountSluch;
                    monthlyVol.CountAppliedSluch= row.CountAppliedSluch;
                    monthlyVol.CountSluchMEE = row.CountSluchMEE;
                    monthlyVol.CountSluchEKMP = row.CountSluchEKMP;
                }
                else
                {
                    var monthlyVolIns = MapReportFromPersist(row, idTheme);
                    db.Report_MonthlyVol.InsertOnSubmit(monthlyVolIns);

                }
            }

            db.SubmitChanges();
        }


        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportMonthlyVol { ReportDataList = new List<ReportMonthlyVolDto>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                var dataList = themeData.Report_MonthlyVol.Select(MapReportFromPersist).ToList();
                outReport.ReportDataList.AddRange(dataList);
            }

            return outReport;
        }


    }
}