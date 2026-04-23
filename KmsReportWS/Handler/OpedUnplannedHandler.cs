using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Handler
{
    public class OpedUnplannedHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private string _themeName = "OpedUnpl";

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }

        public OpedUnplannedHandler(ReportType reportType)
            : base(reportType)
        {
            if (reportType == ReportType.OpedUnpl)
            {
                _themeName = "OpedUnpl";
            }
            else
            {
                _themeName = "";

            }
        }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportOpedUnplanned ??
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

            db.Report_OpedUnplanned.InsertAllOnSubmit(MapReportFromPersist(report, themeData.Id));
            db.SubmitChanges();


        }

        protected List<Report_OpedUnplanned> MapReportFromPersist(ReportOpedUnplanned rep, int idReportData)
        {
            var result = new List<Report_OpedUnplanned>();

            foreach (var row in rep.ReportDataList)
            {
                result.Add(new Report_OpedUnplanned
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

        private Report_OpedUnplanned MapReportFromPersist(ReportOpedUnplannedDto data, int idReportData)
        {
            return new Report_OpedUnplanned
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


        private ReportOpedUnplannedDto MapReportFromPersist(Report_OpedUnplanned data)
        {
            return new ReportOpedUnplannedDto
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
            var report = inReport as ReportOpedUnplanned ??
                             throw new Exception("Error update report, because getting empty report");

            var idTheme = db.Report_Data
                   .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow)?.Id ?? 0;
            if (idTheme == 0)
            {
                Log.Error(
                    $"Error getting data. idTheme = 0; IdFlow = {inReport.IdFlow}");
                return;
            }

            // Создаём НОВЫЙ контекст специально для обновления, чтобы избежать конфликтов отслеживания
            using (var updateDb = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr))
            {
                updateDb.DeferredLoadingEnabled = false;

                var existingData = updateDb.Report_OpedUnplanned
                    .Where(x => x.Id_Report_Data == idTheme)
                    .ToList();

                var clientRowNums = report.ReportDataList.Select(x => x.RowNum).ToHashSet();

                foreach (var row in report.ReportDataList)
                {
                    var existing = existingData.FirstOrDefault(x => x.RowNum == row.RowNum);
                    if (existing != null)
                    {
                        existing.App = row.App;
                        existing.Ks = row.Ks;
                        existing.Ds = row.Ds;
                        existing.Smp = row.Smp;
                        existing.Notes = row.Notes;
                    }
                    else
                    {
                        var newRow = new Report_OpedUnplanned
                        {
                            Id_Report_Data = idTheme,
                            RowNum = row.RowNum,
                            App = row.App,
                            Ks = row.Ks,
                            Ds = row.Ds,
                            Smp = row.Smp,
                            Notes = row.Notes,

                        };
                        updateDb.Report_OpedUnplanned.InsertOnSubmit(newRow);
                    }
                }

                var toDelete = existingData.Where(x => !clientRowNums.Contains(x.RowNum)).ToList();
                updateDb.Report_OpedUnplanned.DeleteAllOnSubmit(toDelete);

                    updateDb.SubmitChanges();
             
            }
        }


        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportOpedUnplanned { ReportDataList = new List<ReportOpedUnplannedDto>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                var dataList = themeData.Report_OpedUnplanned.Select(MapReportFromPersist).ToList();
                outReport.ReportDataList.AddRange(dataList);
            }

            return outReport;
        }


    }
}