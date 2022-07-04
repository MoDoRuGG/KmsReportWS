using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Constructor;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Handler
{
    public class ReportDynamicFlowHandler
    {
        private static readonly string ConnStr = Settings.Default.ConnStr;

        public List<ReportDynamicFlowDto> GetReportFlows(int year)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flows = from r in db.Report_Dynamic_Flow
                        join rd in db.Report_Dynamic on r.Id_Report_Dynamic equals rd.id
                        where rd.Date.Year == year
                        select new ReportDynamicFlowDto
                        {
                            IdFlow = r.id,
                            IdRegion = r.Id_Region,
                            IdReport = rd.id,
                            IdUser = r.id,
                            Created = r.Created,
                            Status = StatusUtils.ParseStatus(r.Status)
                        };

            return flows.ToList();
        }

        public ReportDynamicFlowDto GetReportFlow(int idFlow)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flow = (from r in db.Report_Dynamic_Flow
                        join rd in db.Report_Dynamic on r.Id_Report_Dynamic equals rd.id
                        where r.id == idFlow
                        select new ReportDynamicFlowDto
                        {
                            IdFlow = r.id,
                            IdRegion = r.Id_Region,
                            IdReport = rd.id,
                            IdUser = r.id,
                            Created = r.Created,
                            Status = StatusUtils.ParseStatus(r.Status)
                        }).FirstOrDefault();

            return flow;

        }

        public void ChangeStatus(int idFlow, ReportStatus status)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var flow = db.Report_Dynamic_Flow.First(x => x.id == idFlow);
            flow.Status = status.GetDescription();
            db.SubmitChanges();

        }

        public void SaveScan(int idFlow, string fileName)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            db.Scan_Dynamics.InsertOnSubmit(new Scan_Dynamic
            {
                IdFlow = idFlow,
                FileName = fileName
            });

            db.SubmitChanges();

        }
        public void DeleteScan(int idDynamicScan)
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var item = db.Scan_Dynamics.FirstOrDefault(x => x.id_DynamicScan == idDynamicScan);
            if (item != null)
            {
                db.Scan_Dynamics.DeleteOnSubmit(item);
            }

            db.SubmitChanges();
        }

        public List<ReportDynamicScanModel> GetScans(int idFlow)
        {
            List<ReportDynamicScanModel> result = new List<ReportDynamicScanModel>();
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var dbResult = db.Scan_Dynamics.Where(x=> x.IdFlow == idFlow);
            if(dbResult != null)
            {
                result = dbResult.Select(x=> new ReportDynamicScanModel 
                {
                    IdReportDynamicScan = x.id_DynamicScan,
                    idFlow = x.IdFlow,
                    FileName = x.FileName
                }).ToList();
            }



            return result;
        }

    }
}