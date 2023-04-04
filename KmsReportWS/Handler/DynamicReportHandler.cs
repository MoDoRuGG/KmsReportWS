using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Constructor;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Service;
using KmsReportWS.Support;
using NLog;

namespace KmsReportWS.Handler
{
    public class DynamicReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static readonly string ConnStr = Settings.Default.ConnStr;
        private FileProcessor fileProcessor;

        public DynamicReportHandler()
        {
            fileProcessor = new FileProcessor();
        }

        public ReportDynamicDto GetReportDynamic(int idReport)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var report = db.Report_Dynamic.First(x => x.id == idReport);
            return new ReportDynamicDto
            {
                Id = report.id,
                Name = report.Name,
                Description = report.Description,
                FileName = report.FileName,
                Date = report.Date,
                IsUserRow = report.IsUserRow,
                UserCreated = report.UserCreated,

            };
        }

        public DataTable GetDataTable()
        {
            return new DataTable();
        }

        public List<DynamicDataDto> GetReportRegionData(int idFlow)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            return db.Report_Dynamic_Data.Where(x => x.Id_Flow == idFlow)
                .Select(x => new DynamicDataDto
                {
                    Position = x.Position,
                    Value = x.Value
                }).ToList();
        }

        public Report_Dynamic SaveReport(ReportDynamicDto report)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var report_Dynamic = db.Report_Dynamic.SingleOrDefault(x => x.id == report.Id);
            if (report_Dynamic == null)
            {
                report_Dynamic = new Report_Dynamic
                {
                    Name = report.Name,
                    Date = report.Date,
                    FileName = report.FileName,
                    Description = report.Description,
                    IsUserRow = report.IsUserRow,
                    UserCreated = report.UserCreated
                };

                db.Report_Dynamic.InsertOnSubmit(report_Dynamic);
            }
            else
            {
                report_Dynamic.Name = report.Name;
                report_Dynamic.Date = report.Date;
                report_Dynamic.FileName = report.FileName;
                report_Dynamic.Description = report.Description;
                report_Dynamic.IsUserRow = report.IsUserRow;
                report_Dynamic.UserCreated = report.UserCreated;
            }

            db.SubmitChanges();
            report_Dynamic.FileName = report_Dynamic.id + "_" + report_Dynamic.FileName;
            db.SubmitChanges();
            return report_Dynamic;

        }

        public List<ReportDynamicDto> GetReportDynamic()
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            return db.Report_Dynamic.Select(x => new ReportDynamicDto
            {
                Id = x.id,
                Name = x.Name.Trim(),
                Date = x.Date,
                FileName = x.FileName.Trim(),
                Description = x.Description.Trim(),
                IsUserRow = x.IsUserRow,
                UserCreated = x.UserCreated
            }).ToList();
        }

        public GetDynamicReportResponse GetXmlReport(int reportId)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var report = db.Report_Dynamic.Where(x => x.id == reportId).FirstOrDefault();
            string xmlFileName = report.FileName;
            string XmlText = fileProcessor.ReadXmlDynmaicFile(xmlFileName);
            return new GetDynamicReportResponse
            {
                Id = report.id,
                Xml = XmlText
            };

        }

        public Report_Dynamic_Flow CreateNewFlow(int idUser, string fillialCode, int reportId)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flow = new Report_Dynamic_Flow
            {
                Created = DateTime.Now,
                Id_Employee = idUser,
                Id_Report_Dynamic = reportId,
                Id_Region = fillialCode,
                Status = "Saved"

            };

            db.Report_Dynamic_Flow.InsertOnSubmit(flow);
            db.SubmitChanges();
            return flow;

        }

        public int SaveReportInDb(List<DynamicDataDto> data, int IdReportDynamic, int idUser, string fillialCode)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);

            var currentFlow = db.Report_Dynamic_Flow.FirstOrDefault(x => x.Id_Report_Dynamic == IdReportDynamic && x.Id_Region == fillialCode);


            if (currentFlow == null)
            {
                currentFlow = CreateNewFlow(idUser, fillialCode, IdReportDynamic);
            } else
            {
                currentFlow.Status = "Saved";
                db.SubmitChanges();
            }

            var reportData = db.Report_Dynamic_Data.Where(x => x.Id_Flow == currentFlow.id);

            if (reportData.Count() == 0)
            {
                CreateReportData(data, currentFlow);
            }
            else
            {
                UpdateReportData(data, currentFlow);
            }

            return currentFlow.id;


        }


        public void CreateReportData(List<DynamicDataDto> data, Report_Dynamic_Flow flow)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var result = data.Select(x => new Report_Dynamic_Data
            {
                Id_Flow = flow.id,
                Position = x.Position,
                Value = x.Value
            }).ToList();

            db.Report_Dynamic_Data.InsertAllOnSubmit(result);
            db.SubmitChanges();

        }

        public void UpdateReportData(List<DynamicDataDto> data, Report_Dynamic_Flow flow)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);

            foreach (var dto in data)
            {
                var reportData = db.Report_Dynamic_Data.SingleOrDefault(x => x.Id_Flow == flow.id && x.Position == dto.Position);
                if (reportData != null)
                {
                    reportData.Value = dto.Value;
                }
                else
                {
                    reportData = new Report_Dynamic_Data
                    {
                        Id_Flow = flow.id,
                        Position = dto.Position,
                        Value = dto.Value
                    };

                    db.Report_Dynamic_Data.InsertOnSubmit(reportData);

                }


            }
                   
            db.SubmitChanges();

        }





    }
}