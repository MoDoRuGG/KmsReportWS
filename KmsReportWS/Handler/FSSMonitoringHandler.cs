using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Handler
{
    public class FSSMonitoringHandler : BaseReportHandler
    {

        private string _theme = "Мониторнг";
        private readonly string _connStr = Settings.Default.ConnStr;
        public FSSMonitoringHandler(ReportType reportType) : base(reportType)
        {

        }

        public List<FSSMonitoringPgDataDto> GetPgData(string yymm, string idRegion)
        {
            List<FSSMonitoringPgDataDto> result = new List<FSSMonitoringPgDataDto>();
            using (MsConnection connect = new MsConnection(_connStr))
            {
                connect.NewSp("p_FssMonitoring_Common");
                connect.AddSpParam("@mode", 1);
                connect.AddSpParam("@yymm", yymm);
                connect.AddSpParam("@id_region", idRegion);

                using (var dt = connect.DataTable())
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        result.Add(new FSSMonitoringPgDataDto 
                        {
                            RowNum = row["row_num"].ToString(),
                            ExpertWithEducation = row.ToDecimalNullable("ExpertWithEducation"),
                            ExpertWithoutEducation = row.ToDecimalNullable("ExpertWithoutEducation"),
                            Total = row.ToDecimalNullable("Total"),
                        });
                    }
                }

            }

            return result;
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {

            var report = inReport as Model.Report.ReportFSSMonitroing ??
                     throw new Exception("Error saving new report, because getting empty report");

            var themeData = new Report_Data
            {

                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = _theme
            };
            db.Report_Data.InsertOnSubmit(themeData);
            db.SubmitChanges();

            report.IdReportData = themeData.Id;

            db.FSSMonitroings.InsertAllOnSubmit(report.Data.Select(x => new LinqToSql.FSSMonitroing
            {
                Id_ReportData = themeData.Id,
                RowNum = x.RowNum,
                ExpertWithEducation = x.ExpertWithEducation,
                ExpertWithoutEducation = x.ExpertWithoutEducation
            }));

            db.SubmitChanges();

        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {

            var report = inReport as Model.Report.ReportFSSMonitroing ??
                     throw new Exception("Error saving new report, because getting empty report");

            var reportDb = db.FSSMonitroings.Where(x => x.Report_Data.Id_Flow == inReport.IdFlow);

            foreach (var rep in reportDb)
            {
                var repIn = report.Data.FirstOrDefault(x => x.RowNum == rep.RowNum);

                if (repIn != null)
                {
                    rep.ExpertWithEducation = repIn.ExpertWithEducation;
                    rep.ExpertWithoutEducation = repIn.ExpertWithoutEducation;
                }
                else
                {
                    db.FSSMonitroings.InsertOnSubmit(new LinqToSql.FSSMonitroing
                    {
                        Id_ReportData = report.IdReportData,
                        RowNum = rep.RowNum,
                        ExpertWithEducation = rep.ExpertWithEducation,
                        ExpertWithoutEducation = rep.ExpertWithoutEducation
                    });
                }
            }

            db.SubmitChanges();

        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new Model.Report.ReportFSSMonitroing();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);
            var reportRows = db.FSSMonitroings.Where(x => x.Report_Data.Id_Flow == rep.Id);

            if(reportRows != null)
            {
                outReport.IdReportData = reportRows.ToList().ElementAt(0).Id_ReportData;
                foreach (var rw in reportRows)
                {
                    outReport.Data.Add(new FSSMonitroingData
                    {
                        IdFssMonitoring = rw.Id_FssMonitoring,
                        RowNum = rw.RowNum,
                        ExpertWithEducation = rw.ExpertWithEducation,
                        ExpertWithoutEducation = rw.ExpertWithoutEducation
                    });
                }
            }

           


            return outReport;
        }
    }
}