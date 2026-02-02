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
    public class ReportT6StudentsHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportT6StudentsHandler() : base(ReportType.T6Students)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport report) { }

        public ReportT6StudentsDataDto GetYearData(string yymm, string theme, string filial)
        {
            return null;
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportT6Students ??
                      throw new Exception("Error saving new report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data
                {

                    Id_Flow = flow.Id,
                    Id_Report = flow.Id_Report_Type,
                    Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var f6StudentsList = MapMainThemeFromPersist(themeData.Id, reportForms.Data);
                if (f6StudentsList != null)
                {
                    db.Report_T6Students.InsertOnSubmit(f6StudentsList);
                }

                db.SubmitChanges();
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportT6Students { ReportDataList = new List<ReportT6StudentsDto>() };
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();

                var dto = new ReportT6StudentsDto
                {
                    Theme = theme,
                    Data = new ReportT6StudentsDataDto(),
                };


                var dataList = themeData.Report_T6Students.Select(MapReportDto);
                dto.Data = dataList.FirstOrDefault();

                if (dto.Data != null)
                {
                    outReport.ReportDataList.Add(dto);
                }
            }

            return outReport;
        }

        private Report_T6Students MapMainThemeFromPersist(int idThemeData, ReportT6StudentsDataDto data)
        {
            if (data != null)
            {
                return new Report_T6Students
                {
                    Id = data.Id,
                    CountUniversity = data.CountUniversity,
                    CountCollege = data.CountCollege,
                    CountInsured = data.CountInsured,
                    Comments = data.Comments,
                    Id_Report_Data = idThemeData
                };
            }


            return new Report_T6Students
            {
                Id = 0,
                CountUniversity = 0,
                CountCollege = 0,
                CountInsured = 0,
                Comments = "",
                Id_Report_Data = idThemeData
            };


        }


        private ReportT6StudentsDataDto MapReportDto(Report_T6Students report) =>

        new ReportT6StudentsDataDto
        {
            Id = report.Id,
            CountUniversity = report.CountUniversity ?? 0,
            CountCollege = report.CountCollege ?? 0,
            CountInsured = report.CountInsured ?? 0,
            Comments = report.Comments ?? ""
        };


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportT6Students ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id ?? 0;
                if (idTheme == 0)
                {
                    Log.Error(
                        $"Error getting data. idTheme = 0; IdFlow = {inReport.IdFlow}, Theme = {reportForms.Theme}");
                    continue;
                }


                var row = db.Report_T6Students
                       .SingleOrDefault(x => x.Id_Report_Data == idTheme);

                if (report != null)
                {
                    row.CountUniversity = reportForms.Data.CountUniversity;
                    row.CountCollege = reportForms.Data.CountCollege;
                    row.CountInsured = reportForms.Data.CountInsured;
                    row.Comments = reportForms.Data.Comments;
            

                }
                else
                {
                    var rep = MapMainThemeFromPersist(idTheme, reportForms.Data);
                    db.Report_T6Students.InsertOnSubmit(rep);
                }


                db.SubmitChanges();


            }
        }
    }
}