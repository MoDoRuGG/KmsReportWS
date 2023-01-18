using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class FOpedUHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private string _themeName = "opedU";

       

        public FOpedUHandler(ReportType reportType)
            : base(reportType)
        {
            if (reportType == ReportType.OpedU)
            {
                _themeName = "opedU";
            } else
            {
                _themeName = "";

            }
        }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportOpedU ??
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

            db.Report_OpedU.InsertAllOnSubmit(MapReportFromPersist(report, themeData.Id));
            db.SubmitChanges();
          

        }

        private Report_OpedU MapMainThemeFromPersist(int idThemeData, ReportOpedUDataDto data)
        {
            if (data != null)
            {
                return new Report_OpedU
                {
                    RowNum = data.RowNum,
                    App = data.App,
                    Ks = data.Ks,
                    Ds = data.Ds,
                    Smp = data.Smp,
                    Notes = data.Notes,
                    Id_Report_Data = idThemeData
                };
            }


            return new Report_OpedU
            {
                
                App = 0,
                Ks = 0,
                Ds = 0,
                Smp = 0,
                Notes = "",
                Id_Report_Data = idThemeData
            };


        }

        //protected List<Report_OpedU> MapReportFromPersist(ReportOpedUDto rep, int idReportData)

        //{
        //    var result = new List<Report_OpedU>();

        //    foreach (var row in rep.ReportDataList)
        //    {
        //        result.Add(new Report_OpedU
        //        {
        //            RowNum = row.rowNum,
        //            App = row.App,
        //            Ks = row.Ks,
        //            Ds = row.Ds,
        //            Smp = row.Smp,
        //            //AppOnco = row.AppOnco,
        //            //KsOnco = row.KsOnco,
        //            //DsOnco = row.DsOnco,
        //            //SmpOnco = row.SmpOnco,
        //            //AppLeth = row.AppLeth,
        //            //KsLeth = row.KsLeth,
        //            //DsLeth = row.DsLeth,
        //            //SmpLeth = row.SmpLeth,
        //            Notes = row.Notes

        //        });

        //    }

        //    return result;

        //}

        private Report_OpedU MapReportFromPersist(ReportOpedUDataDto data, int idReportData)
        {
            return new Report_OpedU
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


        private ReportOpedUDataDto MapReportFromPersist(Report_OpedU data)
        {
            return new ReportOpedUDataDto

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
            var report = inReport as ReportOpedU ??
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
                var opedU = db.Report_OpedU.SingleOrDefault(x => x.RowNum == row.RowNum && x.Id_Report_Data == idTheme);
                if (opedU != null)
                {
                    opedU.App = row.App;
                    opedU.Ks = row.Ks;
                    opedU.Ds = row.Ds;
                    opedU.Smp = row.Smp;
                    //opedU.AppOnco = row.AppOnco;
                    //opedU.KsOnco = row.KsOnco;
                    //opedU.DsOnco = row.DsOnco;
                    //opedU.SmpOnco = row.SmpOnco;
                    //opedU.AppLeth = row.AppLeth;
                    //opedU.KsLeth = row.KsLeth;
                    //opedU.DsLeth = row.DsLeth;
                    //opedU.SmpLeth = row.SmpLeth;
                    opedU.Notes = row.Notes;
                }
                else
                {
                    var opedUIns = MapReportFromPersist(row, idTheme);
                    db.Report_OpedU.InsertOnSubmit(opedUIns);

                }
            }

            db.SubmitChanges();
        }


        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportOpedU { ReportDataList = new List<ReportOpedUDto>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                var dataList = themeData.Report_OpedU.Select(MapReportFromPersist).ToList();
                outReport.ReportDataList.AddRange(dataList);
            }

            return outReport;
        }


    }
}