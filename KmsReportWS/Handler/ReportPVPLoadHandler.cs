using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;
using Org.BouncyCastle.Ocsp;

namespace KmsReportWS.Handler
{
    public class ReportPVPLoadHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;
        private string theme = "Нагрузка ПВП";

        public ReportPVPLoadHandler(ReportType reportType) : base(reportType)
        {

        }


        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportPVPLoad ??
                  throw new Exception("Error saving new report, because getting empty report");

            var themeData = new Report_Data
            {

                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = theme
            };
            db.Report_Data.InsertOnSubmit(themeData);
            db.SubmitChanges();

            report.Id_Report_Data = themeData.Id;

            db.Report_PVP_Load.InsertAllOnSubmit(report.Data.Select(x => new LinqToSql.Report_PVP_Load
            {
                Id_Report_Data = themeData.Id,
                RowNumID = x.RowNumID,
                PVP_name = x.PVP_name,
                location_of_the_office = x.location_of_the_office,
                number_of_insured_by_beginning_of_year = x.number_of_insured_by_beginning_of_year,
                number_of_insured_by_reporting_date = x.number_of_insured_by_reporting_date,
                population_dynamics = x.population_dynamics,
                specialist = x.specialist,
                conditions_of_employment = x.conditions_of_employment,
                PVP_plan = x.PVP_plan,
                registered_total_citizens = x.registered_total_citizens,
                newly_insured = x.newly_insured,
                attracted_by_agents = x.attracted_by_agents,
                issued_by_PEO_and_extracts_from_ERZL = x.issued_by_PEO_and_extracts_from_ERZL,
                workload_per_day_for_specialist = x.workload_per_day_for_specialist,
                appeals_through_EPGU = x.appeals_through_EPGU,
                notes = x.notes
            }));

            db.SubmitChanges();


        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportPVPLoad();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);
            var reportRows = db.Report_PVP_Load.Where(x => x.Report_Data.Id_Flow == rep.Id);
            if (reportRows != null)
            {
                outReport.Id_Report_Data = reportRows.ToList().ElementAt(0).Id_Report_Data;
                foreach (var row in reportRows)
                {
                    outReport.Data.Add(new PVPload
                    {
                        RowNumID = row.RowNumID,
                        PVP_name = row.PVP_name,
                        location_of_the_office = row.location_of_the_office,
                        number_of_insured_by_beginning_of_year = row.number_of_insured_by_beginning_of_year,
                        number_of_insured_by_reporting_date = row.number_of_insured_by_reporting_date,
                        population_dynamics = row.population_dynamics,
                        specialist = row.specialist,
                        conditions_of_employment = row.conditions_of_employment,
                        PVP_plan = row.PVP_plan,
                        registered_total_citizens = row.registered_total_citizens,
                        newly_insured = row.newly_insured,
                        attracted_by_agents = row.attracted_by_agents,
                        issued_by_PEO_and_extracts_from_ERZL = row.issued_by_PEO_and_extracts_from_ERZL,
                        workload_per_day_for_specialist = row.workload_per_day_for_specialist,
                        appeals_through_EPGU = row.appeals_through_EPGU,
                        notes = row.notes
                    });
                }

            }
            return outReport;
        }


        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportPVPLoad ??
                     throw new Exception("Error update report, because getting empty report");
            var RowCounter = report.Data.Count();
            var reportDb = db.Report_PVP_Load.Where(x => x.Report_Data.Id_Flow == report.IdFlow);

            var razcount = RowCounter - reportDb.Count();

            foreach (var detail in reportDb)
            {
                db.Report_PVP_Load.DeleteOnSubmit(detail);
            }
            for (var i = 0; i < RowCounter; i++)
            {

                var repIn = report.Data.SingleOrDefault(x => x.RowNumID == i);

                Report_PVP_Load file_row = new Report_PVP_Load
                {
                    Id_Report_Data = report.Id_Report_Data,
                    RowNumID = repIn.RowNumID,
                    PVP_name = repIn.PVP_name,
                    location_of_the_office = repIn.location_of_the_office,
                    number_of_insured_by_beginning_of_year = repIn.number_of_insured_by_beginning_of_year,
                    number_of_insured_by_reporting_date = repIn.number_of_insured_by_reporting_date,
                    population_dynamics = repIn.population_dynamics,
                    specialist = repIn.specialist,
                    conditions_of_employment = repIn.conditions_of_employment,
                    PVP_plan = repIn.PVP_plan,
                    registered_total_citizens = repIn.registered_total_citizens,
                    newly_insured = repIn.newly_insured,
                    attracted_by_agents = repIn.attracted_by_agents,
                    issued_by_PEO_and_extracts_from_ERZL = repIn.issued_by_PEO_and_extracts_from_ERZL,
                    workload_per_day_for_specialist = repIn.workload_per_day_for_specialist,
                    appeals_through_EPGU = repIn.appeals_through_EPGU,
                    notes = repIn.notes
                };

                db.GetTable<Report_PVP_Load>().InsertOnSubmit(file_row);
            }


            db.SubmitChanges();
        }


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportPVPLoad ??
                     throw new Exception("Error update report, because getting empty report");
            var InRepRowCounter = report.Data.Count();
            var reporDb = db.Report_PVP_Load.Where(x => x.Report_Data.Id_Flow == report.IdFlow);

            if (InRepRowCounter != reporDb.Count())
            {
                InsertReport(db, inReport);
            }
            else
            {
                var reportDb = db.Report_PVP_Load.Where(x => x.Report_Data.Id_Flow == report.IdFlow);

                foreach (var rep in reportDb)
                {
                    var repIn = report.Data.FirstOrDefault(x => x.RowNumID == rep.RowNumID);

                    if (repIn != null)
                    {
                        rep.Id_Report_Data = report.Id_Report_Data;
                        rep.RowNumID = repIn.RowNumID;
                        rep.PVP_name = repIn.PVP_name;
                        rep.location_of_the_office = repIn.location_of_the_office;
                        rep.number_of_insured_by_beginning_of_year = repIn.number_of_insured_by_beginning_of_year;
                        rep.number_of_insured_by_reporting_date = repIn.number_of_insured_by_reporting_date;
                        rep.population_dynamics = repIn.population_dynamics;
                        rep.specialist = repIn.specialist;
                        rep.conditions_of_employment = repIn.conditions_of_employment;
                        rep.PVP_plan = repIn.PVP_plan;
                        rep.registered_total_citizens = repIn.registered_total_citizens;
                        rep.newly_insured = repIn.newly_insured;
                        rep.attracted_by_agents = repIn.attracted_by_agents;
                        rep.issued_by_PEO_and_extracts_from_ERZL = repIn.issued_by_PEO_and_extracts_from_ERZL;
                        rep.workload_per_day_for_specialist = repIn.workload_per_day_for_specialist;
                        rep.appeals_through_EPGU = repIn.appeals_through_EPGU;
                        rep.notes = repIn.notes;
                    }

                    db.SubmitChanges();
                }
            }
        }
    }
}