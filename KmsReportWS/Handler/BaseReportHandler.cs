using System;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using NLog;

namespace KmsReportWS.Handler
{
    public abstract class BaseReportHandler : IReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly ReportType _reportType;
        private readonly string _connStr = Settings.Default.ConnStr;

        protected BaseReportHandler(ReportType reportType)
        {
            _reportType = reportType;
        }

        protected BaseReportHandler()
        {

        }


        protected abstract void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport report);
        protected abstract AbstractReport MapReportFromPersist(Report_Flow rep);

        protected abstract void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport report);

        public AbstractReport GetReport(string filialCode, string yymm)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(_connStr);
                var report = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm && x.Version == 1 &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                return report == null ? null : MapReportFromPersist(report);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error getting report with filialCode = {filialCode} and yymm = {yymm}");
                throw;
            }
        }

        public AbstractReport SaveReportToDb(AbstractReport report, string yymm, int idUser, string filialCode)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(_connStr);

                // todo client has bug with Id_flow. Client send id_flow = 0, when report is saved. this
                // method temporarily fix this bug
                var currentReport = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm  &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                int idFlow = currentReport?.Id ?? 0;

                if (idFlow == 0)
                {
                    var flow = CreateNewFlow(db, yymm, idUser, filialCode);
                    CreateNewReport(db, flow, report);
                }
                else
                {
                    var flow = db.Report_Flow.Single(x => x.Id == idFlow);
                    flow.Id_Employee_Upd = idUser;
                    flow.Updated = DateTime.Today;
                    if (flow.Status != ReportStatus.Scan.GetDescriptionSt())
                    {
                        flow.Status = ReportStatus.Saved.GetDescriptionSt();
                    }
                    //if (flow.DataSource == DataSource.Handle.GetDescriptionDS())
                    //{
                    //    flow.DataSource = DataSource.Excel.GetDescriptionDS();
                    //}

                    UpdateReport(db, report);
                }

                var outReport = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm && x.Version == 1 &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                return MapReportFromPersist(outReport);
            }
            catch (Exception e)
            {
                Log.Error(e,
                    $"Error saving report with filialCode = {filialCode} and yymm = {yymm}, IdFlow = {report.IdFlow}");
                throw;
            }
        }

        public AbstractReport SaveReportDataSourceHandle(AbstractReport report, string yymm, int idUser, string filialCode)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(_connStr);

                // todo client has bug with Id_flow. Client send id_flow = 0, when report is saved. this
                // method temporarily fix this bug
                var currentReport = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                int idFlow = currentReport?.Id ?? 0;

                if (idFlow == 0)
                {
                    var flow = CreateNewFlow(db, yymm, idUser, filialCode);
                    CreateNewReport(db, flow, report);
                }
                else
                {
                    var flow = db.Report_Flow.Single(x => x.Id == idFlow);
                    flow.Id_Employee_Upd = idUser;
                    flow.Updated = DateTime.Today;
                    if (flow.DataSource != DataSource.Handle.GetDescriptionDS())
                    {
                        flow.DataSource = DataSource.Handle.GetDescriptionDS();
                    }

                    UpdateReport(db, report);
                }

                var outReport = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm && x.Version == 1 &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                return MapReportFromPersist(outReport);
            }
            catch (Exception e)
            {
                Log.Error(e,
                    $"Error saving report with filialCode = {filialCode} and yymm = {yymm}, IdFlow = {report.IdFlow}");
                throw;
            }
        }

        public AbstractReport SaveReportDataSourceExcel(AbstractReport report, string yymm, int idUser, string filialCode)
        {
            try
            {
                var db = new LinqToSqlKmsReportDataContext(_connStr);

                // todo client has bug with Id_flow. Client send id_flow = 0, when report is saved. this
                // method temporarily fix this bug
                var currentReport = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                int idFlow = currentReport?.Id ?? 0;

                if (idFlow == 0)
                {
                    var flow = CreateNewFlow(db, yymm, idUser, filialCode);
                    CreateNewReport(db, flow, report);
                }
                else
                {
                    var flow = db.Report_Flow.Single(x => x.Id == idFlow);
                    flow.Id_Employee_Upd = idUser;
                    flow.Updated = DateTime.Today;
                    if (flow.DataSource != DataSource.Excel.GetDescriptionDS())
                    {
                        flow.DataSource = DataSource.Excel.GetDescriptionDS();
                    }

                    UpdateReport(db, report);
                }

                var outReport = db.Report_Flow
                    .SingleOrDefault(x => x.Id_Region == filialCode && x.Yymm == yymm && x.Version == 1 &&
                                          x.Id_Report_Type == _reportType.GetDescriptionSt());
                return MapReportFromPersist(outReport);
            }
            catch (Exception e)
            {
                Log.Error(e,
                    $"Error saving report with filialCode = {filialCode} and yymm = {yymm}, IdFlow = {report.IdFlow}");
                throw;
            }
        }

        protected void MapFromReportFlow(Report_Flow flow, AbstractReport report)
        {
            ReportStatus status = StatusUtils.ParseStatus(flow.Status);
            DataSource datasource = DataSourseUtils.ParseDataSource(flow.DataSource);

            report.IdFlow = flow.Id;
            report.IdType = flow.Id_Report_Type;
            report.Status = status;
            report.Created = flow.Created;
            report.Updated = flow.Updated;
            report.Version = flow.Version ?? 1;
            report.Yymm = flow.Yymm;
            report.Scan = flow.Scan;
            report.Scan2 = flow.Scan2;
            report.Scan3 = flow.Scan3;
            report.RefuseDate = flow.Date_edit_co;
            report.DateIsDone = flow.Date_is_done;
            report.DateToCo = flow.Date_to_co;
            report.IdEmployeeUpd = flow.Id_Employee_Upd ?? 0;
            report.RefuseUser = flow.User_edit_co ?? 0;
            report.UserSubmit = flow.User_submit ?? 0;
            report.UserToCo = flow.User_to_co ?? 0;
            report.IdEmployee = flow.Id_Employee ?? 0;
            report.DataSource = datasource;
        }

        private Report_Flow CreateNewFlow(LinqToSqlKmsReportDataContext db, string yymm, int idUser, string filialCode)
        {
            var flow = new Report_Flow
            {
                Id_Employee = idUser,
                Id_Region = filialCode,
                Yymm = yymm,
                Id_Report_Type = _reportType.GetDescriptionSt(),
                Version = 1,
                Created = DateTime.Today,
                Status = ReportStatus.Saved.GetDescriptionSt(),
                DataSource = DataSource.New.GetDescriptionDS()
            };
            db.Report_Flow.InsertOnSubmit(flow);
            db.SubmitChanges();

            return flow;
        }

        public ReportOpedDto[] GetYearOpedData(string yymm, string filial)
        {
            string start = yymm.Substring(0, 2) + "01";
            var db = new LinqToSqlKmsReportDataContext(_connStr);
            var result = db.Report_Opeds.Where(oped =>
            oped.Report_Data.Report_Flow.Id_Region == filial
            && oped.Report_Data.Report_Flow.Id_Report_Type == "foped"
           && Convert.ToInt32(oped.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
           && Convert.ToInt32(oped.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)).
            GroupBy(oped => oped.RowNum).Select(x => new ReportOpedDto
            {
                RowNum = x.Key,
                App = x.Sum(a => a.App),
                Ks = x.Sum(a => a.Ks),
                Ds = x.Sum(a => a.Ds),
                Smp = x.Sum(a => a.Smp),
                Notes = ""
            });

            return result.ToArray();
        }
    }
}