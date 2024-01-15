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
    public class ReportCadreHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportCadreHandler() : base(ReportType.Cadre)
        {
        }


        public ReportCadreDataDto GetYearData(string yymm, string theme, string fillial)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Cadre.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportCadreDataDto
            {
                count_itog_state = x.Sum(g => g.count_itog_state ?? 0),
                count_itog_fact = x.Sum(g => g.count_itog_fact ?? 0),
                count_itog_vacancy = x.Sum(g => g.count_itog_vacancy ?? 0),
                count_leader_state = x.Sum(g => g.count_leader_state ?? 0),
                count_leader_fact = x.Sum(g => g.count_leader_fact ?? 0),
                count_leader_vacancy = x.Sum(g => g.count_leader_vacancy ?? 0),
                count_deputy_leader_state = x.Sum(g => g.count_deputy_leader_state ?? 0),
                count_deputy_leader_fact = x.Sum(g => g.count_deputy_leader_fact ?? 0),
                count_deputy_leader_vacancy = x.Sum(g => g.count_deputy_leader_vacancy ?? 0),
                count_expert_doctor_state = x.Sum(g => g.count_expert_doctor_state ?? 0),
                count_expert_doctor_fact = x.Sum(g => g.count_expert_doctor_fact ?? 0),
                count_expert_doctor_vacancy = x.Sum(g => g.count_expert_doctor_vacancy ?? 0),
                count_specialist_state = x.Sum(g => g.count_specialist_state ?? 0),
                count_specialist_fact = x.Sum(g => g.count_specialist_fact ?? 0),
                count_specialist_vacancy = x.Sum(g => g.count_specialist_vacancy ?? 0),
                count_grf15 = x.Sum(g => g.count_grf15 ?? 0),
                count_grf16 = x.Sum(g => g.count_grf16 ?? 0),
                count_grf17 = x.Sum(g => g.count_grf17 ?? 0),
                count_grf18 = x.Sum(g => g.count_grf18 ?? 0),
                count_grf19 = x.Sum(g => g.count_grf19 ?? 0),
                count_grf20 = x.Sum(g => g.count_grf20 ?? 0),
                count_grf21 = x.Sum(g => g.count_grf21 ?? 0),
                count_grf22 = x.Sum(g => g.count_grf22 ?? 0),
                count_grf23 = x.Sum(g => g.count_grf23 ?? 0),
                count_grf24 = x.Sum(g => g.count_grf24 ?? 0),
                count_grf25 = x.Sum(g => g.count_grf25 ?? 0),
                count_grf26 = x.Sum(g => g.count_grf26 ?? 0),


            }).FirstOrDefault();

            return result;

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportCadre ??
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

                var fCadreList = MapMainThemeFromPersist(themeData.Id, reportForms.Data);
                if (fCadreList != null)
                {
                    db.Report_Cadre.InsertOnSubmit(fCadreList);
                }

                db.SubmitChanges();
            }
        }


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportCadre ??
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


                var row = db.Report_Cadre
                       .SingleOrDefault(x => x.Id_Report_Data == idTheme);

                if (report != null)
                {
                    row.count_itog_state = reportForms.Data.count_itog_state;
                    row.count_itog_fact = reportForms.Data.count_itog_fact;
                    row.count_itog_vacancy = reportForms.Data.count_itog_vacancy;
                    row.count_leader_state = reportForms.Data.count_leader_state;
                    row.count_leader_fact = reportForms.Data.count_leader_fact;
                    row.count_leader_vacancy = reportForms.Data.count_leader_vacancy;
                    row.count_deputy_leader_state = reportForms.Data.count_deputy_leader_state;
                    row.count_deputy_leader_fact = reportForms.Data.count_deputy_leader_fact;
                    row.count_deputy_leader_vacancy = reportForms.Data.count_deputy_leader_vacancy;
                    row.count_expert_doctor_state = reportForms.Data.count_expert_doctor_state;
                    row.count_expert_doctor_fact = reportForms.Data.count_expert_doctor_fact;
                    row.count_expert_doctor_vacancy = reportForms.Data.count_expert_doctor_vacancy;
                    row.count_specialist_state = reportForms.Data.count_specialist_state;
                    row.count_specialist_fact = reportForms.Data.count_specialist_fact;
                    row.count_specialist_vacancy = reportForms.Data.count_specialist_vacancy;
                    row.count_grf15 = reportForms.Data.count_grf15;
                    row.count_grf16 = reportForms.Data.count_grf16;
                    row.count_grf17 = reportForms.Data.count_grf17;
                    row.count_grf18 = reportForms.Data.count_grf18;
                    row.count_grf19 = reportForms.Data.count_grf19;
                    row.count_grf20 = reportForms.Data.count_grf20;
                    row.count_grf21 = reportForms.Data.count_grf21;
                    row.count_grf22 = reportForms.Data.count_grf22;
                    row.count_grf23 = reportForms.Data.count_grf23;
                    row.count_grf24 = reportForms.Data.count_grf24;
                    row.count_grf25 = reportForms.Data.count_grf25;
                    row.count_grf26 = reportForms.Data.count_grf26;

                }
                else
                {
                    var rep = MapMainThemeFromPersist(idTheme, reportForms.Data);
                    db.Report_Cadre.InsertOnSubmit(rep);
                }


                db.SubmitChanges();


            }
        }
        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportCadre { ReportDataList = new List<ReportCadreDto>() };
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();

                var dto = new ReportCadreDto
                {
                    Theme = theme,
                    Data = new ReportCadreDataDto(),

                };


                var dataList = themeData.Report_Cadre.Select(MapReportDto);
                dto.Data = dataList.First();

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }


        private Report_Cadre MapMainThemeFromPersist(int idThemeData, ReportCadreDataDto data)
        {
            if (data != null)
            {
                return new Report_Cadre
                {
                    Id = data.Id,
                    count_itog_state = data.count_itog_state,
                    count_itog_fact = data.count_itog_fact,
                    count_itog_vacancy = data.count_itog_vacancy,
                    count_leader_state = data.count_leader_state,
                    count_leader_fact = data.count_leader_fact,
                    count_leader_vacancy = data.count_leader_vacancy,
                    count_deputy_leader_state = data.count_deputy_leader_state,
                    count_deputy_leader_fact = data.count_deputy_leader_fact,
                    count_deputy_leader_vacancy = data.count_deputy_leader_vacancy,
                    count_expert_doctor_state = data.count_expert_doctor_state,
                    count_expert_doctor_fact = data.count_expert_doctor_fact,
                    count_expert_doctor_vacancy = data.count_expert_doctor_vacancy,
                    count_specialist_state = data.count_specialist_state,
                    count_specialist_fact = data.count_specialist_fact,
                    count_specialist_vacancy = data.count_specialist_vacancy,
                    count_grf15 = data.count_grf15,
                    count_grf16 = data.count_grf16,
                    count_grf17 = data.count_grf17,
                    count_grf18 = data.count_grf18,
                    count_grf19 = data.count_grf19,
                    count_grf20 = data.count_grf20,
                    count_grf21 = data.count_grf21,
                    count_grf22 = data.count_grf22,
                    count_grf23 = data.count_grf23,
                    count_grf24 = data.count_grf24,
                    count_grf25 = data.count_grf25,
                    count_grf26 = data.count_grf26,

                    Id_Report_Data = idThemeData


                };
            }


            return new Report_Cadre
            {
                Id = 0,
                count_itog_state = 0,
                count_itog_fact = 0,
                count_itog_vacancy = 0,
                count_leader_state = 0,
                count_leader_fact = 0,
                count_leader_vacancy = 0,
                count_deputy_leader_state = 0,
                count_deputy_leader_fact = 0,
                count_deputy_leader_vacancy = 0,
                count_expert_doctor_state = 0,
                count_expert_doctor_fact = 0,
                count_expert_doctor_vacancy = 0,
                count_specialist_state = 0,
                count_specialist_fact = 0,
                count_specialist_vacancy = 0,
                count_grf15 = 0,
                count_grf16 = 0,
                count_grf17 = 0,
                count_grf18 = 0,
                count_grf19 = 0,
                count_grf20 = 0,
                count_grf21 = 0,
                count_grf22 = 0,
                count_grf23 = 0,
                count_grf24 = 0,
                count_grf25 = 0,
                count_grf26 = 0,
                Id_Report_Data = idThemeData
            };


        }


        private ReportCadreDataDto MapReportDto(Report_Cadre report) =>

            new ReportCadreDataDto
            {
                Id = report.Id,
                count_itog_state = report.count_itog_state ?? 0,
                count_itog_fact = report.count_itog_fact ?? 0,
                count_itog_vacancy = report.count_itog_vacancy ?? 0,
                count_leader_state = report.count_leader_state ?? 0,
                count_leader_fact = report.count_leader_fact ?? 0,
                count_leader_vacancy = report.count_leader_vacancy ?? 0,
                count_deputy_leader_state = report.count_deputy_leader_state ?? 0,
                count_deputy_leader_fact = report.count_deputy_leader_fact ?? 0,
                count_deputy_leader_vacancy = report.count_deputy_leader_vacancy ?? 0,
                count_expert_doctor_state = report.count_expert_doctor_state ?? 0,
                count_expert_doctor_fact = report.count_expert_doctor_fact ?? 0,
                count_expert_doctor_vacancy = report.count_expert_doctor_vacancy ?? 0,
                count_specialist_state = report.count_specialist_state ?? 0,
                count_specialist_fact = report.count_specialist_fact ?? 0,
                count_specialist_vacancy = report.count_specialist_vacancy ?? 0,
                count_grf15 = report.count_grf15 ?? 0,
                count_grf16 = report.count_grf16 ?? 0,
                count_grf17 = report.count_grf17 ?? 0,
                count_grf18 = report.count_grf18 ?? 0,
                count_grf19 = report.count_grf19 ?? 0,
                count_grf20 = report.count_grf20 ?? 0,
                count_grf21 = report.count_grf21 ?? 0,
                count_grf22 = report.count_grf22 ?? 0,
                count_grf23 = report.count_grf23 ?? 0,
                count_grf24 = report.count_grf24 ?? 0,
                count_grf25 = report.count_grf25 ?? 0,
                count_grf26 = report.count_grf26 ?? 0,

            };


    }
}