using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Handler
{
    public class ReportVaccinationHander : BaseReportHandler
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;
        private string theme = "Вакцинация";

        public ReportVaccinationHander(ReportType reportType) : base(reportType)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        public ReportVaccination GetYearData(string yymm, string fillial)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Vaccination.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial         
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            ).GroupBy(x => x.Report_Data.Report_Flow.Id_Region).
            Select(x => new ReportVaccination
            {
                M18_39 = x.Sum(g => g.m_18_39),
                M40_59 = x.Sum(g => g.m_40_59),
                M60_65 = x.Sum(g => g.m_60_65),
                M66_74 = x.Sum(g => g.m_66_74),
                M75_More = x.Sum(g => g.m_75_more),

                W18_39 = x.Sum(g => g.w_18_39),
                W40_54 = x.Sum(g => g.w_40_54),
                W55_65 = x.Sum(g => g.w_55_65),
                W66_74 = x.Sum(g => g.w_66_74),
                W75_More = x.Sum(g => g.w_75_more)


            }).FirstOrDefault();

            return result;

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportVaccination ??
                      throw new Exception("Error saving new report, because getting empty report");


            var themeData = new Report_Data
            {

                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = theme
            };
            db.Report_Data.InsertOnSubmit(themeData);
            db.SubmitChanges();

            report.IdReportData = themeData.Id;

            db.Report_Vaccination.InsertOnSubmit(MapReportFromData(report));


            db.SubmitChanges();


        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportVaccination();
            MapFromReportFlow(rep, outReport);

            var db = new LinqToSqlKmsReportDataContext(_connStr);

            var report = db.Report_Vaccination.FirstOrDefault(x=>x.Report_Data.Id_Flow == rep.Id);
            outReport = MapReportDto(report);
            return outReport;
        }
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportVaccination ??
                         throw new Exception("Error update report, because getting empty report");

            var reportDb = db.Report_Vaccination.FirstOrDefault(x => x.Id == report.Id);

            if (reportDb != null)
            {
                reportDb.m_18_39 = report.M18_39;
                reportDb.m_40_59 = report.M40_59;
                reportDb.m_60_65 = report.M60_65;
                reportDb.m_66_74 = report.M66_74;
                reportDb.m_75_more = report.M75_More;
                reportDb.w_18_39 = report.W18_39;
                reportDb.w_40_54 = report.W40_54;
                reportDb.w_55_65 = report.W55_65;
                reportDb.w_66_74 = report.W66_74;
                reportDb.w_75_more = report.W75_More;
            }
            else
            {
                db.Report_Vaccination.InsertOnSubmit(MapReportFromData(report));
            }


            db.SubmitChanges();


        }


        private Report_Vaccination MapReportFromData(ReportVaccination report)
        {
            return new Report_Vaccination
            {
                Id = report.Id,
                Id_Report_data = report.IdReportData,
                m_18_39 = report.M18_39,
                m_40_59 = report.M40_59,
                m_60_65 = report.M60_65,
                m_66_74 = report.M66_74,
                m_75_more = report.M75_More,
                w_18_39 = report.W18_39,
                w_40_54 = report.W40_54,
                w_55_65 = report.W55_65,
                w_66_74 = report.W66_74,
                w_75_more = report.W75_More

            };
        }




        private ReportVaccination MapReportDto(Report_Vaccination report) =>

       new ReportVaccination
       {
           Id = report.Id,
           IdReportData = report.Id_Report_data,
           M18_39 = report.m_18_39,
           M40_59 = report.m_40_59,
           M60_65 = report.m_60_65,
           M66_74 = report.m_66_74,
           M75_More = report.m_75_more,
           W18_39 = report.w_18_39,
           W40_54 = report.w_40_54,
           W55_65 = report.w_55_65,
           W66_74 = report.w_66_74,
           W75_More = report.w_75_more

       };


    }



}


