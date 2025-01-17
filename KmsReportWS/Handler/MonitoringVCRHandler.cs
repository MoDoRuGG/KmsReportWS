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
    public class MonitoringVCRHandler : BaseReportHandler
    {

        private string _theme = "Мониторинг";
        private readonly string _connStr = Settings.Default.ConnStr;
        public MonitoringVCRHandler(ReportType reportType) : base(reportType)
        {

        }
        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        public List<MonitoringVCRPgDataDto> GetPgData(string yymm, string idRegion)
        {
            List<MonitoringVCRPgDataDto> result = new List<MonitoringVCRPgDataDto>();
            
            using (MsConnection connect = new MsConnection(_connStr))
            {
                // Определяем процедуру в зависимости от значения yymm
                if (int.TryParse(yymm, out int yymmValue) && yymmValue <= 2409)
                {
                    connect.NewSp("p_MonitoringVCR_Common");
                }
                else
                {
                    connect.NewSp("p_MonitoringVCR2025_Common");
                }
                connect.AddSpParam("@mode", 1);
                connect.AddSpParam("@yymm", yymm);
                connect.AddSpParam("@id_region", idRegion);

                using (var dt = connect.DataTable())
                {
                    foreach(DataRow row in dt.Rows)
                    {
                        result.Add(new MonitoringVCRPgDataDto 
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

            var report = inReport as Model.Report.ReportMonitoringVCR ??
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

            db.MonitoringVCR.InsertAllOnSubmit(report.Data.Select(x => new LinqToSql.MonitoringVCR
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

            var report = inReport as Model.Report.ReportMonitoringVCR ??
                     throw new Exception("Error saving new report, because getting empty report");

            var reportDb = db.MonitoringVCR.Where(x => x.Report_Data.Id_Flow == inReport.IdFlow);

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
                    db.MonitoringVCR.InsertOnSubmit(new LinqToSql.MonitoringVCR
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
            var outReport = new ReportMonitoringVCR();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);
            var reportRows = db.MonitoringVCR.Where(x => x.Report_Data.Id_Flow == rep.Id);

            if(reportRows != null)
            {
                outReport.IdReportData = reportRows.ToList().ElementAt(0).Id_ReportData;
                foreach (var rw in reportRows)
                {
                    outReport.Data.Add(new MonitoringVCRData
                    {
                        IdMonitoringVCR = rw.Id_MonitoringVCR,
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