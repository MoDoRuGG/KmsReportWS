using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class ReportInfomaterialHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private string _themeName = "infomaterial";


        public ReportInfomaterialHandler(ReportType reportType)
          : base(reportType)
        {

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportInfomaterial ??
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

            db.Report_Infomaterials.InsertAllOnSubmit(MapReport(report, themeData.Id));
            db.SubmitChanges();
        }


        protected List<Report_Infomaterial> MapReport(ReportInfomaterial report, int idReportData)
        {
            List<Report_Infomaterial> result = new List<Report_Infomaterial>();

            foreach (var item in report.ReportDataList)
            {
                result.Add(new Report_Infomaterial
                {
                    id_ReportData = idReportData,
                    RowNum = item.RowNum,
                    CurrentCount = item.CurrentCount,
                    YearsAmount = item.YearsAmount
                });
            }


            return result;
        }


        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportInfomaterial { ReportDataList = new List<ReportInfomaterialData>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                if (outReport.IdReportData != 0)
                {
                    outReport.IdReportData = themeData.Id;
                }
                outReport.ReportDataList.AddRange(themeData.Report_Infomaterials.Select(x => new ReportInfomaterialData
                {
                    RowNum = x.RowNum,
                    CurrentCount = x.CurrentCount,
                    YearsAmount = x.YearsAmount
                }));

            }

            return outReport;
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportInfomaterial ??
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
                var infomat = db.Report_Infomaterials.SingleOrDefault(x => x.RowNum == row.RowNum && x.id_ReportData == idTheme);
                if (infomat != null)
                {
                    infomat.CurrentCount = row.CurrentCount;
                    infomat.YearsAmount = row.YearsAmount;
                }
                else
                {
                    var rowIns = new ReportInfomaterialData
                    {
                        RowNum = row.RowNum,
                        CurrentCount = row.CurrentCount,
                        YearsAmount = row.YearsAmount
                       
                    };

                }
            }

            db.SubmitChanges();
        }
    }


}